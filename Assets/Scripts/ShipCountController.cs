using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShipCountController : MonoBehaviour {

	Text scoreText;
	int score;
	// Use this for initialization
	void Start () {
		scoreText = GetComponent<Text>();
		Health[] healths = FindObjectsOfType<Health>();
		score = 0;
		foreach (Health health in FindObjectsOfType<Health>()) {
			if (health.GetComponent<ShipController>() == null &&
				health.GetComponent<BasicAIShipController>() != null) {
				health.OnDeath += DecrementEnemyCount;
				score++;
			}
		}
		scoreText.text = "Ships Left: " + score;
	}
	void DecrementEnemyCount() {
		score--;
		scoreText.text = "Ships Left: " + score;
	}
	
}
