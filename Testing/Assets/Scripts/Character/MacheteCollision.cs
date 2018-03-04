using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MacheteCollision : MonoBehaviour {
	public List<Collider> colliders;

	void Start () {
		colliders = new List<Collider> ();
	}

	void FixedUpdate () {
		for (int i = colliders.Count - 1; i >= 0; i--) {
			//if (colliders[i] == null) {
				colliders.Remove (colliders[i]);
			//}
		}
	}

	/*void OnTriggerEnter (Collider col) {
		if (!colliders.Contains (col) && col.gameObject.name != "Player") {
			Debug.Log ("Enter " + col);
			colliders.Add (col);
		}
	}*/

	void OnTriggerStay (Collider col) {
		if (col.name != "Player") {
			colliders.Add (col);
		}
	}

	/* void OnTriggerExit (Collider col) {
		Debug.Log ("Exit " + col);
		if (colliders.Contains(col)) {
			colliders.Remove (col);
		}
	} */
}
