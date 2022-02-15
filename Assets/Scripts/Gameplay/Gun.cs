using Ironbelly.Systems;
using UnityEngine;

namespace Ironbelly.Gameplay
{
	public class Gun : MonoBehaviour
	{
		[SerializeField]
		private GameObject BarrelOpening;

		public float BulletsPerSecond = 10;
		private float fireRateTimer = 0;

		private void Update()
		{
			BulletsPerSecond = Mathf.Max(0f, BulletsPerSecond);
			fireRateTimer = Mathf.Max(fireRateTimer - Time.deltaTime, -1f);

			Vector3 gunRotationAmount = new Vector3(
				x: Input.mousePosition.y,
				y: Input.mousePosition.x,
				z: 0f
			);

			//todo fix
	//		transform.rotation = Quaternion.LookRotation(gunRotationAmount, Vector3.right);

			if (Input.GetMouseButton(0) && fireRateTimer <= 0f)
			{
				if (BulletsPerSecond > 0f)
					fireRateTimer = 1f / BulletsPerSecond;

				GameObject bulletGameObject = GameplaySystem.Instance.ObjectPooler.SpawnObject(Constant.ObjectPoolTag.Bullet);
				if (bulletGameObject)
					bulletGameObject.GetComponent<Bullet>().Fire(BarrelOpening.transform.position, transform.rotation);
			}
		}
	}
}