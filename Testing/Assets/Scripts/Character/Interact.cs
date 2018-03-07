using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Collections;

public class Interact : MonoBehaviour {
	private Transform player;
	private GameObject interactionInfo;
	private AudioSource eatSource;
	private Text actionText;
	private Text infoText;
	private Text addText;
	private Text otherText;
	private GameObject infoBox;
	//private RawImage preview;
	private float distance = 7f;
	private string[] ammoNames, ammoNamesSingle;

	void Start() {
		player = GameObject.Find ("Player").transform;
		interactionInfo = GameObject.Find ("Interaction Info");
		infoBox = GameObject.Find ("Info Box");
		actionText = GameObject.Find ("Action Text").GetComponent<Text> ();
		infoText = GameObject.Find ("Info Text").GetComponent<Text> ();
		addText = GameObject.Find ("Additional Text").GetComponent<Text> ();
		otherText = GameObject.Find ("Other Text").GetComponent<Text> ();
		//preview = GameObject.Find ("Preview Image").GetComponent<RawImage> ();
		eatSource = GameObject.Find("Eat Sound Source").GetComponent<AudioSource> ();

		interactionInfo.SetActive (false);

		ammoNames = new string[] {"Arrow", "10mm Pistol Bullet", "Revolver Bullet", "Shotgun Shell", "Grenade"};
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
		if (player.GetComponent<Breakable> ().health > 0) {
			infoBox.SetActive (true);
			Collider[] hitColliders = Physics.OverlapSphere (player.position, distance, 1 << 11);
			if (hitColliders.Length >= 1) {
				Transform target = GetClosestTarget (hitColliders);
				if ((target.position - player.position).sqrMagnitude <= 16f) {

					interactionInfo.SetActive (true);

					if (target.GetComponent<Ammo> () != null) {
						AmmoData data = target.GetComponent<Ammo> ().data;
						addText.text = "";
						infoText.text = "";
						if (target.GetComponent<Ammo> ().amount == 1) {
							otherText.text = "+ 1 " + ammoNames [data.weaponType];
						} else if (target.GetComponent<Ammo> ().amount >= 1000) {
							otherText.text = "+ Max " + ammoNames [data.weaponType] + "s";
						} else {
							otherText.text = "+ " + target.GetComponent<Ammo> ().amount + " " + ammoNames [data.weaponType] + "s";
						}
						if (Weapon.currentAmmo [data.weaponType] < Weapon.maxAmmo [data.weaponType]) {
							actionText.text = "Pick Up";
							if (InputManager.interact.Pressed == true) {
								Weapon.currentAmmo [data.weaponType] = Mathf.Clamp (Weapon.currentAmmo [data.weaponType] + target.GetComponent<Ammo> ().amount, 0, Weapon.maxAmmo [data.weaponType]);
								if (data.destroyOnPickUp == true) {
									//Zorgt ervoor dat pijlen die in het voorwerp zijn geschoten niet verdwijnen;
									GameObject[] Arrows = GameObject.FindGameObjectsWithTag ("Arrow");
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
						} else {
							actionText.text = "Pick Up <color=red>(Max Ammo)</color>";
						}

					} else if (target.GetComponent<Item> () != null) {
						ItemData data = target.GetComponent<Item> ().data;
						otherText.text = "";
						infoText.text = data.itemName;
						addText.text = "+ " + data.restoreHealth + " HP";
						if (player.GetComponent<Breakable> ().health != GameObject.Find ("Player").GetComponent<Breakable> ().data.health) {
							actionText.text = "Eat";
							if (InputManager.interact.Pressed == true) {
								eatSource.Play ();

								if (data.restoreHealth != 0f) {
									player.GetComponent<Breakable> ().health += data.restoreHealth;
									if (player.GetComponent<Breakable> ().health >= player.GetComponent<Breakable> ().data.health) {
										player.GetComponent<Breakable> ().health = player.GetComponent<Breakable> ().data.health;
									}
								}

								if (data.dontDestroyOnPickUp == false) {
									//Zorgt ervoor dat pijlen die in het voorwerp zijn geschoten niet verdwijnen;
									GameObject[] Arrows = GameObject.FindGameObjectsWithTag ("Arrow");
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
						} else {
							actionText.text = "Eat <color=red>(Max Health)</color>";
						}

					} else if (target.GetComponent<Locker> () != null) {
						addText.text = "";
						infoText.text = "";
						int itemNumber = target.GetComponent<Locker> ().item;
						Animator anim = target.GetComponent<Animator> ();
						otherText.text = "+ Collectible " + itemNumber;
						if (target.GetComponent<Locker> ().isOpened == false) {
							actionText.text = "Open Locker";
							if (InputManager.interact.Pressed == true) {
								anim.SetTrigger ("open");
								target.GetComponent<Locker> ().isOpened = true;
								PlayerPrefs.SetInt ("Item" + itemNumber.ToString (), 1);
							}
						} else {
							actionText.text = "Open Locker \n<color=red>(Already opened)</color>";
						}

					} else if (target.GetComponent<Dialogue> () != null) {
						addText.text = "";
						infoText.text = "";
						otherText.text = "";
						Dialogue dialogue = target.GetComponent<Dialogue> ();
						DialogueData data = dialogue.data;
						DialogueManager manager = FindObjectOfType<DialogueManager> ();
						actionText.text = "Talk with " + data.partnerName;
						infoBox.SetActive (false);
						if (manager.inConversation == false && dialogue.canTalk <= 0f) {
							if (InputManager.interact.Pressed == true) {
								manager.StartDialogue (data, dialogue);
							}
						}
					}
				} else {
					interactionInfo.SetActive (false);
				}
			} else {
				interactionInfo.SetActive (false);
			}
		} else {
			interactionInfo.SetActive (false);
		}
	}
}
