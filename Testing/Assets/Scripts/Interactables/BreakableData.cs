using UnityEngine;
using System.Collections;

public class BreakableData : ScriptableObject {
	public float health;
	public bool isLiving;
	public float targetDistance;
	public GameObject brokenPrefab;
	public int lootItems;
	public GameObject[] possibleLoot;
}
