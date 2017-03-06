using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour {
	public float EndScreenTime = 1f;
	public Text Instructions;
	private Color InstructionsColor;
	public float fadeTime = 2f;
	private bool fadeIn = false;
	private float startTime = 0;
	public event Action OnWin = delegate {};
	public event Action OnLose = delegate {};
	// Use this for initialization
	void Start () {
		InstructionsColor = Instructions.color;
		FindObjectOfType<UpgradeManager>().ApplyUpgrades();
		FindObjectOfType<ShipController>().GetComponent<Health>().OnDeath += Lose;
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeIn) {
			InstructionsColor.a = Mathf.Lerp(0,1, (Time.time - startTime) / fadeTime);
			Instructions.color = InstructionsColor;
		}
	}
	public void Win() {
		OnWin();
		// TODO: Add winning text of some sort
		fadeIn = true;
		Instructions.text = "Victory!";
		startTime = Time.time;
		
		Invoke("EndGame", EndScreenTime);
	}
	void Lose() {
		OnLose();
		fadeIn = true;
		startTime = Time.time;
		Instructions.text = "Game Over";
		// TODO Add losing text of some sort
		Invoke("EndGame", EndScreenTime);
	}
	void EndGame() {
		FindObjectOfType<LevelManager>().LoadLevel("Main Menu");
	}
}
