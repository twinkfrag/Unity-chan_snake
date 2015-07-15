using UnityEngine;

namespace Assets.Scripts.Models
{
	/// <summary>
	/// TransformのPositionとRotationを持つ値型
	/// </summary>
	public struct Posture
	{
		public Vector3 Position;
		public Quaternion Rotation;

		public Posture(Vector3 p = default(Vector3), Quaternion r = default(Quaternion))
		{
			Position = p;
			Rotation = r;
		}

		public Posture(Transform t) : this(t.position, t.rotation) { }

		public void ForTransform(Transform t)
		{
			t.position = Position;
			t.rotation = Rotation;
		}

		public override string ToString()
		{
			return string.Format("Pos:{0}, Rot:{1}", Position, Rotation);
		}
	}
}
