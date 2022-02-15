using System.Collections.Generic;
using UnityEngine;

//add logging!!!!
//add error handling!!!!

namespace Ironbelly.Systems
{
	public partial class ObjectPooler
	{
		private readonly Dictionary<string, GameObjectPool> gameObjectPools
			= new Dictionary<string, GameObjectPool>();

		public void CreatePool(string tag, int poolSize, GameObject objectToClone)
		{
			if (!gameObjectPools.ContainsKey(tag))
				gameObjectPools.Add(tag, new GameObjectPool(objectToClone, poolSize));

			ResizePool(tag, poolSize);
		}

		public void ResizePool(string tag, int poolSize)
		{
			if (gameObjectPools.TryGetValue(tag, out GameObjectPool pool))
				pool.Resize(poolSize);
		}

		public void DestroyPool(string tag)
		{
			if (gameObjectPools.TryGetValue(tag, out GameObjectPool pool))
			{
				pool.Destroy();
				gameObjectPools.Remove(tag);
			}
		}

		public GameObject SpawnObject(string tag)
		{
			if (gameObjectPools.TryGetValue(tag, out GameObjectPool pool))
				return pool.SpawnObject();
			return default;
		}

		public void DespawnObject(string tag, GameObject gameObject)
		{
			if (gameObjectPools.TryGetValue(tag, out GameObjectPool pool))
				pool.DespawnObject(gameObject);
		}
	}
}