using UnityEngine;
using System.Collections;

public class PauseGame : MonoBehaviour {
	private GameObject pausePanel, inventoryPanel, optionsPanel, screen;
	private GameObject[] panels;
	private CameraController cam;

	void Awake () {
		cam = GameObject.Find ("Character").GetComponent<CameraController> ();
		pausePanel = GameObject.Find ("Pause Panel");
		inventoryPanel = GameObject.Find ("Inventory Panel");
		optionsPanel = GameObject.Find ("Options Panel");
		screen = GameObject.Find ("Cutscene");
		panels = new GameObject[] {pausePanel, inventoryPanel, optionsPanel};
	}

	void Start () {
		pausePanel.SetActive (false);
		inventoryPanel.SetActive (false);
		optionsPanel.SetActive (false);
		Time.timeScale = 1;
	}
		
	void Update () {
		if (screen.activeInHierarchy == true) {
			Time.timeScale = 0;
			cam.enabled = false;
		}

		if (InputManager.pause.Pressed) {
			Pause (pausePanel);
		}

		if (InputManager.inventory.Pressed) {
			Pause (inventoryPanel);
		}
	}

	public void Pause (GameObject panel) {
		foreach (GameObject x in panels) {
			if (x != panel) {
				x.SetActive (false);
			}
		}

		if (panel.activeInHierarchy == false) {
			Activate (panel);
		} else {
			Deactivate (panel);
		}
	}

	public void Activate (GameObject panel) {
		panel.SetActive(true);
		Time.timeScale = 0;
		cam.enabled = false;
	}

	public void Deactivate (GameObject panel) {
		panel.SetActive(false);
		Time.timeScale = 1;
		cam.enabled = true;
	}
}