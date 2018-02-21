using UnityEngine;
using System.Collections;

//Ik ben van plan dit script nog te algemeniseren en te integreren in andere scripts
public class FishScript : MonoBehaviour {
	Rigidbody rb;
	Breakable health;
	private float speed = 3f;
	private float direction;
	private float radius;
	//private bool stop = false;
	//Vector3 spawnPos;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		if (rb == null) {
			rb = this.GetComponentInChildren<Rigidbody> ();
		}
		rb.velocity = transform.forward * speed;
		//spawnPos = transform.position;
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
			//rb.isKinematic = true;
			/*if (transform.rotation.eulerAngles.z != 90f) {
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (new Vector3 (transform.rotation.x, transform.rotation.y, 90f)), 2f * Time.deltaTime);
			}
			if (stop == false) {
				transform.position += new Vector3 (0f, Mathf.Lerp(transform.position.y, 2f, 0.5f * Time.deltaTime), 0f);
			}*/
		} else {
			rb.velocity = rb.velocity.normalized * speed;
			rb.AddForce (transform.right * direction * rb.velocity.magnitude * radius);
			transform.rotation = Quaternion.LookRotation (rb.velocity);
		}
	}

	/*void OnTriggerEnter (Collider col) {
		Debug.Log (col.gameObject.name);
		if (col.transform.parent.name == "Water4Simple") {
			stop = true;
		}
	}*/
}
