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
			// PostureAsObservableの生成
			PostureAsObservable = this.UpdateAsObservable()
				.Select(_ => new Posture(this.transform));

			// PostureAsObservableのログ
			PostureAsObservable.Subscribe(p => Debug.Log(p))
				.AddTo(this);

			// 移動入力
			this.UpdateAsObservable()
				.Select(_ => new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")))
				.Where(v => v.magnitude > 0.1)
				.Subscribe(v =>
				{
					GetComponent<Rigidbody>().velocity = v;
					this.transform.rotation = Quaternion.LookRotation(v);
				}, Debug.LogException)
				.AddTo(this);

			// trigger
			var trigger = this.GetComponent<ObservableTriggerTrigger>();
			if (trigger == null)
			{
				Debug.LogError("must set ObservableTriggerTrigger to Player");
				return;
			}

			trigger.OnTriggerEnterAsObservable()
				   .Where(c => c.gameObject.name.Contains("Food"))
				   .Subscribe(c =>
				   {
					   c.GetComponent<Food>().Next();
					   Destroy(c.gameObject);
				   })
				   .AddTo(this);
		}
	}
}
