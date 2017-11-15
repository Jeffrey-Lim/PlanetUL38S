using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {
	private Transform playerCam;
	private GameObject pausePanel;

	void Start() {
		playerCam = GameObject.Find ("Player Camera").transform;
		pausePanel = GameObject.Find ("PausePanel");
		pausePanel.SetActive(false);

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Pause ();
		}
	}

	public void Pause () {

		if (pausePanel.activeInHierarchy == false) 
		{
			pausePanel.SetActive(true);
			Time.timeScale = 0;
			playerCam.GetComponent<CameraController> ().enabled = false;

		} else 
		{
			pausePanel.SetActive(false);
			Time.timeScale = 1;
			playerCam.GetComponent<CameraController> ().enabled = true;
		}
}
}