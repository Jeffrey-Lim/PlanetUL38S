using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour {
	private Transform player, playerCam, gunPoint, centerPoint;
	private Text ammoAmount;
	private GameObject machetePanel, bowPanel, pistolPanel, revolverPanel, shotgunPanel, grenadePanel;
	private GameObject macheteWeapon, bowWeapon, pistolWeapon, revolverWeapon, shotgunWeapon, grenadeWeapon;
	//private int aimType; //0 = Geen, 1 = Boog, 2 = Pistool & Revolver, 3 = Shotgun & Granaatwerper
	private Animator anim, bowAnim;
	private Collider macheteHitbox;
	private GameObject[] panels, weapons;
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
	public float reloadTime;
	private bool reloading = false;
	//Geluiden
	public AudioClip bowDraw;
	public AudioClip[] shootingSounds;
	public AudioClip reloadSound;
	private AudioSource reloadSource;
	private AudioSource shootSource;

	private List<GameObject> beenHit;

	void Start() {
		player = GameObject.Find ("Player").transform;
		anim = player.GetComponent<Animator> ();
		playerCam = GameObject.Find ("Player Camera").transform;
		gunPoint = GameObject.Find ("Gun Point").transform;
		reloadSource = GameObject.Find("Reload Sound").GetComponent<AudioSource> ();
		shootSource = GameObject.Find("Shoot Sound").GetComponent<AudioSource> ();
		centerPoint = GameObject.Find ("Center Point").transform;
		shotgunRange = GameObject.Find ("Shotgun Range");
		shotgunRange.SetActive (false);

		//De wapens als gameobject in je hand
		macheteWeapon = GameObject.Find("Machete Weapon");
		macheteHitbox = macheteWeapon.GetComponent<Collider> ();
		macheteHitbox.enabled = true;
		bowWeapon = GameObject.Find("Bow Weapon");
		bowAnim = bowWeapon.GetComponent<Animator> ();
		pistolWeapon = GameObject.Find("10mm Pistol Weapon");
		revolverWeapon = GameObject.Find("Revolver Weapon");
		shotgunWeapon = GameObject.Find("Shotgun Weapon");
		grenadeWeapon = GameObject.Find("Grenade Weapon");

		weapons = new GameObject[] { macheteWeapon, bowWeapon, pistolWeapon, revolverWeapon, shotgunWeapon, grenadeWeapon };
		/*foreach (GameObject x in weapons) {
			x.SetActive (false);
		}*/

		//GUI Spul
		ammoAmount = GameObject.Find ("Ammo Amount").GetComponent<Text> ();
		machetePanel = GameObject.Find ("Machete Panel");
		bowPanel = GameObject.Find ("Bow Panel");
		pistolPanel = GameObject.Find ("10mm Pistol Panel");
		revolverPanel = GameObject.Find ("Revolver Panel");
		shotgunPanel = GameObject.Find ("Shotgun Panel");
		grenadePanel = GameObject.Find ("Grenade Launcher Panel");
		panels = new GameObject[] {machetePanel, bowPanel, pistolPanel, revolverPanel, shotgunPanel, grenadePanel};

		maxAmmo = SaveFile.maxAmmo;
		currentAmmo = SaveFile.currentAmmo;
		maxMagazine = SaveFile.maxMagazine;
		inMagazine = new int[] {Mathf.Clamp(currentAmmo[0], 0, 1), Mathf.Clamp(currentAmmo[1], 0, 10), Mathf.Clamp(currentAmmo[2], 0, 6), Mathf.Clamp(currentAmmo[3], 0, 4), Mathf.Clamp(currentAmmo[4], 0, 1)};
		lastWeapon = currentWeapon;

		beenHit = new List<GameObject> ();
	}

	void Update () {
		/*if (Physics.Raycast (playerCam.position, playerCam.position - playerCam.forward, out aimHit, 100f, ~(1 << 4))) {
			Quaternion lookDir = Quaternion.LookRotation (Vector3.MoveTowards (gunPoint.position, aimHit.point, 100f).normalized);
			gunPoint.rotation = lookDir;
		}*/
		gunPoint.rotation = Quaternion.Euler (new Vector3 (centerPoint.rotation.eulerAngles.x, player.rotation.eulerAngles.y));
		currentWeapon = InputManager.currentWeapon;
		if (currentWeapon != lastWeapon) {
			reloading = false;
			anim.SetInteger ("Reload Type", 0);
			reloadSource.Stop ();
			lastWeapon = currentWeapon;
			inMagazine [0] = 0;
		}

		foreach (GameObject x in weapons) {
			x.SetActive (false);
		}
		weapons [currentWeapon - 1].SetActive (true);

		anim.SetInteger ("Aim Type", 0);

		if (currentWeapon == 5) {
			shotgunRange.SetActive (true);
		} else {
			shotgunRange.SetActive (false);
		}

		//Activeer de bijbehorende panel
		foreach (GameObject panel in panels) {
			panel.SetActive (false);
		}
		panels [currentWeapon - 1].SetActive (true);

		if (currentWeapon == 1) {
			ammoAmount.text = '\u221E'.ToString();
		} else {
			ammoAmount.text = inMagazine [currentWeapon - 2].ToString () + "/" + currentAmmo [currentWeapon - 2].ToString ();
		}
			
		//weapons [currentWeapon - 1].SetActive (true);

		if (reloading == true) {
			anim.SetInteger ("Reload Type", currentWeapon - 1);
			draw = 0f;
			canFire = false;
			return;
		}

		if (currentWeapon != 1) {
			if (currentAmmo [currentWeapon - 2] >= 1 && (inMagazine [currentWeapon - 2] == 0 || InputManager.reload.Pressed == true) && reloading == false && inMagazine [currentWeapon - 2] != maxMagazine [currentWeapon - 2]) {
				reloading = true;
				anim.SetInteger ("Reload Type", currentWeapon - 1);
				StartCoroutine (Reload (currentWeapon));
				return;
			}
		}

		switch (currentWeapon) {
		case 1: //Machette
			damage = 50f;
			force = 100f;

			if (InputManager.fire.Pressed == true) { 
				anim.SetTrigger ("Slash");
			}

			if (anim.GetCurrentAnimatorStateInfo (2).IsName ("1st Slash") || anim.GetCurrentAnimatorStateInfo (2).IsName ("2nd Slash")) {
				foreach (Collider col in macheteHitbox.GetComponent<MacheteCollision> ().colliders) {
					if (!beenHit.Contains(col.gameObject)) {
						beenHit.Add (col.gameObject);
						if (col.GetComponent<Breakable> () != null) {
							col.GetComponent<Breakable> ().TakeDamage (damage);
						}
						if (col.attachedRigidbody != null) {
							col.attachedRigidbody.AddForce ((macheteWeapon.transform.position - player.position).normalized * force);
						}
						StartCoroutine ( canHit (col.gameObject));
					}
				}
			}

			break;
		case 2: //Pijl en Boog
			damage = 10f;
			force = 100f;

			if ((PlayerController.playerstate == 0 && InputManager.aim.Hold == true)) {
				anim.SetInteger ("Aim Type", 1);

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
					anim.SetFloat ("Bow Draw", draw);
					bowAnim.SetFloat ("Bow Draw", draw);
				}
				if (InputManager.fire.Release && draw > 0.2f) {
					if (inMagazine [currentWeapon - 2] >= 1) {
						//Maak een pijl en vuur die af
						GameObject arrow = Instantiate (arrowPrefab, gunPoint.position, gunPoint.rotation) as GameObject;
						arrow.GetComponent<Rigidbody> ().AddForce (gunPoint.forward * force * draw);
						currentAmmo [currentWeapon - 2]--;
						inMagazine [currentWeapon - 2]--;
						anim.SetTrigger ("Shoot");
						bowAnim.SetTrigger ("Shoot");
						shootSource.clip = shootingSounds [currentWeapon - 1];
						shootSource.Play ();
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

			if ((PlayerController.playerstate == 0 && InputManager.aim.Hold == true)) {
				anim.SetInteger ("Aim Type", 2);
				//Je schiet op het punt dat in het midden van het zicht van de camera zit
				aimRay.origin = playerCam.position;
				aimRay.direction = playerCam.forward;
				if (InputManager.fire.Pressed == true) {
					if (currentAmmo[currentWeapon - 2] >= 1) {
						currentAmmo [currentWeapon - 2]--;
						inMagazine [currentWeapon - 2]--;
						anim.SetTrigger ("Shoot");
						shootSource.clip = shootingSounds [currentWeapon - 1];
						shootSource.Play ();
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

			if ((PlayerController.playerstate == 0 && InputManager.aim.Hold == true)) {
				anim.SetInteger ("Aim Type", 3);
				if (InputManager.fire.Pressed == true) { 
					if (currentAmmo [currentWeapon - 2] >= 1) {
						currentAmmo [currentWeapon - 2]--;
						inMagazine [currentWeapon - 2]--;
						anim.SetTrigger ("Shoot");
						shootSource.clip = shootingSounds [currentWeapon - 1];
						shootSource.Play ();
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

			if ((PlayerController.playerstate == 0 && InputManager.aim.Hold == true)) {
				anim.SetInteger ("Aim Type", 3);
				if (InputManager.fire.Pressed == true) { 
					if (currentAmmo [currentWeapon - 2] >= 1) {
						currentAmmo [currentWeapon - 2]--;
						inMagazine [currentWeapon - 2]--;
						anim.SetTrigger ("Shoot");
						shootSource.clip = shootingSounds [currentWeapon - 1];
						shootSource.Play ();
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

	IEnumerator Reload (int weapon) {
		if (weapon - 2 != 0) {
			reloadSource.clip = reloadSound;
			reloadSource.Play ();
		} else {
			reloading = true;
			anim.SetInteger ("Reload Type", currentWeapon - 1);
			yield return new WaitForSeconds (1);
			inMagazine [0] = 1;
			reloading = false;
			anim.SetInteger ("Reload Type", 0);
			yield break;
		}
		reloading = true;
		anim.SetInteger ("Reload Type", currentWeapon - 1);
		yield return new WaitForSeconds (reloadTime);
		if (reloading == true) {
			inMagazine [weapon - 2] = Mathf.Clamp (currentAmmo [weapon - 2], 0, maxMagazine [weapon - 2]); 
			reloading = false;
			anim.SetInteger ("Reload Type", 0);
		} else {
			yield break;
		}
	}

	IEnumerator canHit (GameObject x) {
		yield return new WaitForSeconds (0.5f);
		beenHit.Remove (x);
	}
}
