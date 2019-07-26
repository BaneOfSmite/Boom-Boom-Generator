using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public GameObject[] UIGameObjects;
	public TMP_Dropdown _qualitysetting;
	private int _quality;

	void Start() {
		if (PlayerPrefs.HasKey("Quality")) {
			_quality = PlayerPrefs.GetInt("Quality");
			_qualitysetting.value = _quality;
			return;
		}
		_quality = QualitySettings.GetQualityLevel();
		_qualitysetting.value = _quality;

	}
	public void Buttons(int ButtonType) {
		switch (ButtonType) {
			case 0: //Quit
				Application.Quit();
				break;
			case 1: //Play
				SceneManager.LoadScene(1);
				break;
			case 2: //Options
				UIGameObjects[0].SetActive(false);
				UIGameObjects[1].GetComponent<Animator>().SetBool("Options", true);
				break;
			case 3: //How To Play

				break;
			case 4: //Return To Menu From Options
				UIGameObjects[1].GetComponent<Animator>().SetBool("Options", false);
				StartCoroutine(DelayedFunction(1, 1.5f));
				break;
			case 5: //Return To Menu From How To Play

				break;

		}
	}
	public void QualityOption(int QualitySetting) {
		QualitySettings.SetQualityLevel(QualitySetting);
		PlayerPrefs.SetInt("Quality", QualitySetting);
	}

	private IEnumerator DelayedFunction(int type, float delay) {
		yield return new WaitForSeconds(delay);
		switch (type) {
			case 1:
				UIGameObjects[0].SetActive(true);
				break;
		}
	}
}