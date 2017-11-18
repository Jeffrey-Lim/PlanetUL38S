using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	private Transform player, playerCam, gunPoint, centerPoint;
	public GameObject impactEffect, arrowModel;
	public static int currentWeapon;
	public static float damage;
	private float draw = 0;
	private bool canFire = true;
	public float force;
	Ray aimRay;
	RaycastHit aimHit;

	void Start() {
		player = GameObject.Find ("Player").transform;
		playerCam = GameObject.Find ("Player Camera").transform;
		gunPoint = GameObject.Find ("Gun Point").transform;
		centerPoint = GameObject.Find ("Center Point").transform;
	}

	void Update () {
		gunPoint.rotation = Quaternion.Euler (new Vector3 (centerPoint.rotation.eulerAngles.x, player.rotation.eulerAngles.y));
		currentWeapon = InputManager.currentWeapon;
		switch (currentWeapon) {
		case 1: //Machette
			damage = 50f;
			force = 200f;
			break;
		case 2: //Pijl en Boog
			damage = 10f;
			force = 100f;
			if ((PlayerController.playerstate == 0 && InputManager.aim.Hold == true) || PlayerController.playerstate == 1) {
				//Als je terwijl de boog gespannen is op herladen drukt, annuleer je het schieten
				if ((InputManager.reload.Pressed && draw > 0) || canFire == false) {
					draw -= 2f * Time.deltaTime;
					canFire = false;
				}
				if (draw <= 0 && !InputManager.fire.Hold) {
					canFire = true;
				}
				//Hoe langer je de boog aanspant, hoe verder de pijl komt
				if (InputManager.fire.Hold && canFire == true) {
					draw += 1.5f * Time.deltaTime;
				}
				if (InputManager.fire.Release && draw > 0.2f) {
					//Maak een pijl en vuur die af
					GameObject arrow = Instantiate (arrowModel, gunPoint.position, gunPoint.rotation) as GameObject;
					arrow.GetComponent<Rigidbody> ().AddForce (transform.forward * force * draw);
					draw = 0f;
				} else if (InputManager.fire.Release && draw <= 0.2f) {
					draw -= 3f * Time.deltaTime;
					canFire = false;
				}
			} else {
				draw = 0f;
				canFire = true;
			}
			draw = Mathf.Clamp (draw, 0f, 1f);
			break;
		case 3: //10mm Pistool
		case 4: //Revolver
			//De revolver is sterker dan het pistool
			if (currentWeapon == 3) {
				damage = 10f;
				force = 100f;
			} else if (currentWeapon == 4) {
				damage = 20f;
				force = 100f;
			}

			if ((PlayerController.playerstate == 0 && InputManager.aim.Hold == true) || PlayerController.playerstate == 1) {
				//Je schiet op het punt dat in het midden van het zicht van de camera zit
				aimRay.origin = playerCam.position;
				aimRay.direction = playerCam.forward;
				if (Physics.Raycast (aimRay, out aimHit)) {
					if (InputManager.fire.Pressed == true) {
						Transform target = aimHit.transform;
						GameObject impactGO = Instantiate (impactEffect, aimHit.point, Quaternion.LookRotation (aimHit.normal)) as GameObject;
						Destroy (impactGO, 0.2f);
						//Als het object stuk kan gaan
						if (target.GetComponent<Breakable> () != null) {
							target.GetComponent<Breakable> ().TakeDamage (damage);
						}
						if (target.GetComponent<Rigidbody> () != null) {
							Vector3 heading = target.position - player.position;
							float distance = heading.magnitude;
							Vector3 direction = heading / distance;
							target.GetComponent<Rigidbody> ().AddForceAtPosition (direction * force, aimHit.point);
						}
					}
				}
			}
			break;
		default: 
			break;
		}
	}
}
