using UnityEngine;
using System.Collections;

public class AmmoData : ScriptableObject {
	public int weaponType; //0 = Boog, 1 = 10mm pistool, 2 = revolver, 3 = shotgun, 4 = granaat
	public int amount;
	public bool destroyOnPickUp;
}
