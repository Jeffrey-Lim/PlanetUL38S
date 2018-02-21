using UnityEngine;
using System.Collections;

//Eigenschappen voor objecten die beschadigd kunnen worden
public class Breakable : MonoBehaviour {
	public BreakableData data;
	public float health;
	private bool invincible;
	private Rigidbody rb;
	private Animator anim;

	void Start () { 
		anim = this.GetComponent<Animator> ();

		if (GetComponent<Rigidbody> () != null) {
			rb = GetComponent<Rigidbody> ();
			//rb.isKinematic = data.isLiving;
		} else {
			rb = null;
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

		if (data.isLiving == true) {
			//Het is de bedoeling dat vijanden een ragdoll worden wanneer ze dood zijn, maar ik heb nog geen vijanden om het mee te testen
			rb.isKinematic = false;
			rb.constraints = RigidbodyConstraints.None;
			if (anim != null) {
				anim.SetTrigger ("dead");
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
