using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {
	private Transform centerPoint;
	private GameObject pausePanel;

	void Start() {
		centerPoint = GameObject.Find ("Center Point").transform;
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
			centerPoint.GetComponent<CameraController> ().enabled = false;

		} else {
			pausePanel.SetActive(false);
			Time.timeScale = 1;
			centerPoint.GetComponent<CameraController> ().enabled = true;
		}
	}
}