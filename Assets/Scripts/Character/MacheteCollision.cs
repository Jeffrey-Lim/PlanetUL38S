using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MacheteCollision : MonoBehaviour {
	public List<Collider> colliders;

	void Awake () {
		colliders = new List<Collider> ();
	}

	void FixedUpdate () {
		for (int i = colliders.Count - 1; i >= 0; i--) {
			colliders.Remove (colliders[i]);
		}
	}

	void OnTriggerStay (Collider col) {
		if (col.name != "Player") {
			colliders.Add (col);
		}
	}
}
