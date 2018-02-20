using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Cutscene : MonoBehaviour {
	public MovieTexture movie;
	private RawImage image;
	private AudioSource source;
	private bool hasStarted = false;

	void Start () {
		image = this.GetComponent<RawImage> ();
		source = this.GetComponent<AudioSource> ();
		image.gameObject.SetActive (false);
	}

	public void PlayCutscene (MovieTexture cutscene) {
		movie = cutscene;
		if (movie != null) {
			Time.timeScale = 0;
			image.texture = movie;
			movie.Play ();
			source.clip = movie.audioClip;
			source.Play ();
			hasStarted = true;
		} else {
			this.gameObject.SetActive (false);
		}
	}

	void LateUpdate () {
		if (hasStarted == true && movie.isPlaying == true && InputManager.reload.Pressed == true) {
			movie.Stop ();
		}

		if (hasStarted == true && movie.isPlaying == false) {
			this.gameObject.SetActive (false);
			Time.timeScale = 1;
		}
	}
}
