using UnityEngine;
using System.Collections;

public class bulletcollision : MonoBehaviour {

	// de bullet
	public GameObject other;

	// rigidbody component vd bullet hier in slepen
	public Rigidbody rb;
	void Start(){
		rb = other.GetComponent<Rigidbody> ();
	}

	/*void OnCollisionEnter(Collision Col) {
		if(Col.gameObject.name !="Cube" && Col.gameObject.name != "skere vis(Clone)"){
		rb.isKinematic=true; // stop physics
		transform.parent = Col.transform;
		}
	}*/ 
}
