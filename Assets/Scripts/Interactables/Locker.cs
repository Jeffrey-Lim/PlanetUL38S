using UnityEngine;
using System.Collections;

public class Locker : MonoBehaviour {
	public int item; 
	public bool isOpened = false;

	void Start () {
		if (PlayerPrefs.GetInt ("Item" + item.ToString (), 0) == 1) {
			isOpened = true;
			GetComponent<Animator> ().SetTrigger ("open");
		}
	}
}
