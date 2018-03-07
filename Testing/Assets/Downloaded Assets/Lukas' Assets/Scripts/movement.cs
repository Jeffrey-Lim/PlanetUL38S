using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {

	public float movespeed = 40.0F;
	public int jumpstrength = 360;
	public Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

	}

	// Update is called once per frame


	void Update() {



		transform.Translate(movespeed*Input.GetAxis("Horizontal")*Time.deltaTime,0f,movespeed*Input.GetAxis("Vertical")*Time.deltaTime); 
		if(Input.GetKeyDown("space"))
		{
			rb.AddForce (Vector3.up * jumpstrength);

		}
	}
}
