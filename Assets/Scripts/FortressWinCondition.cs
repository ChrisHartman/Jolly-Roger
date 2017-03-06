using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FortressWinCondition : MonoBehaviour {

	// Use this for initialization
	public Text Instructions;
	public float InstructionsFadeTime = .5f;
	public float InstructionSolidTime = 1f;
	private float StartTime;
	private Color InstructionColor;

	void Start () {
		Instructions.text = "Destroy The Fortress!";
		InstructionColor = Instructions.color;
		StartTime = Time.time + InstructionSolidTime;
		GetComponent<Health>().OnDeath+=Win;
	}
	
	void Win() {
		FindObjectOfType<GameManager>().Win();
	}
	void Update() {
		if ((Time.time - StartTime) / InstructionsFadeTime < 2 ) {
			InstructionColor.a = Mathf.Lerp(1,0, (Time.time - StartTime) / InstructionsFadeTime);
			Instructions.color = InstructionColor;
		}
	}
}

