using UnityEngine;
using UniRx;
using UniRx.Triggers;
// ReSharper disable UnusedMember.Local
// ReSharper disable ArrangeThisQualifier
// ReSharper disable UseStringInterpolation

namespace Assets.Scripts
{
	public class GameMaster : MonoBehaviour
	{
		public static int Score { get; set; }

		[SerializeField]
		private GameObject localCamera;

		public GameObject LocalCamera
		{
			get { return localCamera; }
			set { localCamera = value; }
		}

		public static GameObject InheritCamera { get; set; }

		void Start()
		{
			Score = 0;
			Instantiate(InheritCamera ?? LocalCamera).transform.parent = this.transform;

			var scoreText = GetComponentInChildren<UnityEngine.UI.Text>();
			this.UpdateAsObservable()
				.Select(_ => Score)
				.DistinctUntilChanged()
				.Subscribe(s => scoreText.text = string.Format("Score: {0:#,0}", s))
				.AddTo(this);

		}
	}
}
