using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadInventory : MonoBehaviour {
	private List<GameObject> slots;

	void Awake () {
		slots = new List<GameObject> ();
		slots.Add (null);
		for (int i = 1; i < 22; i++) {
			slots.Add (GameObject.Find ("/Canvas/Inventory Panel/Background/Slot Panel/Slot" + i.ToString () + "/Item"));
		}
	}
	
	void Start () {
		Refresh ();
	}

	public void Refresh () {
		for (int i = 1; i < 22; i++) {
			if (PlayerPrefs.GetInt ("Item"+i) != 0) {
				slots[i].SetActive (true);
			} else {
				slots[i].SetActive (false);	
			}
		}
	
	}
}
