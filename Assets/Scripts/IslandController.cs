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

	/// <summary>
	/// Sent when an incoming collider makes contact with this object's
	/// collider (2D physics only).
	/// </summary>
	/// <param name="other">The Collision2D data associated with this collision.</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log(other.gameObject.name);
		if (other.gameObject.GetComponent<ShipController>()) {
			other.gameObject.GetComponent<ShipController>().Crash(other);
		}
	}

	void Die() {
		
		foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>()) {
			sr.color = new Color32(60, 67, 119, 255);
		}
	}
		//GetComponent<SpriteRenderer>().color = new Color32(60, 67, 119, 255);
    //}
}
