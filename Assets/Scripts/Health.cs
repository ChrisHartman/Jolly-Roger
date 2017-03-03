﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public float InitialHealth;
	public Slider healthBarSlider;

	public event Action OnHit = delegate {};
	public event Action OnDeath = delegate {};

	
	public float HealthRemaining;


	// Use this for initialization
	void Start () {
		HealthRemaining = InitialHealth;
		if (healthBarSlider != null) {
			healthBarSlider.value = 1f;
		}
	}

	public void HealthOverride(float h) {
		InitialHealth = h;
		HealthRemaining = h;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Damage(float damage) {
		HealthRemaining -= damage;
		Debug.Log("Ouch, health now at " + HealthRemaining);
		if (healthBarSlider != null) { // this might be a textbook example of how not to do this...
			healthBarSlider.value = Mathf.Lerp(0f, 1f, HealthRemaining/InitialHealth);
		}
		OnHit();
		if (HealthRemaining <= 0) {
			// Debug.Log("oh no!");
			OnDeath();
		}
	}
}
