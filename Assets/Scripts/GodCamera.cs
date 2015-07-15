using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Scripts
{
	class GodCamera : MonoBehaviour
	{
		void Start()
		{
			var playerRigid = Player.CurrentRigid;

			// ゲーム進行中のみ存在するオブジェクト
			var master = GameMaster.Current;
			var updateAsObservable = master.UpdateAsObservable();

			// 移動入力
			updateAsObservable
				.Select(_ => new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")))
				.Where(v => v.magnitude > 0.1)
				.Subscribe(direction =>
				{
					playerRigid.velocity = direction;
					playerRigid.transform.rotation = Quaternion.LookRotation(direction);
				})
				.AddTo(master);

		}
	}
}
