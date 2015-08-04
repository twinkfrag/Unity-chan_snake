using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class GameOver : MonoBehaviour
	{
		void Start()
		{
			GetComponentsInChildren<UnityEngine.UI.Text>()
				.First(x => x.name == "ScoreText")
				.text = string.Format("Score: {0}", GameMaster.Score);
		}

		public void BackToTitle()
		{
			Application.LoadLevel("Unity-chan_snake_Title");
		}
	}
}