using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeAjust : MonoBehaviour {

	public AudioMixer _AudioMixer;
	public TextMeshProUGUI VolumeInt;
	private float SavedVolume;

	void Start() {
		if (PlayerPrefs.HasKey("Audio" + transform.name)) {
			SavedVolume = PlayerPrefs.GetFloat("Audio" + transform.name);
			GetComponent<Slider>().value = SavedVolume;
			_AudioMixer.SetFloat("Audio" + transform.name, Mathf.Log10(SavedVolume) * 20);
			VolumeInt.text = Mathf.FloorToInt(SavedVolume * 100f).ToString();
			return;
		}
		SavedVolume = 1;
	}

	public void SetVolume(float level) {
		_AudioMixer.SetFloat("Audio" + transform.name, Mathf.Log10(level) * 20);
		VolumeInt.text = Mathf.FloorToInt(level * 100f).ToString();
		PlayerPrefs.SetFloat("Audio" + transform.name, level);
	}
}