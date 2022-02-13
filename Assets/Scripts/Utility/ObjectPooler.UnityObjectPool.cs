using System.Linq;
using System;
using System.Collections.Generic;
using UnityObject = UnityEngine.Object;

//add logging!!!!
//add error handling!!!!

namespace Ironbelly.Utility
{
	public partial class ObjectPooler
	{
		private partial class UnityObjectPool
		{
			private readonly UnityObject objectToClone;
			private List<UnityObject> objects = new List<UnityObject>();
			private Stack<int> emptyIds = new Stack<int>();
			private Queue<int> objectRetreveOrder = new Queue<int>();

			public UnityObjectPool(UnityObject objectToClone, int poolSize)
			{
				this.objectToClone = objectToClone;
				Resize(poolSize);
			}

			public void Resize(int poolSize)
			{
				if (poolSize == objects.Count)
					return;

				if (poolSize > objects.Count)
					ResizeLarger(poolSize);
				else
					ResizeSmaller(poolSize);

				void ResizeLarger(int poolSize)
				{
					objects.Capacity = poolSize;
					for (int i = objects.Count; i < poolSize; i++)
					{
						objects.Add(UnityObject.Instantiate(objectToClone));
						emptyIds.Push(i);
						objectRetreveOrder.Enqueue(i);
					}
				}

				void ResizeSmaller(int poolSize)
				{
					int amountOfObjectsToRemove = objects.Count - poolSize;
					List<int> objectIdsToRemove = new List<int>();
					while (true)
					{
						if (amountOfObjectsToRemove == 0)
							break;

						if (emptyIds.Count == 0)
							break;

						int emptyId = emptyIds.Pop();
						objectIdsToRemove.Add(emptyId);
					}

					if (amountOfObjectsToRemove > 0)
						objectIdsToRemove.Union(objectRetreveOrder.Take(amountOfObjectsToRemove));

					foreach (int objectId in objectIdsToRemove)
					{
						UnityObject.Destroy(objects[objectId]);
						objects[objectId] = null;
					}

					for (int index = 0; index < objects.Count; index++)
					{
						if (objects[index] != null)
							continue;


					}

				}
			}

			public void Destroy() => Resize(0);

			public UnityObject GetPooledObject()
			{
				if (emptyIds.Count > 0)
				{
					int emptyId = emptyIds.Pop();
					objectRetreveOrder.Enqueue(emptyId);
					return objects[emptyId];
				}

				int oldestId = objectRetreveOrder.Dequeue();
				objectRetreveOrder.Enqueue(oldestId);
				//invoke something on object
				return objects[oldestId];
			}
		}
	}
}