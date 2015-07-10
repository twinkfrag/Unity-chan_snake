using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Assets.Scripts.Models;
// ReSharper disable ConvertPropertyToExpressionBody
// ReSharper disable FunctionRecursiveOnAllPaths

namespace Assets.Scripts
{
	class Player : MonoBehaviour
	{
		[SerializeField]
		private GameObject playerPrefab;

		public GameObject PlayerPrefab
		{
			get { return playerPrefab; }
			set { playerPrefab = value; }
		}

		public IObservable<Posture> PostureAsObservable { get; private set; }

		void Start()
		{
			// GameOver時に解除するObservable
			var playingObservables = new CompositeDisposable().AddTo(this);

			// PostureAsObservableの生成
			PostureAsObservable = this.UpdateAsObservable()
				.Select(_ => new Posture(this.transform));

			// PostureAsObservableのログ
			PostureAsObservable
				.Subscribe(p => Debug.Log(p))
				.AddTo(playingObservables);

			// 移動入力
			this.UpdateAsObservable()
				.Select(_ => new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")))
				.Where(v => v.magnitude > 0.1)
				.Subscribe(v =>
				{
					GetComponent<Rigidbody>().velocity = v;
					this.transform.rotation = Quaternion.LookRotation(v);
				}, Debug.LogException)
				.AddTo(playingObservables);

			// trigger
			var trigger = this.GetComponent<ObservableTriggerTrigger>().OnTriggerEnterAsObservable();

			trigger
				.Where(c => c.name.Contains("Food"))
				.Subscribe(c =>
				{
					GameMaster.Score += 1000;
					c.GetComponent<Food>().Next();
					Destroy(c.gameObject);
				})
				.AddTo(playingObservables);

			trigger
				.Where(c => c.name.Contains("Wall") || c.name.Contains("Player"))
				.Subscribe(_ =>
				{
					Debug.Log("Game Over");
					Time.timeScale = 0;
					playingObservables.Dispose();
				})
				.AddTo(this);

			var tail = (GameObject)Instantiate(PlayerPrefab, new Vector3(4.5f, transform.position.y, 4.5f), transform.rotation);
			tail.GetComponent<Player>().enabled = false;
		}
	}
}
