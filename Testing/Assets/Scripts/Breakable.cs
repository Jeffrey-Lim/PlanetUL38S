using UnityEngine;
using System.Collections;

//Eigenschappen voor objecten die beschadigd kunnen worden
public class Breakable : MonoBehaviour {
	public BreakableData data;
	public float health;
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
		if (health == -1f) {
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
			}
			if (data.brokenPrefab != null) {
				GameObject broken = Instantiate (data.brokenPrefab, transform.position, transform.rotation) as GameObject;
				string baseName = broken.name;
				//Pauper script, maar het lukt me anders niet... Vergeef me :(
				for (int i = 100; GameObject.Find (baseName + "(" + i + ")") == null && i >= 0; i--) {
					broken.name = baseName + "(" + i + ")";
				}
				GameObject[] brokenParts = GameObject.FindGameObjectsWithTag ("Broken Part");
				foreach (GameObject part in brokenParts) {
					if (part.transform.parent.name == broken.name) {
						if (part.GetComponent<Rigidbody> () != null) {
							part.GetComponent<Rigidbody> ().velocity = this.gameObject.GetComponent<Rigidbody> ().velocity;
						}
					}
				}
			}
			GameObject[] Arrows = GameObject.FindGameObjectsWithTag("Arrow");
			foreach (GameObject Arrow in Arrows) {
				if (Arrow.transform.parent.gameObject == gameObject) {
					Arrow.transform.parent = null;
					Arrow.GetComponent<Rigidbody> ().isKinematic = false;
					Arrow.GetComponent<Collider> ().isTrigger = false;
				}
			}
			Destroy (gameObject);
		}
	}
}
