using UnityEngine;
using System.Collections;

public class Explosive : MonoBehaviour {
	public ExplosiveData data;
	private float delay;
	private bool hasExploded = false;
	private bool setTimer;

	void Start () {
		delay = data.delay;
		if (delay == 0) {
			setTimer = false;
		} else {
			setTimer = true;
		}
	}

	void Update () {
		if (setTimer == true) {
			delay -= Time.deltaTime;
			if (delay <= 0f && hasExploded == false) {
				Explode ();
				hasExploded = true;
			}
		}
	}

	public void Explode () {
		if (hasExploded == false) {
			hasExploded = true;
			Instantiate (data.explosionEffect, transform.position, transform.rotation);

			Collider[] colToDestroy = Physics.OverlapSphere (transform.position, data.explosionRadius);
			foreach (Collider col in colToDestroy) {
				if (col.GetComponent<Breakable> () != null) {
					col.GetComponent<Breakable> ().TakeDamage (data.explosionDamage);
				}

				if (col.GetComponent<Explosive> () != null) {
					col.GetComponent<Explosive> ().Explode ();
				}
			}

			Collider[] colToMove = Physics.OverlapSphere (transform.position, data.explosionRadius);
			foreach (Collider col in colToMove) {
				if (col.GetComponent<Rigidbody> () != null) {
					col.GetComponent<Rigidbody> ().AddExplosionForce (data.explosionForce, transform.position, data.explosionRadius);
				}
			}

			Destroy (gameObject);
		}
	}
}
