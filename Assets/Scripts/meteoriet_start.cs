using UnityEngine;
using System.Collections;

public class meteoriet_start : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Rigidbody rg = this.gameObject.GetComponent<Rigidbody> ();
		rg.velocity = transform.right * -1000;
		transform.Rotate (0,90,0);
		StartCoroutine (WaitDestroy ());
	
	}


	
	// Update is called once per frame
	void Update () {

	
	}
		
	IEnumerator WaitDestroy(){
	
		yield return new WaitForSeconds (20);
		Destroy (this.gameObject);
	
	}

}
