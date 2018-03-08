using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour {
	private Transform player;

	void Start () {
		if (GameObject.Find ("Player") != null && PlayerPrefs.GetFloat("Health") > 0) {
			LoadGame ();
		}
	}

	public void SaveGame () {
		//Save de positie
		player = GameObject.Find ("Player").transform;
		PlayerPrefs.SetFloat ("PlayerPosX", player.position.x);
		PlayerPrefs.SetFloat ("PlayerPosY", player.position.y);
		PlayerPrefs.SetFloat ("PlayerPosZ", player.position.z);

		//Save Scene
		//PlayerPrefs.SetInt ("SceneIndex", SceneManager.GetActiveScene ().buildIndex);
		PlayerPrefs.SetString ("Scene", SceneManager.GetActiveScene ().name);

		//Save Health
		PlayerPrefs.SetFloat ("Health", player.GetComponent<Breakable>().health);

		//Save Ammo
		for (int i = 0; i < 4; i++) {
			PlayerPrefs.SetInt ("Ammo" + i.ToString (), Weapon.currentAmmo [i]);
		}

		PlayerPrefs.Save ();
	}

	public void LoadScene () {
		//SceneManager.LoadScene(PlayerPrefs.GetInt("SceneIndex"));
		if (PlayerPrefs.GetString ("Scene") != "") {
			SceneManager.LoadScene (PlayerPrefs.GetString ("Scene"));
		} else {
			SceneManager.LoadScene ("Main Menu");
		}
	}

	public void LoadGame () {
		player = GameObject.Find ("Player").transform;
		player.GetComponent<Breakable> ().health = PlayerPrefs.GetFloat ("Health");
		Debug.Log (player.GetComponent<Breakable> ().health);
		for (int i = 0; i < 4; i++) { 
			Weapon.currentAmmo [i] = PlayerPrefs.GetInt ("Ammo" + i.ToString ());
		}

		if (PlayerPrefs.GetString ("Scene") == SceneManager.GetActiveScene ().name) {
			player.position = new Vector3 (PlayerPrefs.GetFloat ("PlayerPosX"), PlayerPrefs.GetFloat ("PlayerPosY"), PlayerPrefs.GetFloat ("PlayerPosZ"));
		}
	}

	public void NewGame () {
		PlayerPrefs.DeleteAll ();
		SceneManager.LoadScene ("Debug Room");
	}
}
