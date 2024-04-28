using UnityEngine;
using System.Collections;

//Ik ben van plan dit script nog te algemeniseren en te integreren in andere scripts
public class FishScript : MonoBehaviour {
	Rigidbody rb;
	Breakable health;
	private float speed = 3f;
	private float direction;
	private float radius;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		if (rb == null) {
			rb = this.GetComponentInChildren<Rigidbody> ();
		}
		rb.velocity = transform.forward * speed;
		if (Random.Range (0, 2) == 0) {
			direction = -1f;
		} else {
			direction = 1f;
		}
		radius = Random.Range (0.5f, 2f);

		health = this.GetComponent<Breakable> ();
		if (health == null) {
			health = this.GetComponentInChildren<Breakable> ();
		}
	}

	void Update () {
		if (health.health <= 0f) {
			rb.constraints = RigidbodyConstraints.None;
		} else {
			rb.velocity = rb.velocity.normalized * speed;
			rb.AddForce (transform.right * direction * rb.velocity.magnitude * radius);
			transform.rotation = Quaternion.LookRotation (rb.velocity);
		}
	}
}
