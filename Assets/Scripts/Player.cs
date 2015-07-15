using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Unity.Linq;
using Assets.Scripts.Models;

namespace Assets.Scripts
{
	class Player : MonoBehaviour
	{
		[SerializeField]
		private GameObject tailPrefab;

		public GameObject TailPrefab
		{
			get { return tailPrefab; }
		}

		public static Rigidbody CurrentRigid { get; private set; }

		private Coroutine currentTailCoroutine;

		private readonly List<IList<Posture>> postureHistory = new List<IList<Posture>>();

		void Awake()
		{
			CurrentRigid = GetComponent<Rigidbody>();
		}

		void Start()
		{
			// ゲーム進行中のみ存在するオブジェクト
			var gameSubscriber = GameMaster.Current.GameSubscriber;
			var updateAsObservable = gameSubscriber.UpdateAsObservable();

			// PostureHistory(位置のログ)の生成
			updateAsObservable
				.Select(_ => new Posture(this.transform))
				.Buffer(40)
				// ついでにスコアを加算
				.Do(_ => GameMaster.Score += 1)
				.Subscribe(postureHistory.Add)
				.AddTo(gameSubscriber);

			// trigger
			var trigger = this.OnTriggerEnterAsObservable();

			// アイテム判定
			trigger
				.Where(c => c.name.Contains("Food"))
				.Subscribe(c =>
				{
					GameMaster.Score += 1000;
					c.GetComponent<Food>().Next();
					c.gameObject.Destroy();

					StopCoroutine(currentTailCoroutine);
					gameObject.Parent().Add(TailPrefab);
					currentTailCoroutine = StartCoroutine(HistoryToTail(postureHistory));
				})
				.AddTo(gameSubscriber);

			// 一つ目の追尾オブジェクト
			gameObject.AddAfterSelf(TailPrefab).name = "Neck";

			// 追尾開始
			currentTailCoroutine = StartCoroutine(HistoryToTail(postureHistory));

			// GameOver判定
			trigger
				.Where(c => c.name.Contains("Wall") || c.name.Contains("Tail"))
				.Select(_ => Unit.Default)
				.Subscribe(_ =>
				{
					CurrentRigid.velocity = Vector3.zero;
					gameSubscriber.gameObject.Destroy();
					Application.LoadLevelAdditive("Unity-chan_snake_GameOver");
				})
				.AddTo(this);
		}

		IEnumerator HistoryToTail(IList<IList<Posture>> history)
		{
			yield return null;
			gameObject
				.AfterSelf()
				.Zip(history.Reverse(), (tail, buf) => new { tail, buf })
				.Select(t => Observable.FromCoroutine(() => BufferToTail(t.tail.transform, t.buf)))
				.WhenAll()
				.Subscribe(
					onNext: _ =>
					{
					},
					onCompleted: () =>
					{
						currentTailCoroutine = StartCoroutine(HistoryToTail(postureHistory));
					})
				.AddTo(GameMaster.Current.GameSubscriber)
				;
			Debug.Log("yield return end");
		}

		static IEnumerator BufferToTail(Transform t, IList<Posture> buf)
		{
			foreach (var pos in buf)
			{
				pos.ForTransform(t);
				yield return null;
			}
		}
	}
}
