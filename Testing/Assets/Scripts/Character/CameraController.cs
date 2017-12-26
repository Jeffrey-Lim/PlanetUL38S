using UnityEngine;
using System.Collections;
using System.Linq;

public class CameraController : MonoBehaviour {
	private GameObject crosshair;
	private Transform playerCam, centerPoint, targetPoint, player;
	public static Transform target;
	private float mouseX, mouseY, minHeight = -50f, maxHeight = 90f;
	private int cameraMode = 0;
	Vector3 zoom, toZoom;
	RaycastHit hit;

	void Start() {
		crosshair = GameObject.Find ("Crosshair");
		player = GameObject.Find ("Player").transform;
		playerCam = GameObject.Find ("Player Camera").transform;
		centerPoint = GameObject.Find ("Center Point").transform;
		targetPoint = GameObject.Find ("Target Point").transform;
	}

	Transform GetClosestTarget (Collider[] targets) {
		Transform tMin = null;
		float minDist = 10000f; // = 100^2
		foreach (Collider c in targets) {
			float dist = (c.transform.position - player.position).sqrMagnitude;
			if (dist < minDist && c.GetComponent<Breakable>().health >= 0f) {
				tMin = c.transform;
				minDist = dist;
			}
		}
		return tMin;
	}

	void Update () {
		cameraMode = InputManager.cameraMode;
		//Krijg info over de assen van de muislocatie
		if (InputManager.camX != 0 || InputManager.camY != 0) {
			mouseX += InputManager.camX;
			mouseY += InputManager.camY;
		}

		//Zoom in door rechtermuisknop ingedrukt te houden
		if (cameraMode == 2) {
			minHeight = 0f;
			Collider[] hitColliders = Physics.OverlapSphere (player.position, 100f, 1<<8);
			if (target != null) {
				centerPoint.position = Vector3.Slerp (centerPoint.position, (target.position + player.position) / 2f, Time.deltaTime * 25f);
				toZoom = new Vector3 (0, 0, -10 - (target.position - player.position).magnitude);
				crosshair.SetActive (false);
				if ((player.position - target.position).sqrMagnitude >= 10000f) {
					target = null;
				}
			} else {
				minHeight = -50f;
				target = GetClosestTarget (hitColliders);
				toZoom = new Vector3 (0, 0, -10);
				crosshair.SetActive (false);
				//Het center point volgt het personage
				centerPoint.position = new Vector3 (player.position.x, player.position.y + 2f, player.position.z);
			}
		} else if (cameraMode == 1) {
			minHeight = -50f;
			target = null;
			crosshair.SetActive (true);
			toZoom = new Vector3 (2, 0, -5);
			//Het center point volgt het personage
			centerPoint.position = new Vector3 (player.position.x, player.position.y + 2f, player.position.z);
		} else if (cameraMode == 0) {
			minHeight = -50f;
			target = null;
			toZoom = new Vector3 (0, 0, -10);
			crosshair.SetActive (false);
			//Het center point volgt het personage
			centerPoint.position = new Vector3 (player.position.x, player.position.y + 2f, player.position.z);
		}

		targetPoint.localPosition = Vector3.Slerp (targetPoint.localPosition, toZoom, Time.deltaTime * 25f);
		//targetPoint.position = Vector3.Slerp (targetPoint.position, toPosition, Time.deltaTime * 50f);

		//Dit zorgt ervoor dat de camera niet de y-as snijdt
		mouseY = Mathf.Clamp (mouseY, minHeight, maxHeight);

		//De camera kijkt altijd naar het center point en het center point draait met de muisbeweging
		centerPoint.rotation = Quaternion.Euler (mouseY, mouseX, 0);
	}

	void LateUpdate () {
		//Camera collision
		if (Physics.Linecast (player.position - centerPoint.forward * 0.5f, targetPoint.position, out hit, ~(1 << 8))) {
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