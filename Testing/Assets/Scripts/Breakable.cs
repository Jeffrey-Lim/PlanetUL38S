using UnityEngine;
using System.Collections;

//Eigenschappen voor objecten die beschadigd kunnen worden
public class Breakable : MonoBehaviour {
	public BreakableData data;
	private float health;
	private Rigidbody rb;

	void Start () {
		if (GetComponent<Rigidbody> () != null) {
			rb = GetComponent<Rigidbody> ();
			rb.mass = data.mass;
		} else {
			rb = null;
		}
		health = data.health;
	}

	public void TakeDamage (float amount) {
		health -= amount;
		if (health <= 0f) {
			Die ();
		}
	}

	public void TakeForce (Vector3 force, Vector3 point) {
		if (rb != null) {
			rb.AddForceAtPosition (force, point);
		} else {

		}
	}

	void Die() {
		if (data.isLiving) {

		} else {
			if (data.brokenParticles != null) {
				GameObject particles = Instantiate (data.brokenParticles, transform, transform) as GameObject;
				Destroy (particles, 0.5f);
			}
			if (data.brokenPrefab != null) {
				GameObject broken = Instantiate (data.brokenPrefab, transform, transform) as GameObject;	
				Destroy (broken, 10f);
			}
			Destroy (gameObject);
		}
	}
}
