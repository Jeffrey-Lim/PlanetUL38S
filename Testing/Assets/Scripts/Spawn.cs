using UnityEngine;
using System.Collections;

public class Spawn : MonoBehaviour {

	public Transform meteoriet;

	// Use this for initialization
	void Start () {
		Instantiate (meteoriet, new Vector3 (100, Random.Range(1500f,4000f), 0), Quaternion.identity);
		StartCoroutine (Spawnmeteoriet());	
	
	}
	
	// Update is called once per frame
	void Update () {

		//Instantiate (meteoriet, new Vector3 (100, Random.Range(1500f,4000f), 0), Quaternion.identity);
	
	}

	IEnumerator Spawnmeteoriet(){
		while(true){
			Instantiate (meteoriet, new Vector3 (2000, Random.Range(1500f,4000f), Random.Range(-2000f,2000f)), Quaternion.identity);
			Instantiate (meteoriet, new Vector3 (2000, Random.Range(3000f,5000f), Random.Range(-2000f,2000f)), Quaternion.identity);
			Instantiate (meteoriet, new Vector3 (2000, Random.Range(1500f,4000f), Random.Range(-2000f,2000f)), Quaternion.identity);
			Instantiate (meteoriet, new Vector3 (2000, Random.Range(3000f,5000f), Random.Range(-2000f,2000f)), Quaternion.identity);
			Instantiate (meteoriet, new Vector3 (2000, Random.Range(1500f,4000f), Random.Range(-2000f,2000f)), Quaternion.identity);
			Instantiate (meteoriet, new Vector3 (2000, Random.Range(3000f,5000f), Random.Range(-2000f,2000f)), Quaternion.identity);
			Instantiate (meteoriet, new Vector3 (2000, Random.Range(1500f,4000f), Random.Range(-2000f,2000f)), Quaternion.identity);
			Instantiate (meteoriet, new Vector3 (2000, Random.Range(3000f,5000f), Random.Range(-2000f,2000f)), Quaternion.identity);

			yield return new WaitForSeconds(1);
		}
	}
}
