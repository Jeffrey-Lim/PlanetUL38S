﻿using UnityEngine;
using System.Collections;

public class Interact : MonoBehaviour {
	private Transform player;
	private float distance = 7f;

	void Start() {
		player = GameObject.Find ("Player").transform;
	}

	//Script om het dichtsbijzijnde object uit een collider array te vinden
	Transform GetClosestTarget (Collider[] targets) {
		Transform tMin = null;
		float minDist = Mathf.Infinity; //Pow(distance, 2) + 1f;
		foreach (Collider c in targets) {
			float dist = (c.transform.position - player.position).sqrMagnitude;
			if (dist < minDist) {
				tMin = c.transform;
				minDist = dist;
			}
		}
		return tMin;
	}

	void Update() {
		Collider[] hitColliders = Physics.OverlapSphere (player.position, distance, 1<<11);
		if (hitColliders.Length >= 1) {
			Transform target = GetClosestTarget (hitColliders);
			if ((target.position - player.position).sqrMagnitude <= 16f) {
				if (target.GetComponent<Ammo> () != null) {
					AmmoData data = target.GetComponent<Ammo> ().data;
					if (InputManager.interact.Pressed == true) {
						if (Weapon.currentAmmo [data.weaponType] < Weapon.maxAmmo [data.weaponType]) {
							Weapon.currentAmmo [data.weaponType] = Mathf.Clamp (Weapon.currentAmmo [data.weaponType] + target.GetComponent<Ammo> ().amount, 0, Weapon.maxAmmo [data.weaponType]);
							if (data.destroyOnPickUp == true) {
								//Zorgt ervoor dat pijlen die in het voorwerp zijn geschoten niet verdwijnen;
								GameObject[] Arrows = GameObject.FindGameObjectsWithTag("Arrow");
								foreach (GameObject Arrow in Arrows) {
									if (Arrow.transform.parent != null) {
										if (Arrow.transform.parent.gameObject == target.gameObject) {
											Arrow.transform.parent = null;
											Arrow.GetComponent<ArrowSticker> ().isSticking = false;
										}
									}
								}
								Destroy (target.gameObject);
							}
						}
					}
				} else if (target.GetComponent<Item> () != null) {
					ItemData data = target.GetComponent<Item> ().data;
					if (InputManager.interact.Pressed == true) {
						
						if (data.destroyOnPickUp == true) {
							//Zorgt ervoor dat pijlen die in het voorwerp zijn geschoten niet verdwijnen;
							GameObject[] Arrows = GameObject.FindGameObjectsWithTag("Arrow");
							foreach (GameObject Arrow in Arrows) {
								if (Arrow.transform.parent != null) {
									if (Arrow.transform.parent.gameObject == target.gameObject) {
										Arrow.transform.parent = null;
										Arrow.GetComponent<ArrowSticker> ().isSticking = false;
									}
								}
							}
							Destroy (target.gameObject);
						}
					}
				}
			}
		}
	}
}