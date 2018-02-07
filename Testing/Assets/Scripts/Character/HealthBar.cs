using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {
	private Image healthBarBar;
	private CanvasRenderer gameOverScreen;
	private GameObject ingameUI;
	private Breakable player;
	public float health;
	public float maxHealth;
	private float gameOverAlpha = 0f;

	void Start () {
		healthBarBar = GameObject.Find ("Health Bar/Bar").GetComponent<Image>();
		gameOverScreen = GameObject.Find ("Game Over Screen").GetComponent<CanvasRenderer> ();
		ingameUI = GameObject.Find ("Ingame UI");
		gameOverScreen.SetAlpha (0f);
		player = GameObject.Find ("Player").GetComponent<Breakable>();
		maxHealth = GameObject.Find ("Player").GetComponent<Breakable> ().data.health;
	}

	void Update () {
		health = player.health;
		healthBarBar.fillAmount = health / maxHealth;

		if (health <= 0f) {
			player.health = 0;
			ingameUI.SetActive (false);
			PlayerController.playerstate = 2;
			gameOverAlpha = Mathf.Clamp (gameOverAlpha + 0.3f * Time.deltaTime, 0f, 1f);
			Debug.Log (gameOverAlpha);
			gameOverScreen.SetAlpha (gameOverAlpha);
		}
	}
}
