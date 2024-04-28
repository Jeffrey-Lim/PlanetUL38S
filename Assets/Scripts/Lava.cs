using UnityEngine;
using System.Collections;

public class Lava : MonoBehaviour {
	private float movespeed;
	private Rigidbody rb;
	void Start () {
		movespeed = 1f;
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {
		rb.velocity = new Vector3 (0f, movespeed, 0f); 
	}
}
