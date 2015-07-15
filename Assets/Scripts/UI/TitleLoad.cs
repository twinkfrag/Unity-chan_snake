using UnityEngine;

namespace Assets.Scripts.UI
{
	public class TitleLoad : MonoBehaviour
	{
		public void LoadGodEye()
		{
			GameMaster.GameMode = GameMaster.Mode.God;
			Application.LoadLevel("Unity-chan_snake");
		}

		public void LoadUnityChanEye()
		{
			GameMaster.GameMode = GameMaster.Mode.UnityChan;
			Application.LoadLevel("Unity-chan_snake");
		}
	}
 
}