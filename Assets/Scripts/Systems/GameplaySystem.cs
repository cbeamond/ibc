using Ironbelly.Behaviour;
using System;
using UnityEngine;

//add logging!!!!
//add error handling!!!!

namespace Ironbelly.Systems
{
	public class GameplaySystem : MonoBehaviour
	{
		[SerializeField]
		private GameObject CubePrefab;

		private const string cubePoolTag = "Cube";
		public int CubeAmount = 10;
		private int previousCubeAmount;

		[HideInInspector]
		public static GameplaySystem Instance;
		public ContainingZone ContainingZone;
		public readonly ObjectPooler ObjectPooler = new ObjectPooler();

		private void Awake()
		{
			// Normally we would not use a singleton pattern, but rather something such as depenency injection or
			// perhaps service location pattern. That setup pretty out of scope for this challenge, so for this case
			// the singleton pattern is the correct tool for this job.
			if (Instance)
				throw new InvalidOperationException($"A singleton already exists for {typeof(GameplaySystem).Name}");

			Instance = this;
		}

		private void Start()
		{
			if (!ContainingZone)
				Debug.LogError($"No {typeof(ContainingZone)} has been assigned to {nameof(ContainingZone)}");

			previousCubeAmount = CubeAmount;
			ObjectPooler.CreatePool(cubePoolTag, CubeAmount, CubePrefab);
			SpawnCubes(CubeAmount);

		}

		private void Update()
		{
			if (previousCubeAmount != CubeAmount)
			{
				CubeAmount = Mathf.Max(0, CubeAmount);
				ObjectPooler.ResizePool(cubePoolTag, CubeAmount);

				if (previousCubeAmount < CubeAmount)
					SpawnCubes(CubeAmount - previousCubeAmount);

				previousCubeAmount = CubeAmount;
			}
		}

		private void SpawnCubes(int amountToSpawn)
		{
			for (int i = 0; i < amountToSpawn; i++)
				ObjectPooler.SpawnObject(cubePoolTag);
		}
	}
}