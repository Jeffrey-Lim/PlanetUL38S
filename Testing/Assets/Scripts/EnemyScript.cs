using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {
	public GameObject bullet;
	private GameObject bulletEmittor;
	private Transform player;
	private NavMeshAgent agent;
	private Animator anim;

	private float minwalkdistance = 7f;
	private float maxwalkdistance = 30f;
	private float shootdistance = 13f;
	private float shootdelay = 0.5f;

	private float lastfiretime;
	private bool shoot;
	Rigidbody tempRigidbody;
	GameObject tempBullet;

	void Awake () {
		player = GameObject.Find ("Player").transform;
		bulletEmittor = transform.GetChild (2).gameObject;
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
	}

	void Update () {
		if (Vector3.Distance (transform.position, player.position) < maxwalkdistance) {
			if (Vector3.Distance (transform.position, player.position) > minwalkdistance) {
				agent.SetDestination (player.position);
				agent.Resume ();
				anim.SetBool ("walking", true);
				anim.SetBool ("shoot", false);
			} else {
				Vector3 targetPostition = new Vector3( player.position.x, transform.position.y, player.position.z ) ;
				transform.LookAt( targetPostition ) ;

				bulletEmittor.transform.LookAt (player.position);
				agent.Stop ();
				//anim.SetBool ("walking", false);
				anim.SetBool ("shoot", true);
			}
		} else if(Vector3.Distance (transform.position, player.position) > maxwalkdistance) {
			agent.Stop();
			anim.SetBool ("walking", false);
			anim.SetBool ("shoot", false);
		}
		if (Vector3.Distance (transform.position, player.position) < shootdistance && (Time.time > (lastfiretime + shootdelay))) {
			tempBullet = Instantiate(bullet,bulletEmittor.transform.position, bulletEmittor.transform.rotation) as GameObject;
			tempRigidbody = tempBullet.GetComponent<Rigidbody> ();
			tempRigidbody.AddRelativeForce (Vector3.forward * 3000);
			tempRigidbody.AddTorque(transform.right * 100);
			//tempbullet.transform.rotation = enemybulletemittor.transform.rotation;
			lastfiretime = Time.time;
			Destroy (tempBullet, 3f);
		}
	}
}
