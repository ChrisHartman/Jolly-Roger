using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtShipSprite : MonoBehaviour {

	// Use this for initialization
	public List<Sprite> ShipStages = new List<Sprite>();
	private int currentStage = 0; 

	 private Health health;
	void Start () {
		health = GetComponent<Health>();
		health.OnHit += CheckSpriteChange;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void CheckSpriteChange() {
		if (health.HealthRemaining < Mathf.Lerp(0, health.InitialHealth, ((float)ShipStages.Count-(float)currentStage-1)/(float)ShipStages.Count)
		&& health.HealthRemaining > 0) {
			SwitchSprite();
		}
	}

	void SwitchSprite() {
		currentStage++;
		GetComponent<SpriteRenderer>().sprite = ShipStages[currentStage]; 
	}
}
