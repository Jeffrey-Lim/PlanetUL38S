using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	private Transform centerPoint, player, playerCam;
	private float moveFB, moveLR, moveUD; //FB staat voor Forward Backward, LR voor Left Right, UD voor Up Down
	public float moveSpeed = 20f;
	public float rotationSpeed = 20f;
	public float jumpPower = 30f;
	public float gravity = 10f;
	public static int playerstate = 0, cameraMode; /* 
	 * 0 = Normaal. De speler staat in het midden van je zicht, je kan alleen schieten als je richt.
	 * 1 = Bij rustige stukjes. De camera staat dichterbij de speler, je kan niet aanvallen en je loopt rustiger.
	 * 2 = Als ragdoll
	 * 3 = In Cutscenes
	 */
	public static bool running = false;
	Vector3 movementDir, movement;
	Ray aimRay;
	RaycastHit aimHit;
	Animator anim;

	void Start() {
		centerPoint = GameObject.Find ("Center Point").transform;
		player = GameObject.Find ("Player").transform;
		anim = player.GetComponent<Animator> ();
		playerCam = GameObject.Find ("Player Camera").transform;
	}

	//Pauper functie die checkt of je op de grond staat met een linecast
	bool IsGrounded () {
		return Physics.Linecast (player.transform.position + Vector3.up * 2f, player.transform.position - Vector3.up * 2f);
	}

	void Update () {
		movementDir = InputManager.movementDir;
		running = InputManager.running;
		cameraMode = InputManager.cameraMode;
		Quaternion forward = Quaternion.Euler (0, centerPoint.eulerAngles.y, 0);

		switch (playerstate) {
		case 0: //Normaal
			if (cameraMode == 1) {
				aimRay.origin = playerCam.position;
				aimRay.direction = playerCam.forward;
				if (Physics.Raycast (aimRay, out aimHit, 100f)) {
					Quaternion lookDir = Quaternion.LookRotation (aimHit.point - player.position);
					player.rotation = Quaternion.Slerp (player.rotation, lookDir, Time.deltaTime * rotationSpeed);
					player.rotation = Quaternion.Euler (new Vector3 (0f, player.rotation.eulerAngles.y, 0f));
				} else {
					//Als je richt, kijk je altijd naar voren
					//player.rotation = Quaternion.Slerp (player.rotation, forward, Time.deltaTime * rotationSpeed);
					player.LookAt (playerCam.position + playerCam.forward * 100f);
					player.rotation = Quaternion.Euler (new Vector3 (0f, player.rotation.eulerAngles.y, 0f));
				}
			} else if (cameraMode == 2) {
				if (running == true) {
					//Als je rent terwijl je target, kijk je naar de richting waarheen je gaat
					Quaternion lookRotation = Quaternion.LookRotation (movementDir) * forward;
					player.rotation = Quaternion.Slerp (player.rotation, lookRotation, Time.deltaTime * rotationSpeed);
				} else {
					if (CameraController.target != null) {
						player.LookAt (CameraController.target.position);
						player.rotation = Quaternion.Euler (new Vector3 (0f, player.rotation.eulerAngles.y, 0f));
					} else {
						
					}
				}
			} else if (InputManager.moveX != 0 || InputManager.moveY != 0 && cameraMode == 0) {
				//Als je beweegt, kijk je naar de richting waarheen je gaat
				Quaternion lookRotation = Quaternion.LookRotation (movementDir) * forward;
				player.rotation = Quaternion.Slerp (player.rotation, lookRotation, Time.deltaTime * rotationSpeed);
			}

			//Loopsnelheden bij rennen en richten
			if (running == true) {
				moveSpeed = 30f;
				anim.SetInteger ("Movement Type", 2);
			} else if (cameraMode == 1) {
				moveSpeed = 10f;
				anim.SetInteger ("Movement Type", 3);
			} else {
				moveSpeed = 15f;
				anim.SetInteger ("Movement Type", 1);
			}

			anim.SetBool ("Air Time", !IsGrounded ());

			//Springen
			if (InputManager.jump.Pressed == true && IsGrounded () == true) {
				player.GetComponent<Rigidbody> ().velocity += new Vector3 (0, jumpPower, 0);
				anim.SetTrigger ("Jump");
			}
			break;
		case 1: // Bij rustige stukjes
			if (InputManager.moveX != 0 || InputManager.moveY != 0 && cameraMode == 0) {
				//Als je beweegt, kijk je naar de richting waarheen je gaat
				Quaternion lookRotation = Quaternion.LookRotation (movementDir) * forward;
				player.rotation = Quaternion.Slerp (player.rotation, lookRotation, Time.deltaTime * rotationSpeed);
			}
			break;
		case 2: //Dialoog of ragdoll: geen besturing
			movementDir = Vector3.zero;
			break;
		}
		if (movementDir == Vector3.zero) {
			anim.SetInteger ("Movement Type", 0);
		}
		movement = movementDir * moveSpeed;
		//Draai de bewegingen relatief naar de draaing van het personage
		movement = Quaternion.Euler (0, centerPoint.eulerAngles.y, 0) * movement;
		movement.y = player.GetComponent<Rigidbody> ().velocity.y + Physics.gravity.y * gravity * Time.deltaTime;
		//Beweeg het personage
		player.GetComponent<Rigidbody> ().velocity = movement;
	}
}
