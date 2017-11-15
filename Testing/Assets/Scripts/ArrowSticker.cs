using UnityEngine;
using System.Collections;

public class ArrowSticker : MonoBehaviour {
	private float damage = 10f;
	Rigidbody rb;

	void Start () {
		rb = this.GetComponent<Rigidbody> ();
	}

	void Update () {
		//De pijl kijkt naar de beweginsricjhting zolang die vliegt
		if (rb.velocity != Vector3.zero && transform.parent == null) {
			transform.rotation = Quaternion.LookRotation (rb.velocity);
		}
	}

	//Zorgt ervoor dat pijlen in objecten blijven steken
	void OnCollisionEnter (Collision col) {
		if (col.gameObject.name != "Player" && transform.parent != null) {
			//Pijl moet stoppen met bewegen
			rb.isKinematic = true;
			//Er moet geen collision meer zijn als de pijl in iets steekt, maar de pijl moet nog wel detecteerbaar zijn om op te pakken
			this.GetComponent<Collider> ().isTrigger = true;
			//Zet het object waar de pijl in steekt als parent
			transform.parent = col.transform;

			if (col.transform.GetComponent<Breakable> () != null) {
				Breakable target = col.transform.GetComponent<Breakable> ();
				target.TakeDamage (damage);
			}
		}
	}
}
