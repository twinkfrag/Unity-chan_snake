using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts.Title
{
	public class TitleLoad : MonoBehaviour
	{
		public void LoadGodEye()
		{
			Debug.Log("Load God Eye");
			GameMaster.GameMode = GameMaster.Mode.God;
			Application.LoadLevel("Unity-chan_snake_GodEye");
		}

		public void LoadUnityChanEye()
		{
			Debug.Log("Load Unity-chan Eye");
			GameMaster.GameMode = GameMaster.Mode.UnityChan;
			Application.LoadLevel("Unity-chan_snake_GodEye");
		}
	}
 
}