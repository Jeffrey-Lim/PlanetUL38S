using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class valscript : MonoBehaviour {

	public Transform centerPoint, player, detect;

	//Variabelen voor bewegen
	private float moveFB, moveLR, moveUD; //FB staat voor Forward Backward, LR voor Left Right, UD voor Up Down
	public float moveSpeed = 20f;
	public float rotationSpeed = 20f;
	public float jumpPower = 6f;
	public float gravity = 2f;
	Rigidbody rg;
	public float tilt;




	// Use this for initialization
	void Start () {
		//instructie.text = "hallo mensen";
		rg = this.gameObject.GetComponent<Rigidbody> ();
		//test = GetComponent<Text> ();
		//text = gameObject.GetComponent<Text> ();

		//Text tekst = tekst.GetComponent<Text> ();
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "Meteoriet met 'staart'(Clone)") {
			Debug.Log ("JAAA");
			Destroy (other.gameObject);

		}

		if (other.gameObject.name == "Sphere (7)") {
			Debug.Log ("Volgende");
			SceneManager.LoadScene(4);

		}

	}

	bool IsGrounded () {
		return Physics.Linecast (player.transform.position, player.transform.position - Vector3.up * 1.5f);
	}

	// Update is called once per frame
	void Update () {
		Vector3 movementDir = new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		Vector3 movement = movementDir * moveSpeed;

		Quaternion forward = Quaternion.Euler (0, centerPoint.eulerAngles.y, 0);

		if (Input.GetKey (KeyCode.Mouse1)) {
			//Draai geleidelijk naar de bewegingsrichting toe
			//player.rotation = Quaternion.Slerp (player.rotation, forward, Time.deltaTime * rotationSpeed);
		} else if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
			//Als je beweegt, kijk je naar de richting waarheen je gaat
			//Quaternion lookRotation = Quaternion.LookRotation (movementDir) * forward;
			//player.rotation = Quaternion.Slerp (player.rotation, lookRotation, Time.deltaTime * rotationSpeed);
			//transform.rotation = 
		}

		if (Input.GetButton ("Jump") == true && IsGrounded() == true) {
			//rg.velocity.y += 20;
			player.GetComponent<Rigidbody> ().velocity += new Vector3 (0, jumpPower, 0);
		}

		//Draai de bewegingen relatief naar de draaing van het personage
		//movement = Quaternion.Euler (0, centerPoint.eulerAngles.y, 0) * movement;
		movement.y = player.GetComponent<Rigidbody> ().velocity.y + Physics.gravity.y * gravity * Time.deltaTime;
		//Beweeg het personage
		player.GetComponent<Rigidbody> ().velocity = movement;
		//Het center point volgt het personage
		centerPoint.position = new Vector3 (player.position.x, player.position.y + 2f, player.position.z);

		float distancebetween = Vector3.Distance (player.position, detect.position);
		//Debug.Log (distancebetween);


	}
}
