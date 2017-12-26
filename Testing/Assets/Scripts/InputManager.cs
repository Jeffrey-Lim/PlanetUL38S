using UnityEngine;
using System.Collections;

//Dit script heb ik gemaakt zodat alle input in één script wordt bewaard. Hierdoor blijft de input script in andere scripts kort

// Constructor om makkelijk knoppen te maken
public class Button {
	public bool Hold { get; set;}
	public bool Pressed { get; set;}
	public bool Release { get; set;}
	public Button (string keyname) {
		Hold = Input.GetButton (keyname); //True wanneer je de knop ingedrukt houdt
		Pressed = Input.GetButtonDown (keyname); //Alleen true op de frame dat je het indrukt
		Release = Input.GetButtonUp (keyname); //Alleen true op de frame dat je loslaat
	}
}

public class InputManager : MonoBehaviour {
	public static float moveX = 0f, moveY = 0f, camX = 0f, camY = 0f;
	public static float lStickSensitivity = 10f, rStickSensitivity = 10f, mouseSensitivity = 10f, scrollSensitivity = 10f;
	public static int controlMode = 0, cameraMode = 0; 
	// controlmode: Muis en Toetsenbord of Controller
	//cameraMode: 0 = normaal, 1 = richten, 2 = z-targetting
	public static bool running = false;
	public static Button jump, aim, fire, reload, melee, run;
	public static float moveXraw, moveYraw;
	public static Vector3 movementDir;
	public static int currentWeapon = 3;

	void Update () {
		if (controlMode == 0) { // Muis en Toetsenbord
			aim = new Button ("Aim");
			fire = new Button ("Fire");
			reload = new Button ("Reload");
			melee = new Button ("Melee");
			run = new Button ("Run");
			jump = new Button ("Jump");

			camX = Input.GetAxis("Mouse X") * mouseSensitivity;
			camY = -Input.GetAxis ("Mouse Y") * mouseSensitivity;

			moveX = Input.GetAxis ("Move L") + Input.GetAxis ("Move R");
			moveY = Input.GetAxis ("Move U") + Input.GetAxis ("Move D");
			movementDir = new Vector3 (moveX, 0, moveY);
			moveXraw = Input.GetAxisRaw ("Move L") + Input.GetAxisRaw ("Move R");
			moveYraw = Input.GetAxisRaw ("Move U") + Input.GetAxisRaw ("Move D");

			//Verander van wapen
			if (Input.GetButtonDown ("W1")) {
				currentWeapon = 1; //Machette
			} else if (Input.GetButtonDown ("W2")) {
				currentWeapon = 2; //Pijl en Boog
			} else if (Input.GetButtonDown ("W3")) {
				currentWeapon = 3; //10mm Pistool
			} else if (Input.GetButtonDown ("W4")) {
				currentWeapon = 4; //Revolver
			}

			if (aim.Hold == true) {
				if (currentWeapon == 1) {
					cameraMode = 2;
				} else {
					cameraMode = 1;
				}
			} else {
				cameraMode = 0;
			}

			//Script voor het rennen
			//Je moet kunnen rennen door maar even linkershift in te drukken en je rent niet meer door te stoppen met lopen
			if (moveXraw != 0 || moveYraw != 0) {
				if (run.Hold == true) {
					running = true;
				}
			} else {
				running = false;
			}
			if (cameraMode == 1) {
				running = false;
			}

		} else if (controlMode == 1) { //Controller

		}
	}
}