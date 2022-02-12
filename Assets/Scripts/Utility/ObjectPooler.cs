using System;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;

//add logging!!!!
//add error handling!!!!

namespace Ironbelly.Utility
{
	public partial class ObjectPooler
	{
		private readonly Dictionary<Type, UnityObjectPool> unityObjectPools
			= new Dictionary<Type, UnityObjectPool>();

		public void CreatePool<T>(T objectToClone, int poolSize) where T : UnityObject
		{
			Type poolType = typeof(T);
			if (!unityObjectPools.ContainsKey(poolType))
				unityObjectPools.Add(poolType, new UnityObjectPool(objectToClone, poolSize));

			ResizePool<T>(poolSize);
		}

		public void ResizePool<T>(int poolSize) where T : UnityObject
		{
			if (unityObjectPools.TryGetValue(typeof(T), out var pool))
				pool.Resize(poolSize);
		}

		public void DestroyPool<T>() where T : UnityObject
		{
			Type poolType = typeof(T);

			if (unityObjectPools.TryGetValue(poolType, out var pool))
			{
				pool.Destroy();
				unityObjectPools.Remove(poolType);
			}
		}

		public T GetPooledObject<T>() where T : UnityObject
		{
			if (unityObjectPools.TryGetValue(typeof(T), out var pool))
				return pool.GetPooledObject();
			return default;
		}

		public void DestroyObject<T>(UnityObject unityObject)
		{
			return unityObjectPools[typeof(T)].DestroyObject(unityObject);
		}
	}
}