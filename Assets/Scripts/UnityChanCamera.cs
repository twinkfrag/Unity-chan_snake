using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Assets.Scripts
{
	class UnityChanCamera : MonoBehaviour
	{
		void Start()
		{
			var playerRigid = Player.CurrentRigid;

			// ゲーム進行中のみ存在するオブジェクト
			var master = GameMaster.Current;
			var updateAsObservable = master.UpdateAsObservable();

			// 移動入力
			updateAsObservable
				.Select(_ => new Vector3(0, Input.GetAxisRaw("Horizontal"), 0))
				// 連続入力の防止
				.DistinctUntilChanged()
				.Where(v => v.magnitude > 0.1)
				.ThrottleFirstFrame(5)
				// 現在の方向に対する変化を外積で作る
				.Select(v => Vector3.Cross(v, playerRigid.velocity))
				.Subscribe(direction =>
				{
					playerRigid.velocity = direction;
					playerRigid.transform.rotation = Quaternion.LookRotation(direction);
				})
				.AddTo(master);

		}
	}
}
