using UnityEngine;
using System.Collections;

public class Ammo : MonoBehaviour {
	public AmmoData data;
	public int amount;

	void Start () {
		if (data.randomAmount == true) {
			amount = Mathf.RoundToInt(Random.Range (2, 20));
		} else {
			amount = data.amount;
			if (amount == 0) {
				amount = 1000;
			}
		}
	}
}
