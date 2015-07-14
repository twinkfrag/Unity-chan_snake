using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts.Title
{
	public class TitleLoad : MonoBehaviour
	{
		[SerializeField]
		private GameObject godCamera;

		public void LoadGodEye()
		{
			Debug.Log("Load God Eye");
			GameMaster.InheritCamera = godCamera;
			Application.LoadLevel("Unity-chan_snake_GodEye");
		}
	}
 
}