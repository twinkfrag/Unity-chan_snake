using UnityEngine;
using UniRx;
using UniRx.Triggers;

namespace Assets.Scripts
{
	public class GameMaster : MonoBehaviour
	{
		public static int Score { get; set; }

		public static GameMaster Current { get; private set; }

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

		void Awake()
		{
			Current = this;
		}

		void Start()
		{
			Score = 0;

			switch (GameMode)
			{
				case Mode.UnityChan:
					Instantiate(UnityChanCamera).transform.parent = Player.CurrentRigid.transform;
					break;
				case Mode.God:
					Instantiate(GodCamera).transform.parent = this.transform;
					break;
			}

			this.ObserveEveryValueChanged(_ => Score)
				.SubscribeToText(GetComponentInChildren<UnityEngine.UI.Text>(),
					i => string.Format("Score: {0:#,0}", i))
				.AddTo(this);
		}

		public enum Mode
		{
			God, UnityChan
		}
	}
}
