using UnityEngine;
using System.Collections;

public class CameraVal : MonoBehaviour {
	// Variabelen voor besturing met de muis
	public Transform playerCam, centerPoint, targetPoint, player;
	private float mouseX, mouseY;
	public float mouseSensitivity = 10f; // Muisgevoeligheid die bij de instellingen veranderd moeten kunnen worden
	public float rStickSensitivity = 10f; // Gevoeligheid van de rechterstick van een controller
	Vector3 zoom, toZoom;
	RaycastHit hit;

	// Update is called once per frame
	void Update () {
		//Krijg info over de assen van de muislocatie
		if (Input.GetAxis ("Mouse X") != 0 || Input.GetAxis ("Mouse Y") != 0) {
			mouseX += Input.GetAxis ("Mouse X") * mouseSensitivity;
			mouseY -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		}

		//Zoom in door rechtermuisknop ingedrukt te houden
		if (Input.GetButton ("Aim") == true) {toZoom = new Vector3 (3, 0, -5);} else {toZoom = new Vector3 (0, 0, -1);}
		zoom = Vector3.Slerp (zoom, toZoom, Time.deltaTime * 50);

		//Dit zorgt ervoor dat de camera niet de y-as snijdt
		mouseY = Mathf.Clamp (mouseY, -90f, 90f);

		//De camera kijkt altijd naar het center point en het center point draait met de muisbeweging
		centerPoint.rotation = Quaternion.Euler (mouseY, mouseX, 0);
	}

	void FixedUpdate () {
		//Verplaats de camera
		playerCam.localPosition = Vector3.Slerp (playerCam.localPosition, zoom, Time.deltaTime * 15);

		//Vector3 collisionCheckEnd = playerCam.position + player.transform.up * 2;
		Debug.DrawLine (centerPoint.position, playerCam.position, Color.red);

		//Camera collision
		/*if (Physics.Linecast (centerPoint.position - centerPoint.forward * 0.5f, playerCam.position, out hit)) {
			playerCam.localPosition = hit.point + hit.normal * 0.14f;
		}*/
	}

	void LateUpdate(){
		//Camera collision
		if (Physics.Linecast (centerPoint.position - centerPoint.forward * 0.5f, playerCam.position, out hit)) {
			playerCam.position = hit.point + hit.normal * 0.14f;
		}

	}
}
