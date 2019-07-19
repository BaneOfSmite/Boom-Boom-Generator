using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Buttons(int ButtonType) {
		switch (ButtonType) {
			case 0:
				Application.Quit();
				break;
			case 1:
				SceneManager.LoadScene(1);
				break;
		}
	}
}
