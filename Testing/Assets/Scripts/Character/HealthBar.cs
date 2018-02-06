using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {
	private Image healthBarBar;
	private CanvasRenderer gameOverScreen;
	private GameObject healthBar;
	private Breakable player;
	public float health;
	public float maxHealth;
	private float gameOverAlpha = 0f;

	void Start () {
		healthBarBar = GameObject.Find ("Health Bar/Bar").GetComponent<Image>();
		healthBar = GameObject.Find ("Health Bar");
		gameOverScreen = GameObject.Find ("Game Over Screen").GetComponent<CanvasRenderer> ();
		gameOverScreen.SetAlpha (0f);
		player = GameObject.Find ("Player").GetComponent<Breakable>();
		maxHealth = GameObject.Find ("Player").GetComponent<Breakable> ().data.health;
	}

	void Update () {
		health = player.health;
		healthBarBar.fillAmount = health / maxHealth;

		if (health <= 0f) {
			player.health = 0;
			healthBar.SetActive (false);
			PlayerController.playerstate = 2;
			gameOverAlpha = Mathf.Clamp (gameOverAlpha + 0.3f * Time.deltaTime, 0f, 1f);
			Debug.Log (gameOverAlpha);
			gameOverScreen.SetAlpha (gameOverAlpha);
		}
	}
}
