using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public float startSpawnRadius = 10f;
	private float spawnRadius;

	[HideInInspector]
	public Wave currentWave;

	private float nextSpawnTime = 1f;

	// Update is called once per frame
	void Update () {

		if (Progression.IsGrowing)
			return;

		spawnRadius = startSpawnRadius * Progression.Growth;

		if (Time.time >= nextSpawnTime)
		{
			SpawnWave();
			nextSpawnTime = Time.time + 1f / currentWave.spawnRate;
		}
	}

	void SpawnWave ()
	{
		foreach(EnemyType eType in currentWave.enemies)
		{
			if (Random.value <= eType.spawnChance)
			{
				SpawnEnemy(eType.enemyPrefab);
			}
		}
	}

	void SpawnEnemy(GameObject enemyPrefab)
	{
		Vector2 spawnPos = PlayerController.Position;
		spawnPos += Random.insideUnitCircle.normalized * spawnRadius;

		Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
	}

}
