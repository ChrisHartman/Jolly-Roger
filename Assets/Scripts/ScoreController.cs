using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {
	Text scoreText;
	int score = 0;
	// Use this for initialization
	void Start () {
		scoreText = GetComponent<Text>();
		Health[] healths = FindObjectsOfType<Health>();
		Debug.Log(healths.Length);
		foreach (Health health in FindObjectsOfType<Health>()) {
			if (health.GetComponent<ShipController>() == null &&
				health.GetComponent<IslandController>() == null) {
				health.OnDeath += IncrementEnemyCount;
			}
		}
	}
	void IncrementEnemyCount() {
		score++;
		scoreText.text = "Score: " + score;
	}
	
}
