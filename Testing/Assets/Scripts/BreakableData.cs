using UnityEngine;
using System.Collections;

public class BreakableData : ScriptableObject {
	public float health;
	public float mass;
	public bool isLiving;
	public float targetDistance;
	public GameObject brokenPrefab;
	public GameObject brokenParticles;
}
