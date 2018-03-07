using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DamgeOnTouch : MonoBehaviour {
	public float damage;
	private List<Collider> beenHit;

	void Start () {
		beenHit = new List<Collider> ();
	}

	void OnTriggerStay (Collider col) {
		if (col.GetComponent<Breakable> () != null && !beenHit.Contains(col)) {
			col.GetComponent<Breakable> ().TakeDamage (damage);
			beenHit.Add (col);
			StartCoroutine(canHit(col));
		}
	}

	IEnumerator canHit (Collider x) {
		yield return new WaitForSeconds (0.5f);
		beenHit.Remove (x);
	}
}
