using UnityEngine;
using System.Collections;

public class Target : MonoBehaviour {
	public Transform objectToMove;
	public Transform toPosition;
	public float moveTime;
	private bool isTriggered = false;
	private Rigidbody rb;

	void Start () {
		//rb = objectToMove.GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		if (isTriggered == true) {
			objectToMove.position = Vector3.Slerp (objectToMove.position, toPosition.position, moveTime * Time.deltaTime);
			objectToMove.rotation = Quaternion.Slerp (objectToMove.rotation, toPosition.rotation, moveTime * Time.deltaTime);
		}
	}

	public void MoveObject() {
		isTriggered = true;
	}
}
