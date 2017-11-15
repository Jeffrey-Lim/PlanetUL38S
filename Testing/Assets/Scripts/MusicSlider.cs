using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MusicSlider : MonoBehaviour {

	public Slider Volume;
	public AudioSource myMusic;
	
	// Update is called once per frame
	void Update () {
		myMusic.volume = Volume.value;
	}
}
