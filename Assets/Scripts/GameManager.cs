using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;

	[Header("Game variables")]
	[Tooltip("Time in seconds")]
	public float RoundTime;

	[Header("Game audioClips")]
	public AudioClip BackgroundMusic;
	//public AudioClip GameWinSound;
	public AudioClip[] GameLoseSound;

	[Header("Text boxes references")]
	public TextMeshProUGUI ScoreTextbox;
	public string ScoreTextPrefix;

	public TextMeshProUGUI AmmoTextbox;
	public string AmmoTextPrefix;

	public TextMeshProUGUI HealthTextbox;
	public string HealthTextPrefix;

	public TextMeshProUGUI TimeLeftTextbox;
	public string TimeLeftTextPrefix;

	public GameObject GameOverUI;

	[HideInInspector]
	public bool isGameOver;

	private int score;

	public TextMeshProUGUI GeneratorTextbox;
	public int GeneratorLeft;
	private AudioSource audioSource;

	// Use this for initialization
	void Awake() {
		if (Instance == null) {
			Instance = this;
		}

		audioSource = GetComponent<AudioSource>();
		audioSource.clip = BackgroundMusic;
		audioSource.Play();

		GameOverUI.SetActive(false);
	}

	// Update is called once per frame
	void Update() {
		GeneratorTextbox.text = GeneratorLeft.ToString();
		if (GeneratorLeft <= 0) {
			GameObject.FindGameObjectWithTag("Door").GetComponent<Animator>().SetBool("Open", true);
		}
		if (isGameOver)
			return;

		UpdateTimeLeft();

		if (Input.GetKeyDown(KeyCode.F1)) {
			SetGameOver(true);
		}
	}

	public void UpdateScore(int _score) {
		score += _score;
		ScoreTextbox.text = ScoreTextPrefix + score;
	}

	public void UpdateAmmo(int ammo) {
		AmmoTextbox.text = AmmoTextPrefix + ammo;
	}

	public void UpdateHealth(int health) {
		if (health <= 0 && !isGameOver) {
			health = 0;
			SetGameOver(false);
		}

		HealthTextbox.text = HealthTextPrefix + health;
	}

	public void UpdateTimeLeft() {
		if (RoundTime <= 0) {
			RoundTime = 0;
			TimeLeftTextbox.text = TimeLeftTextPrefix + "\n00:00:00";

			SetGameOver(false);

			return;
		}

		RoundTime -= Time.deltaTime;

		int minutes = (int) RoundTime / 60;
		int seconds = (int) RoundTime - 60 * minutes;
		int milliseconds = (int) (100 * (RoundTime - minutes * 60 - seconds));

		TimeLeftTextbox.text = TimeLeftTextPrefix + '\n' + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
	}

	public void SetGameOver(bool isWin) {
		isGameOver = true;
		GameOverUI.SetActive(true);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		GameObject.Find("FPSPlayer").GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
		if (isWin) {
			StartCoroutine(WinRGB());
			int minutes = (int) RoundTime / 60;
			int seconds = (int) RoundTime - 60 * minutes;
			int milliseconds = (int) (100 * (RoundTime - minutes * 60 - seconds));
			GameOverUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "You Won In\n" + string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
		} else {
			audioSource.PlayOneShot(GameLoseSound[Random.Range(0, GameLoseSound.Length)]);
		}
	}

	public void ResetGame() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
	public void ReturnToMenu() {
		SceneManager.LoadScene(0);
	}

	public int GetScore() {
		return score;
	}
	private IEnumerator WinRGB() {
		Color RGB;
		int mode = 1;
		RGB = new Color(1, 0, 1, 0.686f);
		while (true) {
			if (mode == 1) {
				RGB = RGB -= new Color(0.05f, 0, 0, 0);
				if (RGB.r <= 0) {
					mode = 2;
				}
			} else if (mode == 2) {
				RGB = RGB += new Color(0, 0.05f, 0, 0);
				if (RGB.g >= 1) {
					mode = 3;
				}
			} else if (mode == 3) {
				RGB = RGB -= new Color(0, 0, 0.05f, 0);
				if (RGB.b <= 0) {
					mode = -1;
				}
			} else if (mode == -1) {
				RGB = RGB += new Color(0.05f, 0, 0, 0);
				if (RGB.r >= 1) {
					mode = -2;
				}
			} else if (mode == -2) {
				RGB = RGB -= new Color(0, 0.05f, 0, 0);
				if (RGB.g <= 0) {
					mode = -3;
				}
			} else if (mode == -3) {
				RGB = RGB += new Color(0, 0, 0.05f, 0);
				if (RGB.b >= 1) {
					mode = 1;
				}
			}
			GameOverUI.transform.GetChild(0).GetComponent<Image>().color = RGB;
			yield return new WaitForSeconds(0.01f);
		}
	}
}