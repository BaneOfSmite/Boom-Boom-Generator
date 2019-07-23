using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	[Tooltip("Time the spawner start after starting the scene")]
	public float StartTime;

	[Tooltip("Duration the spawner will spawn continuously")]
	public float SpawnDuration;

	[Tooltip("Time the spawner end spawning")]
	public float SpawnRate;

	[Tooltip("Enemy prefeb to be spawned")]
	public GameObject EnemyPrefeb;

	[Tooltip("Destory spawner after spawning")]
	public bool DestoryAfterSpawning;

	public EnemySpawnerTrigger EnemySpawnerTrigger;

	// Use this for initialization
	void Start() {
		if (EnemySpawnerTrigger == null) {
			InitializedSpawner();
		} else {
			EnemySpawnerTrigger.GetComponent<EnemySpawnerTrigger>().SetEnemySpawner(this);
		}
	}

	private void StartSpawner() {
		if (GameManager.Instance.isGameOver) {
			CancelInvoke("StartSpawner");
			return;
		}

		Instantiate(EnemyPrefeb, transform.position, Quaternion.identity);
	}

	private IEnumerator StopSpawning() {
		yield return new WaitForSeconds(StartTime + SpawnDuration);
		CancelInvoke("StartSpawner");

		if (DestoryAfterSpawning) {
			Destroy(gameObject);
		}
	}

	public void InitializedSpawner() {
		InvokeRepeating("StartSpawner", StartTime, SpawnRate);
		StartCoroutine(StopSpawning());
	}
}