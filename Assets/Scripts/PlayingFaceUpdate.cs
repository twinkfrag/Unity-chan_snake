using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
	public class PlayingFaceUpdate : MonoBehaviour
	{
		[SerializeField]
		private AnimationClip[] animations;

		Animator anim;

		[SerializeField]
		private float delayWeight;

		[SerializeField]
		private bool isKeepFace = false;

		void Start()
		{
			anim = GetComponent<Animator>();
		}

		float current = 0;

		void Update()
		{

			if (Input.GetMouseButton(0))
			{
				current = 1;
			}
			else if (!isKeepFace)
			{
				current = Mathf.Lerp(current, 0, delayWeight);
			}
			anim.SetLayerWeight(1, current);
		}


		//アニメーションEvents側につける表情切り替え用イベントコール
		public void OnCallChangeFace(string str)
		{
			var s = animations
				.Select(a => a.name)
				.FirstOrDefault(n => str == n)
				?? "default@sd_hmd";

			isKeepFace = true;
			current = 1;
			anim.CrossFade(s, 0);
		}
	}
}
