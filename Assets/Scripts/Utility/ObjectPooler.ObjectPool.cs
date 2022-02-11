using System;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;

//add logging!!!!
//add error handling!!!!

public partial class ObjectPooler
{
	private class UnityObjectPool
	{
		private readonly UnityObject objectToClone;
		private readonly List<UnityObject> objects = new List<UnityObject>();
		private readonly Stack<int> emptyIds = new Stack<int>();

		public UnityObjectPool(UnityObject objectToClone, int poolSize)
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
					emptyIds.Push(i);
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