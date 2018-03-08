using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {
	private Image healthBarBar;
	private CanvasRenderer gameOverScreen;
	private GameObject ingameUI, continueButton;
	private Breakable player;
	public float health;
	public float maxHealth;
	private float gameOverAlpha = 0f;

	void Awake () {
		healthBarBar = GameObject.Find ("Health Bar/Bar").GetComponent<Image>();
		gameOverScreen = GameObject.Find ("Game Over Screen").GetComponent<CanvasRenderer> ();
		ingameUI = GameObject.Find ("Ingame UI");
		continueButton = GameObject.Find ("Continue Button");
		gameOverScreen.SetAlpha (0f);
		player = GameObject.Find ("Player").GetComponent<Breakable>();
		maxHealth = GameObject.Find ("Player").GetComponent<Breakable> ().data.health;
		PlayerController.playerstate = 0;
	}

	void Start () {
		continueButton.SetActive (false);
	}

	void Update () {
		health = player.health;
		healthBarBar.fillAmount = health / maxHealth;

		if (health <= 0f) {
			player.health = 0;
			ingameUI.SetActive (false);
			PlayerController.playerstate = 2;
			gameOverAlpha = Mathf.Clamp (gameOverAlpha + 0.3f * Time.deltaTime, 0f, 1f);
			gameOverScreen.SetAlpha (gameOverAlpha);
			FindObjectOfType<PauseGame> ().enabled = false;
			if (gameOverAlpha == 1) {
				continueButton.SetActive (true);
			}
		}
	}
}
