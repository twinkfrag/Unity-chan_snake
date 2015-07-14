using UniRx;
using UnityEngine;

namespace Assets.Scripts
{
	class GodCamera : MonoBehaviour
	{
		void Start()
		{
			var player = Player.Current;

			// ゲーム進行中のみ存在するオブジェクト
			var gameSubscriber = GameMaster.Current.GameSubscriber;
			var updateAsObservable = gameSubscriber.UpdateAsObservable();

			// 移動入力
			updateAsObservable
				.Select(_ => new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")))
				.Where(v => v.magnitude > 0.1)
				.Subscribe(direction =>
				{
					player.GetComponent<Rigidbody>().velocity = direction;
					player.transform.rotation = Quaternion.LookRotation(direction);
				}, Debug.LogException)
				.AddTo(gameSubscriber);

		}
	}
}
