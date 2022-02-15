using Ironbelly.Systems;
using UnityEngine;

namespace Ironbelly.Gameplay
{
	public class Cube : MonoBehaviour, IShootable, IPooledObject
	{
		#region IPooledObject

		public void DespawnObject()
		{
			GameplaySystem.Instance.ObjectPooler.DespawnObject(Constant.ObjectPoolTag.Cube, gameObject);
		}

		public void OnObjectDespawn()
		{
		}

		public void OnObjectSpawned()
		{
		}

		#endregion

		#region IShootable

		public void Shoot()
		{
			DespawnObject();
		}

		#endregion
	}
}