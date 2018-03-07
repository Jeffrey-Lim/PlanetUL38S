using UnityEngine;
using System.Collections;

public class lava : MonoBehaviour {
	public float movespeed;

	// Use this for initialization
	void Start () {
		movespeed = 0.02f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate (0f, movespeed * Time.deltaTime, 0f); 
	}
}
