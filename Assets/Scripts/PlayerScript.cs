using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerScript : MonoBehaviour {
	[Tooltip("Shooting rate of the player")]
	public float ShootingRate;

	[Tooltip("Damage on enemy on each hit")]
	public int ShootingDamage;

	[Tooltip("Damage speed with contact with enemy")]
	public int DamageRate;

	[Tooltip("Starting health of the enemy")]
	public int HealthPoint;

	[Tooltip("Starting ammo of the enemy")]
	public int AmmoCount;

	[Tooltip("Shooting sound effect")]
	public AudioClip ShootingAudioClip;

	public AudioClip[] TakingDamage;

	private Rigidbody rb = null;
	private Vector3 moveDirection = Vector3.zero;
	private bool canShoot;
	private bool canDamage;
	private AudioSource audioSource;
	private GameObject _camera;
	public Recoil RecoilObject;
	private FirstPersonController _recoil;
	public GameObject Flare;

	// Use this for initialization
	void Start() {
		_recoil = transform.GetComponentInParent<FirstPersonController>();
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();

		canShoot = true;
		canDamage = true;
		audioSource.clip = ShootingAudioClip;
		_camera = Camera.main.gameObject;

		GameManager.Instance.UpdateAmmo(AmmoCount);
		GameManager.Instance.UpdateHealth(HealthPoint);
	}

	// Update is called once per frame
	void Update() {
		if (GameManager.Instance.isGameOver)
			return;

		Shoot();
		Debug.DrawLine(_camera.transform.position + _camera.transform.forward, _camera.transform.forward * 100, Color.green);
	}

	private void Shoot() {
		if (Input.GetMouseButtonDown(0) && canShoot && AmmoCount > 0) {
			RecoilObject.recoil += 0.1f;
			StartCoroutine(SpawnBullet());
		}
	}
	private IEnumerator SpawnBullet() {
		RaycastHit hit;

		if (Physics.Raycast(_camera.transform.position + _camera.transform.forward, _camera.transform.forward, out hit, Mathf.Infinity)) {
			if (hit.collider.gameObject.tag.Equals("Enemy")) {
				hit.collider.gameObject.GetComponent<EnemyScript>().OnHit(ShootingDamage);
			}
		}
		StartCoroutine(GunFlare());
		_recoil.m_MouseLook.RecoilOn();
		audioSource.PlayOneShot(ShootingAudioClip);

		GameManager.Instance.UpdateAmmo(--AmmoCount);

		canShoot = false;
		//wait for some time
		yield return new WaitForSeconds(ShootingRate);

		canShoot = true;
	}

	private void OnTriggerStay(Collider collision) {
		if (collision.gameObject.tag.Equals("Enemy") && canDamage) {
			StartCoroutine(GetDamage(collision));
		}
	}
	private IEnumerator GunFlare() {
		Flare.GetComponent<MeshRenderer>().enabled = true;
		Flare.GetComponent<Light>().enabled = true;
		yield return new WaitForSeconds(0.05f);
		Flare.GetComponent<MeshRenderer>().enabled = false;
		Flare.GetComponent<Light>().enabled = false;
	}
	private IEnumerator GetDamage(Collider collision) {
		EnemyScript enemyScript = collision.gameObject.GetComponent<EnemyScript>();
		HealthPoint -= enemyScript.ContactDamage;
		audioSource.PlayOneShot(TakingDamage[Random.Range(0, TakingDamage.Length)]);
		GameManager.Instance.UpdateHealth(HealthPoint);

		if (HealthPoint <= 0) {
			Dead();
		}

		canDamage = false;
		//wait for some time
		yield return new WaitForSeconds(DamageRate);

		canDamage = true;
	}

	private void Dead() {
		Destroy(gameObject);
	}

	public void AddAmmo(int ammo, AudioClip audioClip) {
		AmmoCount += ammo;
		GameManager.Instance.UpdateAmmo(AmmoCount);

		audioSource.PlayOneShot(audioClip);
	}

	public void AddHealth(int health, AudioClip audioClip) {
		HealthPoint += health;
		GameManager.Instance.UpdateHealth(HealthPoint);

		audioSource.PlayOneShot(audioClip);
	}
}