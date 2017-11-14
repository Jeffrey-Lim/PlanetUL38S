using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public GameObject crosshair;
	// Variabelen voor besturing met de muis
	public Transform playerCam, centerPoint, targetPoint, player;
	private float mouseX, mouseY;
	Vector3 zoom, toZoom;
	RaycastHit hit;

	void Update () {
		//Krijg info over de assen van de muislocatie
		if (InputManager.camX != 0 || InputManager.camY != 0) {
			mouseX += InputManager.camX;
			mouseY += InputManager.camY;
		}

		//Zoom in door rechtermuisknop ingedrukt te houden
		if (InputManager.aim.Hold == true) {
			toZoom = new Vector3 (3, 0, -5);
			crosshair.SetActive (true);
		} else {
			toZoom = new Vector3 (0, 0, -10);
			crosshair.SetActive (false);
		}
		zoom = Vector3.Slerp (zoom, toZoom, Time.deltaTime * 50);

		//Dit zorgt ervoor dat de camera niet de y-as snijdt
		mouseY = Mathf.Clamp (mouseY, -90f, 90f);

		//De camera kijkt altijd naar het center point en het center point draait met de muisbeweging
		centerPoint.rotation = Quaternion.Euler (mouseY, mouseX, 0);
	}

	void LateUpdate () {
		//Camera collision
		/*if (Physics.Linecast (centerPoint.position - centerPoint.forward * 0.5f, zoom, out hit)) {
			playerCam.position = hit.point + hit.normal * 0.14f;
		}*/

		//Verplaats de camera
		playerCam.localPosition = Vector3.Slerp (playerCam.localPosition, zoom, Time.deltaTime * 15);
	}
}