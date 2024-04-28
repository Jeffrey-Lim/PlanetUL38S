using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour {
	public DialogueData data;
	public float canTalk = 0f;

	void Update () {
		if (canTalk != 0f) {
			canTalk -= Time.deltaTime;
		}
		Mathf.Clamp (canTalk, 0f, 10f);
	}
}
