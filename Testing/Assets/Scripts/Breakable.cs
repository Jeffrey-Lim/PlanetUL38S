using UnityEngine;
using System.Collections;

//Eigenschappen voor objecten die beschadigd kunnen worden
public class Breakable : MonoBehaviour {
	public BreakableData data;
	private float health;
	private bool invincible;
	private Rigidbody rb;

	void Start () {
		if (GetComponent<Rigidbody> () != null) {
			rb = GetComponent<Rigidbody> ();
			rb.mass = data.mass;
			rb.isKinematic = data.isLiving;
		} else {
			rb = null;
		}
		health = data.health;
		if (health == -1) {
			invincible = true;
		} else {
			invincible = false;
		}
	}

	public void TakeDamage (float amount) {
		if (invincible == false) {
			health -= amount;
			if (health <= 0f) {
				Die ();
			}
		}
	}

	void Die() {
		if (data.isLiving == true) {
			rb.isKinematic = false;
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
