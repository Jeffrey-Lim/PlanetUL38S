using UnityEngine;
using System.Collections;

public class ItemData : ScriptableObject {
	public string itemName;
	public int amount;
	public bool dontDestroyOnPickUp;
	public float restoreHealth;
}
