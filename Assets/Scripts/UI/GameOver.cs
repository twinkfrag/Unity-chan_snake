using UnityEngine;

namespace Assets.Scripts.UI
{
	public class GameOver : MonoBehaviour
	{
		public void BackToTitle()
		{
			Application.LoadLevel("Unity-chan_snake_Title");
		}
	}
}