using UnityEngine;
using UnityEditor;

public class ItemDataAsset
{
	[MenuItem("Assets/Create/Item Data")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<ItemData> ();
	}
}
