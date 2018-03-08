using UnityEngine;
using System.Collections;

public class SaveTrigger : MonoBehaviour {
	private bool isTriggered = false;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Player" && isTriggered == false) {
			isTriggered = true;
			FindObjectOfType<SaveManager> ().SaveGame ();
			Debug.Log ("nfg");
		}
	}
}
