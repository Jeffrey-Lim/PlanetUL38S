using UnityEngine;
using System.Collections;

public class ArrowSticker : MonoBehaviour {
	private bool hasParent = false;
	public float damage = 10f, force = 100f;
	Rigidbody rb;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
	}

	void Update () {
		if (rb.velocity != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation (rb.velocity);
		}
	}

	void OnCollisionEnter (Collision col) {
		if (col.gameObject.name != "Arrow(Clone)" && col.gameObject.name != "Player" && hasParent == false) {
			hasParent = true;
			rb.isKinematic = true;
			Physics.IgnoreCollision (this.GetComponent<Collider>(), col.collider, true);
			//transform.parent.position = col.transform.position;
			//transform.parent.rotation = col.transform.rotation;
			transform.parent = col.transform;

			if (col.transform.GetComponent<Breakable> () != null) {
				Breakable target = col.transform.GetComponent<Breakable> ();
				target.TakeDamage (damage);
			}
		}
	}
}
