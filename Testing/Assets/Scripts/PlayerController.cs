using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public Transform centerPoint, player, playerCam;
	private float moveFB, moveLR, moveUD; //FB staat voor Forward Backward, LR voor Left Right, UD voor Up Down
	public float moveSpeed = 20f;
	public float rotationSpeed = 20f;
	public float jumpPower = 6f;
	public float gravity = 20f;
	public static int playerstate = 0; /* 
	 * 0 = Normaal. De speler staat in het midden van je zicht, je kan alleen schieten als je richt.
	 * 1 = Bij actie. De ligging van de camera is iets anders zodat je kan schieten zonder te richten en je loopt iets sneller.
	 * 2 = Bij rustige stukjes. De camera staat dichterbij de speler, je kan niet aanvallen en je loopt rustiger.
	 */
	public static bool running = false;
	Vector3 movementDir, movement;
	Ray aimRay;
	RaycastHit aimHit;

	//Pauper functie die checkt of je op de grond staat met een linecast
	bool IsGrounded () {
		return Physics.Linecast (player.transform.position, player.transform.position - Vector3.up);
	}

	void Update () {
		movementDir = InputManager.movementDir;
		running = InputManager.running;
		Quaternion forward = Quaternion.Euler (0, centerPoint.eulerAngles.y, 0);

		switch (playerstate) {
		case 0: //Normaal
		case 1: //Bij actie
			if (InputManager.aim.Hold) {
				aimRay.origin = playerCam.position;
				aimRay.direction = playerCam.forward;
				if (Physics.Raycast (aimRay, out aimHit, 100f)) {
					Quaternion lookDir = Quaternion.LookRotation (aimHit.point - player.position);
					player.rotation = Quaternion.Slerp (player.rotation, lookDir, Time.deltaTime * rotationSpeed);
					player.rotation = Quaternion.Euler (new Vector3 (0f, player.rotation.eulerAngles.y, 0f));
				} else {
					//Als je richt, kijk je altijd naar voren
					//player.rotation = Quaternion.Slerp (player.rotation, forward, Time.deltaTime * rotationSpeed);
					Quaternion lookDir = Quaternion.LookRotation (playerCam.forward * 100f);
					player.rotation = Quaternion.Slerp (player.rotation, lookDir, Time.deltaTime * rotationSpeed);
					player.rotation = Quaternion.Euler (new Vector3 (0f, player.rotation.eulerAngles.y, 0f));
				}
			} else if (InputManager.moveX != 0 || InputManager.moveY != 0) {
				//Als je beweegt, kijk je naar de richting waarheen je gaat
				Quaternion lookRotation = Quaternion.LookRotation (movementDir) * forward;
				player.rotation = Quaternion.Slerp (player.rotation, lookRotation, Time.deltaTime * rotationSpeed);
			}

			//Loopsnelheden bij rennen en richten
			if (running == true) {
				moveSpeed = 30f;
			} else if (InputManager.aim.Hold == true) {
				moveSpeed = 10f;
			} else {
				moveSpeed = 15f;
			}

			//Springen
			if (InputManager.jump.Pressed == true && IsGrounded () == true) {
				player.GetComponent<Rigidbody> ().velocity += new Vector3 (0, jumpPower, 0);
			}
			break;
		case 2: // Bij rustige stukjes
			break;
		}
			
		movement = movementDir * moveSpeed;
		//Draai de bewegingen relatief naar de draaing van het personage
		movement = Quaternion.Euler (0, centerPoint.eulerAngles.y, 0) * movement;
		movement.y = player.GetComponent<Rigidbody> ().velocity.y + Physics.gravity.y * gravity * Time.deltaTime;
		//Beweeg het personage
		player.GetComponent<Rigidbody> ().velocity = movement;
		//Het center point volgt het personage
		centerPoint.position = new Vector3 (player.position.x, player.position.y + 2f, player.position.z);
	}
}
