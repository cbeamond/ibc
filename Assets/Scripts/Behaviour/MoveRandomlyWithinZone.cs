using Ironbelly.Systems;
using UnityEngine;

//add logging!!!!
//add error handling!!!!

namespace Ironbelly.Behaviour
{
	public class MoveRandomlyWithinZone : MonoBehaviour, IPooledObject
	{
		public float maxMoveSpeedPerSecond = 2f;
		public float minMoveSpeedPerSecond = 6f;
		private float moveAmountPerSecond;

		private Vector3 direction;

		private void Awake()
		{
			moveAmountPerSecond = Random.Range(minMoveSpeedPerSecond, maxMoveSpeedPerSecond);
		}

		private void Update()
		{
			if (Time.frameCount < 10)
				return;

			transform.position += moveAmountPerSecond * Time.deltaTime * direction.normalized;

			if (!GameplaySystem.Instance.ContainingZone.Bounds.Contains(transform.position))
			{
				// If we leave the zone lets simply reverse direction and add extra time to make sure
				// that we don't glitch out of the zone
				direction *= -1;
			}
		}

		private void UpdateCourse()
		{
			direction = new Vector3(
				x: Random.Range(0, 360),
				y: Random.Range(0, 360),
				z: Random.Range(0, 360)
			);
		}

		private void MoveToInsideZone(ContainingZone containingZone)
		{
			Bounds bounds = containingZone.Bounds;

			transform.position = new Vector3
			(
				x: Random.Range(-bounds.extents.x + bounds.extents.x * 0.05f, bounds.extents.x - bounds.extents.x * 0.05f),
				y: Random.Range(-bounds.extents.y + bounds.extents.y * 0.05f, bounds.extents.y - bounds.extents.y * 0.05f),
				z: Random.Range(-bounds.extents.z + bounds.extents.z * 0.05f, bounds.extents.z - bounds.extents.z * 0.05f)
			);
		}

		#region IPooledObject

		public void OnObjectSpawned()
		{
			MoveToInsideZone(GameplaySystem.Instance.ContainingZone);
			UpdateCourse();
		}

		public void OnObjectDespawn()
		{
		}

		#endregion
	}
}