using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneManager : MonoBehaviour {
	private GameObject screen, pausePanel, inventoryPanel;
	public MovieTexture movie;
	private RawImage image;
	private AudioSource source;
	private PauseGame pause;
	private bool hasStarted = false, isPaused = false, wasPaused = false;

	void Awake () {
		screen = GameObject.Find ("Cutscene");
		pausePanel = GameObject.Find ("Pause Panel");
		inventoryPanel = GameObject.Find ("Inventory Panel");
		image = screen.GetComponent<RawImage> ();
		source = screen.GetComponent<AudioSource> ();
		pause = FindObjectOfType <PauseGame> ();
	}

	void Start () {
		screen.SetActive (false);
	}

	public void PlayCutscene (MovieTexture cutscene) {
		movie = cutscene;
		if (movie != null) {
			Time.timeScale = 0;
			image.texture = movie;
			movie.Play ();
			source.clip = movie.audioClip;
			source.Play ();
			hasStarted = true;
		} else {
			screen.SetActive (false);
		}
	}

	void Update () {
		if (hasStarted == true && (pausePanel.activeInHierarchy == true || inventoryPanel.activeInHierarchy == true)) {
			movie.Pause ();
			isPaused = true;
		} else if (hasStarted == true && wasPaused == true) {
			movie.Play ();
			isPaused = false;
		}
	}

	void LateUpdate () {
		if (hasStarted == true && isPaused == false) {

			if (movie.isPlaying == true && InputManager.reload.Pressed == true) {
				movie.Stop ();
			}

			if (movie.isPlaying == false) {
				pause.Deactivate (screen);
				hasStarted = false;
			}

		}
		wasPaused = isPaused;
	}
}
