using UnityEngine;
using System.Collections;

//Dit tijdelijk script heb ik gemaakt om variabelen bij te houden die moeten worden opgeslagen in een save file. 
public class SaveFile : MonoBehaviour {
	//Volgorde: Pijl & Boog, Pistool, Revolver, Shotgun
	public static int[] maxAmmo = new int[] {30, 100, 60, 10}, currentAmmo = new int[] {30, 100, 60, 6}, maxMagazine = new int[] {1, 10, 6, 4};

	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}
}
