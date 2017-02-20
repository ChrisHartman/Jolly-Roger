using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandController : MonoBehaviour {

	

	// Use this for initialization
	void Start () {
		GetComponent<Health>().OnDeath += Die;
	}
	
	// Update is called once per frame
	void Update () {

		
	}

	void Die() {
		
		GetComponent<SpriteRenderer>().color= new Color32(60, 67, 119, 255);
    }
}
