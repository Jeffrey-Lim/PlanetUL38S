using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {
	public bool inConversation = false;
	private Queue<string> names;
	private Queue<string> sentences;
	public static Dialogue current;
	private GameObject dialogueBox;
	private Text nameText;
	private Text speechText;
	private PauseGame pause;

	void Awake () {
		names = new Queue<string> ();
		sentences = new Queue<string> ();

		nameText = GameObject.Find ("Name Text").GetComponent<Text>();
		speechText = GameObject.Find ("Speech Text").GetComponent<Text>();
		dialogueBox = GameObject.Find ("DialogueBox");
		pause = FindObjectOfType<PauseGame> ();
	}

	void Start () {
		dialogueBox.SetActive (false);
	}

	void Update () {
		if (inConversation == true) {
			pause.Activate (dialogueBox);
		}
	}

	public void StartDialogue (DialogueData data, Dialogue dialogue) {
		names.Clear ();
		sentences.Clear ();
		inConversation = true;
		current = dialogue;
		pause.Pause(dialogueBox);

		foreach (DialoguePart part in data.dialogueParts) {
			names.Enqueue (part.name);
			sentences.Enqueue (part.speech);
		}

		DisplayNextPart ();
	}

	public void DisplayNextPart () {
		if (sentences.Count == 0) {
			EndDialogue (current);
			return;
		}
		string speecher = names.Dequeue ();
		string sentence = sentences.Dequeue ();

		nameText.text = speecher;
		StopAllCoroutines ();
		StartCoroutine (TypeSentence (sentence));
	}

	IEnumerator TypeSentence (string sentence) {
		speechText.text = "";
		foreach (char letter in sentence.ToCharArray()) {
			speechText.text += letter;
			yield return null;
		}
	}

	public void EndDialogue (Dialogue dialogue) {
		inConversation = false;
		dialogue.canTalk = 1f;
		pause.Pause (dialogueBox);
	}
}
