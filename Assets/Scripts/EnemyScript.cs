using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour {
	[Tooltip("Speed of the enemy")]
	public float MovementSpeed;

	[Tooltip("Damage on player on touch")]
	public int ContactDamage;

	[Tooltip("Health of the enemy")]
	public int HealthPoint;

	[Tooltip("Score reward for destorying enemy")]
	public int ScoreReward;

	[Tooltip("Sound upon death")]
	public AudioClip[] DamageClips;

	private Transform playerTransform;
	private NavMeshAgent navMeshAgent;
	public GameObject GruntObject;

	// Use this for initialization
	void Start() {
		playerTransform = GameObject.Find("Player").transform;
		navMeshAgent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (playerTransform != null && !GameManager.Instance.isGameOver) {
			navMeshAgent.SetDestination(playerTransform.position);
		}
	}

	private void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.tag.Equals("Player")) {
			print(true);
		}
	}

	private void Dead() {
		GameManager.Instance.UpdateScore(ScoreReward);
		GameObject g = Instantiate(GruntObject, transform.position, Quaternion.identity);
		g.GetComponent<AudioSource>().PlayOneShot(DamageClips[Random.Range(0, DamageClips.Length)]);
		Destroy(g, 1.5f);
		Destroy(gameObject);
	}

	public void OnHit(int damage) {
		HealthPoint -= damage;
		GetComponent<AudioSource>().PlayOneShot(DamageClips[Random.Range(0, DamageClips.Length)]);
		if (HealthPoint <= 0) {
			Dead();
		}
	}
}