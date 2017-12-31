using UnityEngine;
using UnityEditor;

public class ExplosiveDataAsset
{
	[MenuItem("Assets/Create/Explosive Data")]
	public static void CreateAsset ()
	{
		ScriptableObjectUtility.CreateAsset<ExplosiveData> ();
	}
}
