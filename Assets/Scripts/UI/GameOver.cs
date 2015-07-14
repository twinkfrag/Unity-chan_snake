using UniRx;
using UnityEngine;

namespace Assets.Scripts.UI
{
	public class GameOver : MonoBehaviour
	{
		public void BackToTitle()
		{
			//Application.LoadLevel("Unity-chan_snake_Title");

			Observable
				.Return("申し訳ありません。\nブラウザの更新ボタンを押してください。")
				.SubscribeToText(GetComponentInChildren<UnityEngine.UI.Text>());
		}
	}
}