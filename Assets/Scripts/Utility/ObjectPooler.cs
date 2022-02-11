using System;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;

//add logging!!!!

public partial class ObjectPooler
{
	private readonly Dictionary<Type, UnityObjectPool<UnityObject>> unityObjectPools
		= new Dictionary<Type, UnityObjectPool<UnityObject>>();

	public void CreatePool<T>(T objectToClone, int poolSize) where T : UnityObject
	{
		Type poolType = typeof(T);
		if (!unityObjectPools.ContainsKey(poolType))
			unityObjectPools.Add(poolType, new UnityObjectPool<T>(objectToClone, poolSize));

		ResizePool<T>(poolSize);
	}

	public void ResizePool<T>(int poolSize)
	{
		if (unityObjectPools.TryGetValue(typeof(T), out var pool))
			pool.Resize(poolSize);
	}

	public void DestroyPool<T>()
	{
		Type poolType = typeof(T);

		if (unityObjectPools.TryGetValue(poolType, out var pool))
		{
			pool.Destroy();
			unityObjectPools.Remove(poolType);
		}
	}
}