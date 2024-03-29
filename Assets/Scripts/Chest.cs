﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

	private bool CanOpen, CanLoot;
	private float cooldown, animationCoolDown;
	private int ItemID;
	private GameObject Player;
	public GameObject E;

	public GameObject[] ItemType;
	public AudioClip[] PickupSounds;
	void Start() {
		Player = GameObject.FindGameObjectWithTag("Player");
	}

	// Update is called once per frame
	void Update() {
		if (!CanLoot && CanOpen && cooldown <= 0 && Input.GetKeyDown(KeyCode.E)) {
			CanLoot = true;
			animationCoolDown = 1;
			GetComponent<Animator>().SetBool("ChestOpen", true);
			GetComponent<AudioSource>().Play();
			ItemID = Random.Range(0, ItemType.Length);
			transform.GetChild(0).GetComponent<MeshFilter>().mesh = ItemType[ItemID].GetComponent<MeshFilter>().sharedMesh;
			transform.GetChild(0).GetComponent<MeshRenderer>().material = ItemType[ItemID].GetComponent<MeshRenderer>().sharedMaterial;
		}

		if (CanOpen && CanLoot && animationCoolDown <= 0 && Input.GetKeyDown(KeyCode.E)) {
			CanLoot = false;
			cooldown = 15;
			GetComponent<AudioSource>().Play();
			GetComponent<Animator>().SetBool("ChestOpen", false);
			transform.GetChild(0).GetComponent<MeshFilter>().mesh = null;
			switch (ItemID) {
				case 0:
					Player.GetComponent<PlayerScript>().AddHealth(Random.Range(10, 20), PickupSounds[0]);
					break;
				case 1:
					Player.GetComponent<PlayerScript>().AddAmmo(Random.Range(10, 20), PickupSounds[1]);
					break;
			}
		}
		cooldown -= Time.deltaTime;
		animationCoolDown -= Time.deltaTime;

	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			E.SetActive(true);
			CanOpen = true;
		}
	}

	private void OnTriggerExit(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			E.SetActive(false);
			CanOpen = false;
		}
	}
}