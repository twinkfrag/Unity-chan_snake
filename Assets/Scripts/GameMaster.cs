using UnityEngine;
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
		private GameObject localCamera;

		public GameObject LocalCamera
		{
			get { return localCamera; }
			set { localCamera = value; }
		}

		public static GameObject InheritCamera { get; set; }

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
			Instantiate(InheritCamera ?? LocalCamera).transform.parent = this.transform;

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
	}
}
