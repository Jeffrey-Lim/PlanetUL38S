using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetrudeIsABitch : MonoBehaviour {
	
	private Transform player;
	private float minwalkdistance;
	private float maxwalkdistance;
	private NavMeshAgent agent;
	private Vector3 positionwarp;
	private Animator anim;

	void Awake () {
		player = GameObject.Find ("Player").transform;
		agent = gameObject.GetComponent<NavMeshAgent> ();
		anim = gameObject.GetComponent<Animator> ();
		minwalkdistance = 10;
		maxwalkdistance = 80;
	}

	void Update () {
		// print distance between gertude and player (GP)
		// test if GP is larger than maxwalk
		if (Vector3.Distance (transform.position, player.position) > maxwalkdistance) {
			positionwarp = new Vector3(player.position.x, player.position.y + 6, player.position.z);
			agent.velocity = Vector3.zero;
			agent.Stop ();
			agent.Warp(positionwarp);
			anim.SetBool ("walking", false);
		} 
		// test if GP is smaller than the walking distance
		else if (Vector3.Distance (transform.position, player.position) < minwalkdistance) {
			agent.velocity = Vector3.zero;
			agent.Stop ();
			anim.SetBool ("walking", false);
		} 
		// else GP is between the min and max so the gertrude follows the player
		else {
			agent.SetDestination (player.position);
			agent.Resume ();
			anim.SetBool ("walking", true);
		}

	}



}
