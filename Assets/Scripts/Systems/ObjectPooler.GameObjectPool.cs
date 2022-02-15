using Ironbelly.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

//add logging!!!!
//add error handling!!!!

namespace Ironbelly.Systems
{
	public partial class ObjectPooler
	{
		private partial class GameObjectPool
		{
			private readonly GameObject objectToClone;

			// int for the following collections is the unique instance id of the unity object
			private Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();
			private Stack<int> inactiveIds = new Stack<int>();
			private IndexedLinkedList<int> spawnedObjectOrder = new IndexedLinkedList<int>();

			public GameObjectPool(GameObject objectToClone, int poolSize)
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
					for (int i = objects.Count; i < poolSize; i++)
					{
						GameObject gameObject = UnityObject.Instantiate(objectToClone);
						gameObject.SetActive(false);

						int instanceId = gameObject.GetInstanceID();
						objects.Add(instanceId, gameObject);
						inactiveIds.Push(instanceId);
						spawnedObjectOrder.AddLast(instanceId);
					}
				}

				void ResizeSmaller(int poolSize)
				{
					int amountOfObjectsToRemove = objects.Count - poolSize;
					List<int> objectIdsToRemove = new List<int>(amountOfObjectsToRemove);

					while (true)
					{
						if (amountOfObjectsToRemove == 0)
							break;

						if (inactiveIds.Count == 0)
							break;

						objectIdsToRemove.Add(inactiveIds.Pop());
						amountOfObjectsToRemove--;
					}

					for (int i = amountOfObjectsToRemove; i > 0; i--)
						objectIdsToRemove.Add(spawnedObjectOrder.Pop());

					foreach (int objectId in objectIdsToRemove)
					{
						GameObject gameObject = objects[objectId];

						if (gameObject.TryGetComponent(out IPooledObject pooledObject))
							pooledObject.OnObjectDespawn(); //todo check

						UnityObject.Destroy(objects[objectId]);
						objects.Remove(objectId);
						spawnedObjectOrder.Remove(objectId);
					}
				}
			}

			public void Destroy() => Resize(0);

			public GameObject SpawnObject()
			{
				int targetObjectId = inactiveIds.Count > 0 ? inactiveIds.Pop() : spawnedObjectOrder.Pop();
				spawnedObjectOrder.Remove(targetObjectId);
				spawnedObjectOrder.AddLast(targetObjectId);
				GameObject gameObject = objects[targetObjectId];
				gameObject.SetActive(true);

				if (gameObject.TryGetComponent(out IPooledObject pooledObject))
					pooledObject.OnObjectSpawned();

				return gameObject;
			}

			public void DespawnObject(GameObject gameObject)
			{
				int instanceId = gameObject.GetInstanceID();
				inactiveIds.Push(instanceId);
				gameObject.SetActive(false);

				if (gameObject.TryGetComponent(out IPooledObject pooledObject))
					pooledObject.OnObjectDespawn();

				spawnedObjectOrder.AddLast(instanceId);
			}
		}
	}
}