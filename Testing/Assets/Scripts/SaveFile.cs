using UnityEngine;
using System.Collections;

public class SaveFile : MonoBehaviour {
	//Volgorde: Pijl & Boog, Pistool, Revolver, Shotgun
	public static int[] maxAmmo = new int[] {30, 100, 60, 10}, currentAmmo = new int[] {30, 20, 10, 6};

	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}
}
