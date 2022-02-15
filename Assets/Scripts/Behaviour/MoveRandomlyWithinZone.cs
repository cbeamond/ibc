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

		public float maxTimeBetweenCourseChange = 5f;
		public float minTimeBetweenCourseChange = 1f;
		private float timeUntilNextCourseChange;

		private Vector3 direction;

		private void Awake()
		{
			moveAmountPerSecond = Random.Range(minMoveSpeedPerSecond, maxMoveSpeedPerSecond);
		}

		private void Start()
		{
			MoveToInsideZone(GameplaySystem.Instance.ContainingZone);
			UpdateCourse();

			void MoveToInsideZone(ContainingZone containingZone)
			{

				Bounds bounds = containingZone.Bounds;

				transform.position = new Vector3
				(
					x: Random.Range(1f, bounds.extents.x - 1f),
					y: Random.Range(1f, bounds.extents.y - 1f),
					z: Random.Range(1f, bounds.extents.z - 1f)
				);
			}
		}

		private void Update()
		{
			if (Time.frameCount < 10)
				return;
			timeUntilNextCourseChange -= Time.deltaTime;
			//if (timeUntilNextCourseChange < 0)
			//	UpdateCourse();

			//transform.position += moveAmountPerSecond * Time.deltaTime * direction;
			transform.position += moveAmountPerSecond * Time.deltaTime * direction.normalized;
				

			if (!GameplaySystem.Instance.ContainingZone.Bounds.Contains(transform.position))
			{
				// If we leave the zone lets simply reverse direction and add extra time to make sure
				// that we don't glitch out of the zone
				direction *= -1;
				timeUntilNextCourseChange += 1f;
			}
		}

		private void UpdateCourse()
		{
			timeUntilNextCourseChange = Random.Range(minTimeBetweenCourseChange, maxTimeBetweenCourseChange);
			direction = new Vector3(
				x: Random.Range(0, 360),
				y: Random.Range(0, 360),
				z: Random.Range(0, 360)
			);
		}

		#region IPooledObject

		public void OnObjectSpawned()
		{
			UpdateCourse();
		}

		public void OnObjectDespawn()
		{
			timeUntilNextCourseChange = 0f;
		}

		#endregion
	}
}