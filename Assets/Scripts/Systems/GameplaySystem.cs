using Ironbelly.Behaviour;
using System;
using UnityEngine;

namespace Ironbelly.Systems
{
	public class GameplaySystem : MonoBehaviour
	{
		[SerializeField]
		private GameObject CubePrefab;

		[SerializeField]
		private GameObject BulletPrefab;

		public int CubeSpawnAmount = 20;
		public int BulletSpawnAmount = 10;

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

			ObjectPooler.CreatePool(Constant.ObjectPoolTag.Cube, CubeSpawnAmount, CubePrefab);
			SpawnCubes(CubeSpawnAmount);

			ObjectPooler.CreatePool(Constant.ObjectPoolTag.Bullet, BulletSpawnAmount, BulletPrefab);
		}

		private void Update()
		{
			CubeSpawnAmount = Mathf.Max(0, CubeSpawnAmount);
			ObjectPooler.ResizePool(Constant.ObjectPoolTag.Cube, CubeSpawnAmount);

			BulletSpawnAmount = Mathf.Max(0, BulletSpawnAmount);
			ObjectPooler.ResizePool(Constant.ObjectPoolTag.Bullet, BulletSpawnAmount);
		}

		private void SpawnCubes(int amountToSpawn)
		{
			for (int i = 0; i < amountToSpawn; i++)
				ObjectPooler.SpawnObject(Constant.ObjectPoolTag.Cube);
		}
	}
}