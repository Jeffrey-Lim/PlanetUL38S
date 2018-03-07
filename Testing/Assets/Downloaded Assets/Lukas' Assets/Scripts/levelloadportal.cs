using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class levelloadportal : MonoBehaviour {


	void Start(){
		print ("loadportal script started");
	}


	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag ("loadportal")) 
		{
			SceneManager.LoadScene("loadingscreen");
			
			print ("yup");



		}

	}

}
