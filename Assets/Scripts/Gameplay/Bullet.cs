using Ironbelly.Systems;
using UnityEngine;

namespace Ironbelly.Gameplay
{
	public class Bullet : MonoBehaviour, IPooledObject
	{
		public float movementSpeed = 30f;
		public float maxTimeAliveSeconds = 4f;
		private float timeAlive;


		public void Fire(Vector3 position, Quaternion rotation)
		{
			transform.position = position;
			transform.rotation = rotation;
		}

		private void Update()
		{
			timeAlive += Time.deltaTime;
			if (timeAlive > maxTimeAliveSeconds)
				DespawnObject();

			transform.Translate(transform.forward *= movementSpeed * Time.deltaTime);
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.layer == Constant.Layer.Id.Shootable)
			{
				IShootable shootable = other.gameObject.GetComponent<IShootable>();
				if (shootable != null)
				{
					shootable.Shoot();
					DespawnObject();
				}
				return;
			}
		}

		#region IPooledObject

		public void DespawnObject()
		{
			GameplaySystem.Instance.ObjectPooler.DespawnObject(Constant.ObjectPoolTag.Bullet, gameObject);
		}

		public void OnObjectSpawned()
		{
		}

		public void OnObjectDespawn()
		{
		}

		#endregion
	}
}