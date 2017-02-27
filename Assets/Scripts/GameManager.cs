using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
	public float EndScreenTime = 1f;

	public event Action OnWin = delegate {};
	public event Action OnLose = delegate {};
	// Use this for initialization
	void Start () {
		FindObjectOfType<UpgradeManager>().ApplyUpgrades();
		FindObjectOfType<ShipController>().GetComponent<Health>().OnDeath += Lose;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void Win() {
		OnWin();
		// TODO: Add winning text of some sort
		Invoke("EndGame", EndScreenTime);
	}
	void Lose() {
		OnLose();
		// TODO Add losing text of some sort
		Invoke("EndGame", EndScreenTime);
	}
	void EndGame() {
		FindObjectOfType<LevelManager>().LoadLevel("Main Menu");
	}
}
