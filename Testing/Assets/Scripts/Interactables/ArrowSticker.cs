using UnityEngine;
using System.Collections;

//Script dat ervoor zorgt dat pijlen in dingen kunnen steken
public class ArrowSticker : MonoBehaviour {
	private float damage = 10f;
	public bool isSticking = false;
	public float minVelocity;
	Rigidbody rb;
	CapsuleCollider collider;
	TrailRenderer tr;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		collider = GetComponent<CapsuleCollider> ();
		tr = GetComponent<TrailRenderer> ();
	}

	void Update () {
		if (isSticking == true) {
			//Pijl moet stoppen met bewegen
			rb.isKinematic = true;
			//Er moet geen collision meer zijn als de pijl in iets steekt, maar de pijl moet nog wel detecteerbaar zijn om op te pakken
			collider.isTrigger = true;
		} else {
			rb.isKinematic = false;
			collider.isTrigger = false;
		}

		//Je ziet alleen een trail wanneer de pijl een bepaalde snelheid heeft
		if (rb.velocity.sqrMagnitude >= minVelocity) {
			tr.enabled = true;
			//De pijl kijkt naar de beweginsricjhting zolang die vliegt en zolang die geen parent heeft
			if (transform.parent == null) {
				transform.rotation = Quaternion.LookRotation (rb.velocity);
			}
		} else {
			tr.enabled = false;
		}
	}

	//Zorgt ervoor dat pijlen in objecten blijven steken
	void OnCollisionEnter (Collision col) {
		if (col.gameObject.name != "Arrow(Clone)" && col.gameObject.name != "Axe(Clone)" /*&& col.gameObject.name != "Player"*/ && transform.parent == null && col.relativeVelocity.sqrMagnitude >= minVelocity) {
			GetComponent<AudioSource> ().Play ();
			isSticking = true;
			//Zet het object waar de pijl in steekt als parent
			transform.parent = col.transform;
			//Dien schade toe
			if (col.transform.GetComponent<Breakable> () != null) {
				Breakable target = col.transform.GetComponent<Breakable> ();
				target.TakeDamage (damage);
			}
		}
	}
}
