using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour {
	public UnityEngine.Video.VideoClip cutscene;
	private GameObject screen;
	private CutsceneManager manager;
	private bool isTriggered = false;

	void Awake () {
		screen = GameObject.Find ("Cutscene");
		manager = FindObjectOfType<CutsceneManager> ();
	}

	void OnTriggerEnter (Collider col) {
		if (col.name == "Player" && isTriggered == false) {
			isTriggered = true;
			screen.SetActive (true);
			manager.PlayCutscene (cutscene);
		}
	}
}
