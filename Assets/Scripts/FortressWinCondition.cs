using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortressWinCondition : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Health>().OnDeath+=Win;
	}
	
	void Win() {
		FindObjectOfType<GameManager>().Win();
	}
}
