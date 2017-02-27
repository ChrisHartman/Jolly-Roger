using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionWinCondition : MonoBehaviour {

	// Use this for initialization
	public float EnemyCount;
	void Start () {
		EnemyCount = 0;
		Health[] healths = FindObjectsOfType<Health>();
		Debug.Log(healths.Length);
		foreach (Health health in FindObjectsOfType<Health>()) {
			if (health.GetComponent<ShipController>() == null) {
				EnemyCount++;
				health.OnDeath += DecrementEnemyCount;
			}
		}
	}
	
	void DecrementEnemyCount() {
		EnemyCount--;
		if (EnemyCount <= 0) {
			FindObjectOfType<GameManager>().Win();
		}
	}
}
