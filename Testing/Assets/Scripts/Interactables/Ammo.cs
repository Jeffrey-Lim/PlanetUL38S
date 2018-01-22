using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {
	public AmmoData data;
	public int amount;

	void Start () {
		amount = data.amount;
		if (amount == 0) {
			amount = 1000;
		}
	}
}
