using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour {
	private float AnimateSpeed = 100;
	// Update is called once per frame
	void Update() {
		//transform.Rotate(Vector3.up, AnimateSpeed * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider collision) {
		if (collision.gameObject.tag.Equals("Player") && !GameManager.Instance.isGameOver) {
			GameManager.Instance.SetGameOver(true);
		}
	}
}