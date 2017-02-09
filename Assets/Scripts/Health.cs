using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Health : MonoBehaviour {

	public float InitialHealth;

	public event Action OnHit = delegate {};
	public event Action OnDeath = delegate {};

	
	private float HealthRemaining;


	// Use this for initialization
	void Start () {
		HealthRemaining = InitialHealth;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Damage(float damage) {
		
		HealthRemaining -= damage;
		Debug.Log("Ouch, health now at " + HealthRemaining);
		OnHit();
		if (HealthRemaining <= 0) {
			OnDeath();
		}
	}
}
