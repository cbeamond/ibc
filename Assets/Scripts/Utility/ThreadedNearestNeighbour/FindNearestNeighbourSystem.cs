using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Ironbelly.Systems
{
	public class ThreadedFindNearestNeighbourSystem : MonoBehaviour
	{
		private enum ThreadStatus
		{
			Waiting,
			WaitingForProcessing,
			Processing,
			ProcessingComplete
		}

		private Dictionary<int, List<ObjectTransforms>> objectsToProcess = new Dictionary<int, List<ObjectTransforms>>();
		private Dictionary<int, Dictionary<int, int>> objectsProcessed = new Dictionary<int, Dictionary<int, int>>();
		private Dictionary<int, ThreadStatus> threadStatus = new Dictionary<int, ThreadStatus>();
		private Dictionary<int, Task> tasks = new Dictionary<int, Task>();
		private ConcurrentStack<int> spareThreadIds = new ConcurrentStack<int>();
		private Dictionary<int, ThreadedFindNearestNeighbour> nearestNeighbours;

		//Todo: this could be dynamic?
		private const int maxThreads = 1;

		private Task processingThread;
		private bool performThreadProcessing;
		public int FramesPerRefresh = 3;
		private int FramesUntilRefresh;

		[HideInInspector]
		public static ThreadedFindNearestNeighbourSystem Instance;

		private void Awake()
		{
			// Normally we would not use a singleton pattern, but rather something such as depenency injection or
			// perhaps service location pattern. That setup pretty out of scope for this challenge, so for this case
			// the singleton pattern is the correct tool for this job.
			if (Instance)
				throw new InvalidOperationException($"A singleton already exists for {typeof(ThreadedFindNearestNeighbourSystem).Name}");

			Instance = this;
		}

		public void AddNeighbour(ThreadedFindNearestNeighbour neighbour)
		{
			nearestNeighbours[neighbour.GetInstanceID()] = neighbour;
		}

		public void RemoveNeighbour(ThreadedFindNearestNeighbour neighbour)
		{
			nearestNeighbours.Remove(neighbour.GetInstanceID());
		}
		public ThreadedFindNearestNeighbour GetNearestNeighbour(ThreadedFindNearestNeighbour neighbour)
		{
			if (nearestNeighbours.TryGetValue(neighbour.GetInstanceID(), out ThreadedFindNearestNeighbour closestNeighbour))
				return closestNeighbour;
			return null;
		}

		private void Start()
		{
			FramesUntilRefresh = FramesPerRefresh;
		}

		public void OnEnable()
		{
			performThreadProcessing = true;
			for (int i = 1; i <= maxThreads; i++)
			{
				spareThreadIds.Push(i);
				objectsToProcess[i] = new List<ObjectTransforms>();
				threadStatus[i] = ThreadStatus.Waiting;
			}
			processingThread = Task.Run(ProcessObjectsThreadAsync);
		}

		public void Update()
		{
			bool processedThread = false;
			foreach (var status in threadStatus)
			{
				if (!processedThread && status.Value == ThreadStatus.ProcessingComplete)
				{
					processedThread = true;
					ProcessCompletedObjects(status.Key);
					continue;
				}

				if (status.Value == ThreadStatus.ProcessingComplete)
					threadStatus[status.Key] = ThreadStatus.Waiting;
			}

			FramesUntilRefresh--;
			if (FramesUntilRefresh > 0)
				return;

			FramesUntilRefresh = Mathf.Min(1, FramesPerRefresh);

			void ProcessCompletedObjects(int threadId)
			{

			}
		}

		public void OnDisable()
		{
			performThreadProcessing = false;

			foreach (Task task in tasks.Values)
				task?.Dispose();

			tasks.Clear();
		}

		private async Task ProcessObjectsThreadAsync()
		{
			spareThreadIds.TryPop(out int threadId);
			while (true)
			{
				if (!performThreadProcessing)
					return;

				// This should be measured better with stopwatch to ensure as best
				// as possible to keep timing consistent, but doing a crude 1/30th
				// a frame will be good enough for the task
				await Task.Delay(33);

				if (threadStatus[threadId] != ThreadStatus.WaitingForProcessing)
					continue;

				threadStatus[threadId] = ThreadStatus.Processing;

				List<ObjectTransforms> objects = objectsToProcess[threadId];
				Dictionary<int, int> processedObjects = objectsProcessed[threadId];

				int objectCount = objects.Count;
				for (int currentObjectIndex = 0; currentObjectIndex < objectCount; currentObjectIndex++)
				{
					float closestDistance = float.MaxValue;
					int closestObjectId = 0;
					Vector3 currentPosition = objects[currentObjectIndex].Position;

					for (int targetObjectIndex = 0; targetObjectIndex < objectCount; targetObjectIndex++)
					{
						if (currentObjectIndex == targetObjectIndex)
							continue;

						float squareMagnitude = (currentPosition - objects[targetObjectIndex].Position).sqrMagnitude;
						if (squareMagnitude < closestDistance)
							closestObjectId = objects[targetObjectIndex].InstanceId;
					}

					if (closestDistance < float.MaxValue)
						processedObjects[currentObjectIndex] = closestObjectId;
				}

				threadStatus[threadId] = ThreadStatus.ProcessingComplete;
			}
		}

		private struct ObjectTransforms
		{
			public int InstanceId;
			public Vector3 Position;
		}
	}
}