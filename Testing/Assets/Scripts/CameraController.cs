using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private GameObject crosshair;
	private Transform playerCam, centerPoint, targetPoint, player;
	private float mouseX, mouseY;
	Vector3 zoom, toZoom;
	RaycastHit hit;

	void Start() {
		crosshair = GameObject.Find ("Crosshair");
		player = GameObject.Find ("Player").transform;
		playerCam = GameObject.Find ("Player Camera").transform;
		centerPoint = GameObject.Find ("Center Point").transform;
		targetPoint = GameObject.Find ("Target Point").transform;
	}

	void Update () {
		//Krijg info over de assen van de muislocatie
		if (InputManager.camX != 0 || InputManager.camY != 0) {
			mouseX += InputManager.camX;
			mouseY += InputManager.camY;
		}
		//Dit zorgt ervoor dat de camera niet de y-as snijdt
		mouseY = Mathf.Clamp (mouseY, -70f, 90f);

		//Zoom in door rechtermuisknop ingedrukt te houden
		if (InputManager.aim.Hold == true) {
			if (InputManager.currentWeapon == 1) {
				Collider[] hitColliders = Physics.OverlapSphere (player.position, 100f, 8);

			} else {
				toZoom = new Vector3 (3, 0, -5);
			}
			crosshair.SetActive (true);
		} else {
			toZoom = new Vector3 (0, 0, -10);
			crosshair.SetActive (false);
		}
		targetPoint.localPosition = Vector3.Slerp (targetPoint.localPosition, toZoom, Time.deltaTime * 50f);
		//targetPoint.position = Vector3.Slerp (targetPoint.position, toPosition, Time.deltaTime * 50f);

		//De camera kijkt altijd naar het center point en het center point draait met de muisbeweging
		centerPoint.rotation = Quaternion.Euler (mouseY, mouseX, 0);
	}

	void LateUpdate () {
		//Camera collision
		if (Physics.Linecast (centerPoint.position - centerPoint.forward * 0.5f, targetPoint.position, out hit)) {
			if (hit.transform.GetComponent<Breakable> () != null) {

			} else {
				
			}
			playerCam.position = hit.point + hit.normal * 0.30f;
		} else {
			//Verplaats de camera
			playerCam.position = Vector3.Slerp (playerCam.position, targetPoint.position, Time.deltaTime * 100);
		}
		playerCam.rotation = Quaternion.Slerp (playerCam.rotation, targetPoint.rotation, Time.deltaTime * 100f);
	}
}