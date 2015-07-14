﻿using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
	class UnityChanCamera : MonoBehaviour
	{
		void Start()
		{
			var player = Player.Current;

			// 自動移動しないと動けない
			player.GetComponent<Rigidbody>().velocity = Vector3.right;

			// ゲーム進行中のみ存在するオブジェクト
			var gameSubscriber = GameMaster.Current.GameSubscriber;
			var updateAsObservable = gameSubscriber.UpdateAsObservable();

			// 移動入力
			updateAsObservable
				.Select(_ => new Vector3(0, Input.GetAxisRaw("Horizontal"), 0))
				.Where(v => v.magnitude > 0.1)
				.Select(v => Vector3.Cross(v, player.GetComponent<Rigidbody>().velocity))
				.Subscribe(direction =>
				{
					player.GetComponent<Rigidbody>().velocity = direction;
					player.transform.rotation = Quaternion.LookRotation(direction);
				}, Debug.LogException)
				.AddTo(gameSubscriber);

		}
	}
}
