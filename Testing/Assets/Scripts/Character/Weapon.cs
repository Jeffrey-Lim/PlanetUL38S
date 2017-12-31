using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {
	private Transform player, playerCam, gunPoint, centerPoint;
	public GameObject impactEffect, arrowPrefab, grenadePrefab;
	private GameObject shotgunRange;
	public static int currentWeapon, lastWeapon;
	public static float damage;
	private float draw = 0;
	private bool canFire = true;
	private float force;
	Ray aimRay;
	RaycastHit aimHit;
	//Variabelen voor het herladen
	public static int[] maxAmmo, currentAmmo, inMagazine, maxMagazine;
	public float[] reloadTime;
	private bool reloading = false;

	void Start() {
		player = GameObject.Find ("Player").transform;
		playerCam = GameObject.Find ("Player Camera").transform;
		gunPoint = GameObject.Find ("Gun Point").transform;
		centerPoint = GameObject.Find ("Center Point").transform;
		shotgunRange = GameObject.Find ("Shotgun Range");
		shotgunRange.SetActive (false);

		maxAmmo = SaveFile.maxAmmo;
		currentAmmo = SaveFile.currentAmmo;
		maxMagazine = SaveFile.maxMagazine;
		inMagazine = new int[] {Mathf.Clamp(currentAmmo[0], 0, 1), Mathf.Clamp(currentAmmo[1], 0, 10), Mathf.Clamp(currentAmmo[2], 0, 6), Mathf.Clamp(currentAmmo[3], 0, 4), Mathf.Clamp(currentAmmo[4], 0, 1)};
		lastWeapon = currentWeapon;
	}

	void Update () {
		gunPoint.rotation = Quaternion.Euler (new Vector3 (centerPoint.rotation.eulerAngles.x, player.rotation.eulerAngles.y));
		currentWeapon = InputManager.currentWeapon;
		if (currentWeapon != lastWeapon) {
			reloading = false;
			lastWeapon = currentWeapon;
			inMagazine [0] = 0;
		}

		if (currentWeapon == 5) {
			shotgunRange.SetActive (true);
		} else {
			shotgunRange.SetActive (false);
		}
			
		if (reloading == true) {
			draw = 0f;
			canFire = false;
			return;
		}

		if (currentWeapon != 1) {
			if (currentAmmo [currentWeapon - 2] >= 1 && (inMagazine [currentWeapon - 2] == 0 || InputManager.reload.Pressed == true) && reloading == false && inMagazine [currentWeapon - 2] != maxMagazine [currentWeapon - 2]) {
				reloading = true;
				StartCoroutine (Reload (currentWeapon));
				return;
			}
		}

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
					if (inMagazine [currentWeapon - 2] >= 1) {
						//Maak een pijl en vuur die af
						GameObject arrow = Instantiate (arrowPrefab, gunPoint.position, gunPoint.rotation) as GameObject;
						arrow.GetComponent<Rigidbody> ().AddForce (gunPoint.forward * force * draw);
						currentAmmo [currentWeapon - 2]--;
						inMagazine [currentWeapon - 2]--;
					}
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
				if (InputManager.fire.Pressed == true) {
					if (currentAmmo[currentWeapon - 2] >= 1) {
						currentAmmo [currentWeapon - 2]--;
						inMagazine [currentWeapon - 2]--;
						if (Physics.Raycast (aimRay, out aimHit, 100f, ~(1 << 4))) {
							if (aimHit.transform.gameObject.name != "Shotgun Range") {
								Transform target = aimHit.transform;
								GameObject impactGO = Instantiate (impactEffect, aimHit.point, Quaternion.LookRotation (aimHit.normal)) as GameObject;
								Destroy (impactGO, 0.2f);
								//Als het object stuk kan gaan
								if (target.GetComponent<Rigidbody> () != null) {
									Vector3 heading = target.position - player.position;
									float distance = heading.magnitude;
									Vector3 direction = heading / distance;
									target.GetComponent<Rigidbody> ().AddForceAtPosition (direction * force, aimHit.point);
								}
								if (target.GetComponent<Breakable> () != null) {
									target.GetComponent<Breakable> ().TakeDamage (damage);
								}
							}
						}
					}
				}
			}
			break;
		case 5: //Shotgun
			damage = 30f;
			force = 500f;

			if ((PlayerController.playerstate == 0 && InputManager.aim.Hold == true) || PlayerController.playerstate == 1) {
				if (InputManager.fire.Pressed == true) { 
					if (currentAmmo [currentWeapon - 2] >= 1) {
						currentAmmo [currentWeapon - 2]--;
						inMagazine [currentWeapon - 2]--;
						foreach (Collider col in shotgunRange.GetComponent<ShotgunCollision> ().colliders) {
							Transform target = col.transform;
							//Als het object stuk kan gaan
							if (target.GetComponent<Rigidbody> () != null) {
								Vector3 heading = target.position - player.position;
								float distance = heading.magnitude;
								Vector3 direction = heading / distance;
								target.GetComponent<Rigidbody> ().AddForce (direction * force);
							}

							if (target.GetComponent<Breakable> () != null) {
								target.GetComponent<Breakable> ().TakeDamage (damage);
							}
						}
					}
				}
			}

			break;
		case 6:
			damage = 100f;
			force = 100f;

			if ((PlayerController.playerstate == 0 && InputManager.aim.Hold == true) || PlayerController.playerstate == 1) {
				if (InputManager.fire.Pressed == true) { 
					if (currentAmmo [currentWeapon - 2] >= 1) {
						currentAmmo [currentWeapon - 2]--;
						inMagazine [currentWeapon - 2]--;
						GameObject grenade = Instantiate (grenadePrefab, gunPoint.position, gunPoint.rotation) as GameObject;
						grenade.GetComponent<Rigidbody> ().AddForce (gunPoint.forward * force);
					}
				}
			}
			break;
		default: 
			break;
		}
	}

	void OnGUI () {
		if (currentWeapon == 1) {
			GUI.Box (new Rect (0, Screen.height - 60, 100, 50), '\u221E'.ToString());
		} else {
			GUI.Box (new Rect (0, Screen.height - 60, 100, 50), inMagazine [currentWeapon - 2].ToString () + "/" + currentAmmo [currentWeapon - 2].ToString ());
		}
	}

	IEnumerator Reload (int weapon) {
		reloading = true;
		Debug.Log ("Reloading");
		yield return new WaitForSeconds (reloadTime[weapon - 2]);
		Debug.Log ("Done");
		if (reloading == true) {
			inMagazine [weapon - 2] = Mathf.Clamp (currentAmmo [weapon - 2], 0, maxMagazine [weapon - 2]); 
			reloading = false;
		} else {
			yield break;
		}
	}
}
