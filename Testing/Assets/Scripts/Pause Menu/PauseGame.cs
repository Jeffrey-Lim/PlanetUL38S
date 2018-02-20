using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {
	private Transform character;
	private GameObject pausePanel;

	void Start() {
		character = GameObject.Find ("Character").transform;
		pausePanel = GameObject.Find ("Pause Panel");
		pausePanel.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)){
			Pause ();
		}
	}

	public void Pause () {

		if (pausePanel.activeInHierarchy == false) {
			pausePanel.SetActive(true);
			Time.timeScale = 0;
			character.GetComponent<CameraController> ().enabled = false;

		} else {
			pausePanel.SetActive(false);
			Time.timeScale = 1;
			character.GetComponent<CameraController> ().enabled = true;
		}
	}
}