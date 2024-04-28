using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneManager : MonoBehaviour {
	private GameObject screen, pausePanel, inventoryPanel;
	public UnityEngine.Video.VideoClip cutscene;
	private RawImage image;
	private UnityEngine.Video.VideoPlayer videoPlayer;
	private AudioSource source;
	private PauseGame pause;
	private bool hasStarted = false, isPaused = false, wasPaused = false;

	void Awake () {
		screen = GameObject.Find ("Cutscene");
		pausePanel = GameObject.Find ("Pause Panel");
		inventoryPanel = GameObject.Find ("Inventory Panel");
		image = screen.GetComponent<RawImage>();
		source = screen.GetComponent<AudioSource> ();
		pause = FindObjectOfType <PauseGame> ();
		
		videoPlayer = screen.GetComponent<UnityEngine.Video.VideoPlayer>();
		videoPlayer.playOnAwake = false;
		videoPlayer.clip = cutscene;
		videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.MaterialOverride;
		videoPlayer.targetMaterialRenderer = GetComponent<Renderer>();
		videoPlayer.targetMaterialProperty = "_MainTex";
		videoPlayer.audioOutputMode = UnityEngine.Video.VideoAudioOutputMode.AudioSource;
		videoPlayer.SetTargetAudioSource(0, source);
	}

	void Start () {
		screen.SetActive (false);
	}

	public void PlayCutscene (UnityEngine.Video.VideoClip new_cutscene) {
		cutscene = new_cutscene;
		if (cutscene != null) {
			Time.timeScale = 0;
			videoPlayer.Play ();
			hasStarted = true;
		} else {
			screen.SetActive (false);
		}
	}

	void Update () {
		if (hasStarted == true && (pausePanel.activeInHierarchy == true || inventoryPanel.activeInHierarchy == true)) {
			videoPlayer.Pause ();
			isPaused = true;
		} else if (hasStarted == true && wasPaused == true) {
			videoPlayer.Play ();
			isPaused = false;
		}
	}

	void LateUpdate () {
		if (hasStarted == true && isPaused == false) {

			if (videoPlayer.isPlaying == true && InputManager.reload.Pressed == true) {
				videoPlayer.Stop ();
			}

			if (videoPlayer.isPlaying == false) {
				pause.Deactivate (screen);
				hasStarted = false;
			}

		}
		wasPaused = isPaused;
	}
}
