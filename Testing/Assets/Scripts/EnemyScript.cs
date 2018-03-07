using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyScript : MonoBehaviour 
{
	public Transform playertransform;
	public int speed = 2;
	public float minwalkdistance = 7f;
	public float maxwalkdistance = 30f;
	public float shootdistance = 13f;
	public GameObject bullet;
	public float shootdelay = 2f;
	public float lastfiretime;
	public GameObject enemybulletemittor;
	public NavMeshAgent agent;
	public int health;
	public Animator anim;
	public bool walking;
	Rigidbody tijdelijke_rigidbody;
	public bool shoot;
	GameObject tijdelijke_bullet;
	public GameObject player;

	void start(){
		health = 3;
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
		agent.Warp (new Vector3(0,0,0));
	}

	void Update () {
		if (Vector3.Distance (transform.position, playertransform.position) < maxwalkdistance) {
			if (Vector3.Distance (transform.position, playertransform.position) > minwalkdistance) {

				agent.SetDestination (playertransform.position);
				agent.Resume ();
				walking = true;
				anim.SetBool ("walking", true);
				anim.SetBool ("shoot", false);
				//transform.Translate (new Vector3 (0, 0, speed * Time.deltaTime));
			} else {
				//enemybulletemittor.transform.LookAt (playertransform.position);
				Vector3 targetPostition = new Vector3( playertransform.position.x, transform.position.y, playertransform.position.z ) ;
				transform.LookAt( targetPostition ) ;

				enemybulletemittor.transform.LookAt (playertransform.position);
				agent.Stop ();
				walking = false;
				//anim.SetBool ("walking", false);
				anim.SetBool ("shoot", true);
			}
		} else if(Vector3.Distance (transform.position, playertransform.position) > maxwalkdistance) {
			//agent = gameObject.GetComponent<NavMeshAgent> ();
			agent.Stop();
			walking = false;
			anim.SetBool ("walking", false);
			anim.SetBool ("shoot", false);
		}
		if (Vector3.Distance (transform.position, playertransform.position) < shootdistance && (Time.time > (lastfiretime + shootdelay))) {
			fire ();

		}


	}




	void fire (){


		
		tijdelijke_bullet = Instantiate(bullet,enemybulletemittor.transform.position, enemybulletemittor.transform.rotation) as GameObject;

		//tijdelijke_bullet.transform.Rotate (Vector3.left *90);


		tijdelijke_rigidbody = tijdelijke_bullet.GetComponent<Rigidbody> ();

		tijdelijke_rigidbody.AddForce ( transform.forward * 3000);
		tijdelijke_rigidbody.AddTorque(transform.right * 100);
		//tijdelijke_bullet.transform.rotation = enemybulletemittor.transform.rotation;
		lastfiretime = Time.time;

		Destroy (tijdelijke_bullet, 2f);
	}
}
