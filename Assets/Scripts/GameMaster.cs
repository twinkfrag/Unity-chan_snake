﻿using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Assets.Scripts
{
	public class GameMaster : MonoBehaviour
	{
		public static int Score { get; set; }

		public static GameMaster Current { get; private set; }

		public ObservableUpdateTrigger GameSubscriber { get; private set; }

		[SerializeField]
		private GameObject godCamera;

		public GameObject GodCamera
		{
			get { return godCamera; }
		}

		[SerializeField]
		private GameObject unityChanCamera;

		public GameObject UnityChanCamera
		{
			get { return unityChanCamera; }
		}

		public static Mode GameMode { get; set; }

		public GameMaster()
		{
			Current = this;
		}

		void Awake()
		{
			GameSubscriber = GetComponentInChildren<ObservableUpdateTrigger>();
		}

		void Start()
		{
			Score = 0;

			switch (GameMode)
			{
				case Mode.UnityChan:
					Instantiate(UnityChanCamera).transform.parent = Player.CurrentRigid.transform;
					break;
				default:
					Instantiate(GodCamera).transform.parent = this.transform;
					break;
			}

			var scoreText = GetComponentInChildren<UnityEngine.UI.Text>();
			GameSubscriber.UpdateAsObservable()
				.Select(_ => Score)
				.DistinctUntilChanged()
				.Subscribe(
					s => scoreText.text = string.Format("Score: {0:#,0}", s),
					() =>
					{
						Time.timeScale = 0f;
						Debug.Log("Game Over Complete");
					})
				.AddTo(this);
		}

		public enum Mode
		{
			God, UnityChan
		}
	}
}
