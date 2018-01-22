using UnityEngine;
using System.Collections;

[System.Serializable]
public class DialoguePart {
	public string name;
	[TextArea(1, 10)]
	public string speech;
}
	
public class DialogueData : ScriptableObject {
	public DialoguePart[] dialogueParts;
}
