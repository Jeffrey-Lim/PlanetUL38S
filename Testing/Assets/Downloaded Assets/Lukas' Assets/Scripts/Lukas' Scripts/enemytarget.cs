using UnityEngine;
using System.Collections;

public class enemytarget : MonoBehaviour {
	public GameObject playerinstance;
	public Transform InstanceTransform;

	public GameObject[] all;

	// Use this for initialization
	void Start () {
		all = GameObject.FindGameObjectsWithTag("enemy");
		foreach (GameObject TheEnemy in all)
		{
			TheEnemy.GetComponent<EnemyScript>().playertransform = playerinstance.transform;
		}
	}

	// Update is called once per frame
	void Update () {
		InstanceTransform = playerinstance.transform;
		if(all.Length>0){
			foreach (GameObject TheEnemy in all)
			{
				TheEnemy.GetComponent<EnemyScript>().playertransform = InstanceTransform;
			}
		}
	}
}
