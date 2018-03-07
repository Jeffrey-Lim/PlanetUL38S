using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalScript : MonoBehaviour {
	public string sceneName;

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Player") {
			SceneManager.LoadScene(sceneName);
		}
	}
}
