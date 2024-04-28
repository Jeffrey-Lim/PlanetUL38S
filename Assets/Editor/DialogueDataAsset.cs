using UnityEngine;
using UnityEditor;

public class DialogueDataAsset
{
	[MenuItem("Assets/Create/Dialogue Data")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<DialogueData> ();
	}
}
