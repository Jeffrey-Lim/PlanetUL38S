using UnityEngine;
using System.Collections;

//Ik ben van plan dit script nog te algemeniseren en te integreren in andere scripts
public class TempFishScript : MonoBehaviour {
	Rigidbody rb;
	private float speed = 3f;
	private float direction;
	private float radius;
	Vector3 spawnPos;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
		rb.velocity = transform.forward * speed;
		spawnPos = transform.position;
		if (Random.Range (0, 2) == 0) {
			direction = -1f;
		} else {
			direction = 1f;
		}
		radius = Random.Range (0.5f, 2f);
	}

	void Update () {
		if (this.GetComponent<Breakable> ().health <= 0f) {
			rb.constraints = RigidbodyConstraints.None;
			//rb.isKinematic = true;
			/*if (transform.rotation.eulerAngles.z != 90f) {
				transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (new Vector3 (transform.rotation.x, transform.rotation.y, 90f)), 2f * Time.deltaTime);
			}
			if (transform.position.y != spawnPos.y + 1f) {
				transform.position += Vector3.up * 2f * Time.deltaTime;
			}*/
		} else {
			rb.velocity = rb.velocity.normalized * speed;
			rb.AddForce (transform.right * direction * rb.velocity.magnitude * radius);
			transform.rotation = Quaternion.LookRotation (rb.velocity);
		}
	}
}
