using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Eigenschappen voor objecten die beschadigd kunnen worden
public class Breakable : MonoBehaviour {
	public BreakableData data;
	public float health;
	private bool invincible;
	private Rigidbody rb;
	private Collider col;
	private List<Rigidbody> ragdollRb;
	private List<Collider> ragdollCol;
	private Animator anim;

	void Start () { 
		anim = this.GetComponent<Animator> ();
		col = this.GetComponent<Collider> ();
		rb = this.GetComponent<Rigidbody> ();

		ragdollRb = new List<Rigidbody> ();
		ragdollCol = new List<Collider> ();

		if (GetComponentsInChildren<Rigidbody> ().Length > 1 && GetComponentsInChildren<Collider> ().Length > 1) {
			foreach (Rigidbody x in GetComponentsInChildren<Rigidbody> ()) {
				if (!x.Equals (rb) && x.gameObject.name != "Machete Weapon") {
					ragdollRb.Add (x);
					x.isKinematic = true;
					x.detectCollisions = false;
				}
			}

			foreach (Collider x in GetComponentsInChildren<Collider> ()) {
				if (!x.Equals (col) && x.gameObject.name != "Machete Weapon") {
					ragdollCol.Add (x);
					x.enabled = false;
				}
			}
		}

		//Zet health op -1 als het wel een Breakable component nodig heeft, maar niet moet kunnen breken
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
		if (data.lootItems > 0 && data.possibleLoot.Length > 0) {
			for (int i = 1; i <= data.lootItems || i > 50; i ++) {
				Instantiate (data.possibleLoot [Random.Range (0, data.possibleLoot.Length)], this.transform.position, this.transform.rotation);
			}
		}

		if (GetComponent<GetrudeIsABitch> () != null) {
			GetComponent<GetrudeIsABitch> ().enabled = false;
		}
		if (GetComponent<EnemyScript> () != null) {
			GetComponent<EnemyScript> ().enabled = false;
		}
		if (GetComponent<Dialogue> () != null) {
			GetComponent<Dialogue> ().enabled = false;
		}

		if (data.isLiving == true) {
			rb.isKinematic = false;
			rb.constraints = RigidbodyConstraints.None;
			anim.enabled = false;

			if (ragdollRb.Count > 1) {
				foreach (Rigidbody x in ragdollRb) {
					x.isKinematic = false;
					x.detectCollisions = true;
				}
				rb.isKinematic = true;
				rb.detectCollisions = false;
			}

			if (ragdollCol.Count > 1) {
				foreach (Collider x in ragdollCol) {
					x.enabled = true;
				}

				//Zorgt ervoor dat pijlen die in het voorwerp zijn geschoten niet verdwijnen;
				GameObject[] Arrows = GameObject.FindGameObjectsWithTag ("Arrow");
				foreach (GameObject Arrow in Arrows) {
					if (Arrow.transform.parent != null) {
						if (Arrow.transform.parent.gameObject == col.gameObject) {
							Arrow.transform.parent = null;
							Arrow.GetComponent<ArrowSticker> ().isSticking = false;
						}
					}
				}
				col.enabled = false;
			}
		} else {
			if (this.GetComponent<Explosive> () != null) {
				this.GetComponent<Explosive> ().Explode ();
			}

			if (this.GetComponent<Target> () != null) {
				this.GetComponent<Target> ().MoveObject ();
				return;
			}

			//Zorgt ervoor dat pijlen die in het voorwerp zijn geschoten niet verdwijnen;
			GameObject[] Arrows = GameObject.FindGameObjectsWithTag("Arrow");
			foreach (GameObject Arrow in Arrows) {
				if (Arrow.transform.parent != null) {
					if (Arrow.transform.parent.gameObject == gameObject) {
						Arrow.transform.parent = null;
						Arrow.GetComponent<ArrowSticker> ().isSticking = false;
					}
				}
			}

			if (data.brokenPrefab != null) {
				//Spawn de gebroken versie van het voorwerp
				GameObject broken = Instantiate (data.brokenPrefab, transform.position, transform.rotation) as GameObject;
				//Geeft een unieke naam aan het net gevormde object
				string baseName = broken.name;
				//Pauper script, maar het lukt me anders niet... Vergeef me :(
				for (int i = 100; GameObject.Find (baseName + "(" + i + ")") == null && i >= 0; i--) {
					broken.name = baseName + "(" + i + ")";
				}

				//Zorgt ervoor dat de gebroken prefab dezelfde snelheid heeft als de gewone prefab
				GameObject[] brokenParts = GameObject.FindGameObjectsWithTag ("Broken Part");
				foreach (GameObject part in brokenParts) {
					if (part.transform.parent.name == broken.name) {
						if (part.GetComponent<Rigidbody> () != null) {
							part.GetComponent<Rigidbody> ().velocity = this.gameObject.GetComponent<Rigidbody> ().velocity;
						}
					}
				}
			}
			Destroy (gameObject);
		}
	}
}
