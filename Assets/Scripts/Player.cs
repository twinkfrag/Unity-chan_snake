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
		private GameObject tailPrefab = null;

		public GameObject TailPrefab
		{
			get { return tailPrefab; }
		}

		private readonly List<IList<Posture>> postureHistory = new List<IList<Posture>>();

		void Start()
		{
			var playingAsObservable = GameMaster.Current.GameSubscriber.UpdateAsObservable();

			// GameOver時に解除するObservable
			var playingObservables = new CompositeDisposable().AddTo(this);

			// PostureAsObservableの生成
			var postureAsObservable = playingAsObservable
				.Select(_ => new Posture(this.transform))
				.Buffer(40);

			postureAsObservable
				.Subscribe(postureHistory.Add)
				.AddTo(playingObservables);

			// PostureAsObservableのログ
			//postureAsObservable
			//	.Select(x => x.First())
			//	.Subscribe(p => Debug.Log(p))
			//	.AddTo(playingObservables);

			// 移動入力
			playingAsObservable
				.Select(_ => new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")))
				.Where(v => v.magnitude > 0.1)
				.Subscribe(direction =>
				{
					GetComponent<Rigidbody>().velocity = direction;
					this.transform.rotation = Quaternion.LookRotation(direction);
				}, Debug.LogException)
				.AddTo(playingObservables);

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

					postureHistory
						.Skip(postureHistory.Count - 2)
						.First()
						.First()
						.ForTransform(gameObject.Parent().Add(TailPrefab).transform);
				})
				.AddTo(playingObservables);

			gameObject.AddAfterSelf(TailPrefab).name = "Neck";
			StartCoroutine(HistoryToTail(postureHistory));

			// GameOver判定
			trigger
				.Where(c => c.name.Contains("Wall") || c.name.Contains("Tail"))
				.Select(_ => Unit.Default)
				.Subscribe(_ =>
				{
					playingObservables.Dispose();
					GameMaster.Current.GameSubscriber.gameObject.Destroy();
				})
				.AddTo(this);
		}

		IEnumerator HistoryToTail(IList<IList<Posture>> history)
		{
			yield return null;
			var zip = gameObject
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
						StartCoroutine(HistoryToTail(postureHistory));
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
