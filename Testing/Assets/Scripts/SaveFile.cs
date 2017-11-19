using UnityEngine;
using System.Collections;

public class SaveFile : MonoBehaviour {
	public static int[] maxAmmo = new int[] {30, 100, 60}, currentAmmo = new int[] {5, 20, 10};

	void Awake () {
		DontDestroyOnLoad (transform.gameObject);
	}
}
