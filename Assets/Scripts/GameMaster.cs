using System.Collections.Generic;
using System.Linq;
using UnityEngine;
// ReSharper disable UnusedMember.Local
// ReSharper disable ArrangeThisQualifier

namespace Assets.Scripts
{
	public class GameMaster : MonoBehaviour
	{
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
			Instantiate(InheritCamera ?? LocalCamera).transform.parent = this.transform;
		}
	}
}
