using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ContinueButton : MonoBehaviour {
	private Button button;
	private Text text;

	void Awake () {
		button = GetComponent<Button> ();
		text = GetComponentInChildren<Text> ();
	}

	void Start () {
		if (PlayerPrefs.GetFloat ("Health") <= 0) {
			button.interactable = false;
		} else {
			Debug.Log (PlayerPrefs.GetFloat ("Health"));
			text.text = "Continue: \n" + PlayerPrefs.GetString ("Scene");
		}
	}
}
