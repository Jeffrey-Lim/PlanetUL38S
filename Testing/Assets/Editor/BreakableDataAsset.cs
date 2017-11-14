using UnityEngine;
using UnityEditor;

public class BreakableDataAsset
{
	[MenuItem("Assets/Create/Breakable Data")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<BreakableData> ();
	}
}