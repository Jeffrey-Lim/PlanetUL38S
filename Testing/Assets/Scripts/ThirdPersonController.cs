using UnityEngine;
using System.Collections;

public class ThirdPersonController : MonoBehaviour {

	//Variabelen voor transformaties
	public Transform playerCam, character, centerPoint;

	private float mouseX, mouseY;
	public float mouseSensitivity = 10f;
	public float mouseYPosition = 2f;

	//Variabelen voor bewegen
	private float moveFB, moveLR, moveUD;
	public float moveSpeed = 6f;
	public float rotationSpeed = 5f;
	public float jumpPower = 6f;
	public float gravity = 20f;

	//Variablen voor camera zoom
	public float zoomSpeed = 2f;
	public float zoomMin = -5f;
	public float zoomMax = -7f;

	private Vector3 zoomVector = new Vector3 (0, 0, -7);
	private Vector3 toZoomVector;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		//Krijg info over de assen van de muislocatie
		mouseX += Input.GetAxis ("Mouse X") * mouseSensitivity;
		mouseY -= Input.GetAxis ("Mouse Y") * mouseSensitivity;

		//Zoom in door rechtermuisknop ingedrukt te houden
		if (Input.GetKey (KeyCode.Mouse1)) {
			toZoomVector = new Vector3 (3, 0, zoomMin);
		} else {
			toZoomVector = new Vector3 (0, 0, zoomMax);
		}
		zoomVector = Vector3.Slerp (zoomVector, toZoomVector, Time.deltaTime * zoomSpeed);

		//Dit zorgt ervoor dat de camera niet de y-as van het personage snijdt
		mouseY = Mathf.Clamp (mouseY, -90f, 90f);

		//Zet de camera op de juiste coordinaten ten opzichte van het center point
		playerCam.transform.localPosition = zoomVector;

		//De camera kijkt altijd naar het center point en het center point draait met de muisbeweging
		centerPoint.localRotation = Quaternion.Euler (mouseY, mouseX, 0);

		//FB staat voor Forward Backward, LR voor Left Right, UD voor Up Down
		moveFB = Input.GetAxis ("Vertical") * moveSpeed;
		moveLR = Input.GetAxis ("Horizontal") * moveSpeed;
		//Lol, springen werkt niet zo goed. 
		if (Input.GetKey(KeyCode.Space) == true) {moveUD = jumpPower;} else {moveUD = 0;}

		Vector3 movement = new Vector3 (moveLR, 0, moveFB);
		Vector3 movementDir = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		Quaternion turnAngle = Quaternion.Euler (0, centerPoint.eulerAngles.y, 0);

		if (Input.GetKey (KeyCode.Mouse1)) {
			//Draai geleidelijk naar de bewegingsrichting toe
			character.rotation = Quaternion.Slerp (character.rotation, turnAngle, Time.deltaTime * rotationSpeed);
		} else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
			//Als je beweegt, kijk je naar de richting waarheen je gaat
			Quaternion lookRotation = Quaternion.LookRotation (movementDir) * Quaternion.Euler (0, centerPoint.eulerAngles.y, 0);
			character.rotation = Quaternion.Slerp (character.rotation, lookRotation, Time.deltaTime * rotationSpeed);
		}

		//Draai de bewegingen relatief naar de draaing van het personage
		movement = Quaternion.Euler (0, centerPoint.eulerAngles.y, 0) * movement;
		movement.y = character.GetComponent<Rigidbody> ().velocity.y + Physics.gravity.y * gravity * Time.deltaTime;
		//Beweeg het personage
		character.GetComponent<Rigidbody> ().velocity = movement;
		//Het center point volgt het personage
		centerPoint.position = new Vector3 (character.position.x, character.position.y + mouseYPosition, character.position.z);

		//Camera collision
		Debug.DrawLine (centerPoint.position, playerCam.position, Color.red);
		RaycastHit hit;
		if (Physics.Linecast (centerPoint.position - centerPoint.forward * 0.5f, playerCam.position, out hit)) {
			//Vector3 newPos = new Vector3 (hit.point.x + hit.normal.x * 2f, hit.point.y + hit.normal.y * 2f, hit.point.z + hit.normal.z * 2f);
			playerCam.position = new Vector3 (hit.point.x, hit.point.y, hit.point.z);
		}
	}
}