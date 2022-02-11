using System;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;

public partial class ObjectPooler
{
	private class UnityObjectPool<T> where T : UnityObject
	{
		private readonly T objectToClone;
		private readonly List<T> objects = new List<T>();
		private readonly Stack<int> stack = new Stack<int>();

		public UnityObjectPool(T objectToClone, int poolSize)
		{
			this.objectToClone = objectToClone;
			Resize(poolSize);
		}

		public void Resize(int poolSize)
		{
			int originalObjectCount = objects.Count;

			if (poolSize > originalObjectCount)
			{
				for (int i = originalObjectCount; i < poolSize; i++)
				{
					objects.Add(UnityObject.Instantiate(objectToClone));
					stack.Push(i);
				}
				return;
			}

			// there are two options here.
			//	1 - remove only the end of the list. This may involve removing active objects that don't need to be removed
			//  2 - remove inactive objects first, and then the end of the list. Much better!
			// we also have to destroy any removed active objects
			throw new NotImplementedException();
		}

		public void Destroy() => Resize(0);
	}
}