using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CutsceneTrigger : MonoBehaviour {
	public MovieTexture cutscene;
	private GameObject screen;
	private bool isTriggered = false;

	void Awake () {
		screen = GameObject.Find ("Cutscene");
	}

	void OnTriggerEnter (Collider col) {
		if (col.name == "Player" && isTriggered == false) {
			isTriggered = true;
			screen.SetActive (true);
			screen.GetComponent<Cutscene> ().PlayCutscene (cutscene);
		}
	}
}
