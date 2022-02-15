using UnityEngine;

namespace Ironbelly.Behaviour
{
	public class ContainingZone : MonoBehaviour
	{
		public float X = 50f;
		public float Y = 50f;
		public float Z = 50f;

		public Bounds Bounds => new Bounds(
			center: Vector3.zero,
			size: new Vector3(X, Y, Z)
		);
	}
}