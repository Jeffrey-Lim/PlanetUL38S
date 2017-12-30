using UnityEngine;
using UnityEditor;

public class AmmoDataAsset
{
	[MenuItem("Assets/Create/Ammo Data")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<AmmoData> ();
	}
}
