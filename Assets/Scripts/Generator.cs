﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour {

	public float TimeLeft = 30;
	private bool inProgress, isDone, isSkillCheck;
	public RectTransform[] UIComponents, SkillCheck;
	public AudioClip SkillCheckSound, Explode, Ding;

	private int SkillCheckAppearChance = 90;
	void Start() {
		if (PlayerPrefs.GetInt("Chevy") == 1) {
			SkillCheckAppearChance = 0;
		}
	}

	void Update() {
		if (transform.GetChild(0).GetComponent<TextMesh>() != null) {
			if (GameObject.FindGameObjectWithTag("Player") != null) {
				transform.GetChild(0).LookAt(2 * (new Vector3(transform.position.x, 1.5f, transform.position.z)) - (new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, 0, GameObject.FindGameObjectWithTag("Player").transform.position.z)));
			}
		}
		if (TimeLeft > 0 && inProgress) {
			TimeLeft -= Time.deltaTime;
			UIComponents[1].localPosition = new Vector3((-900f + (30f - TimeLeft) * 30f), 0, 0);
			if (transform.GetChild(0).GetComponent<TextMesh>() != null) {
				transform.GetChild(0).GetComponent<TextMesh>().text = Mathf.FloorToInt(TimeLeft).ToString();
			}
			if (isSkillCheck) {
				SkillCheck[2].localPosition += new Vector3(1, 0, 0) * 400 * Time.deltaTime;
				if (Input.GetKeyDown(KeyCode.E)) {
					if (SkillCheck[2].localPosition.x < SkillCheck[1].localPosition.x - 27) {
						SkillCheckResult(false);
					} else {
						SkillCheckResult(true);
					}
				}
				if (SkillCheck[2].localPosition.x > SkillCheck[1].localPosition.x + 27) {
					SkillCheckResult(false);
				}
			}
		}
		if (TimeLeft <= 0 && !isDone) {
			isDone = !isDone;
			StartCoroutine(CompletedDing());
			GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().GeneratorLeft -= 1;
			transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().Play();
			Destroy(transform.GetChild(0).gameObject);
			SkillCheck[0].gameObject.SetActive(false);
		}
	}
	private void TriggerSkillCheck() { //+- 27 in X = success
		SkillCheck[0].gameObject.SetActive(true);
		SkillCheck[1].localPosition = new Vector3(Random.Range(-427f, 427f), 0, 0);
		SkillCheck[2].localPosition = new Vector3(-750, 0, 0);
		GetComponent<AudioSource>().PlayOneShot(SkillCheckSound);
	}
	private void SkillCheckResult(bool _success) {
		isSkillCheck = false;
		SkillCheck[0].gameObject.SetActive(false);
		if (_success) {
			//PlaySuccessAudio
		} else {
			GetComponent<AudioSource>().PlayOneShot(Explode);
			TimeLeft += 3f;
		}
	}
	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			if (transform.GetChild(0).name == "TimeLeft") {
				transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
				UIComponents[0].gameObject.SetActive(true);
				InvokeRepeating("CanTriggerSkillCheck", 0, 1);
			}
			inProgress = true;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			CancelInvoke("CanTriggerSkillCheck");
			if (isSkillCheck) {
				SkillCheckResult(false);
			}
			UIComponents[0].gameObject.SetActive(false);
			if (transform.GetChild(0).name == "TimeLeft") {
				transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
			}
			inProgress = false;
		}
	}

	IEnumerator CompletedDing() {
		while (true) {
			yield return new WaitForSeconds(0.5f);
			GetComponent<AudioSource>().PlayOneShot(Ding);
		}
	}

	private void CanTriggerSkillCheck() {
		if (Random.Range(1, 101) >= SkillCheckAppearChance && !isSkillCheck) {
			isSkillCheck = true;
			TriggerSkillCheck();
		}
	}
}

/*

if (Random.Range(1, 500) == 1 && !isSkillCheck) {
				isSkillCheck = true;
				TriggerSkillCheck();
			}

 */