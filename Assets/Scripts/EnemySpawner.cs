using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	[Tooltip("Start Spawner After this amount of gen")]
	public int GeneratorCompletedStarting;

	[Tooltip("Duration the spawner will spawn continuously")]
	public float SpawnDuration;

	[Tooltip("Time the spawner end spawning")]
	public float SpawnRate;

	[Tooltip("Enemy prefeb to be spawned")]
	public GameObject EnemyPrefeb;

	[Tooltip("Destory spawner after spawning")]
	public bool DestoryAfterSpawning;
	private bool StartedSpawning;

	public EnemySpawnerTrigger EnemySpawnerTrigger;

	// Use this for initialization
	void Start() {
		/*if (EnemySpawnerTrigger == null) {
			InitializedSpawner();
		} else {
			EnemySpawnerTrigger.GetComponent<EnemySpawnerTrigger>().SetEnemySpawner(this);
		}*/
	}
	void Update() {
		if (!StartedSpawning && (5 - GameManager.Instance.GeneratorLeft) >= GeneratorCompletedStarting) {
			StartedSpawning = true;
			InitializedSpawner();
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
		yield return new WaitForSeconds(GeneratorCompletedStarting + SpawnDuration);
		CancelInvoke("StartSpawner");

		if (DestoryAfterSpawning) {
			Destroy(gameObject);
		}
	}

	public void InitializedSpawner() {

		InvokeRepeating("StartSpawner", 0, SpawnRate);
		StartCoroutine(StopSpawning());
	}
}