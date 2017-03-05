using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandController : MonoBehaviour {

	
	public float IslandHealth = 0;
	// Use this for initialization
	void Start () {
		GetComponent<Health>().OnDeath += Die;
		foreach (Health h in GetComponentsInChildren<Health>()) {
			if (!h.GetComponent<IslandController>()) {
				h.OnDeath += LoseTower;
				IslandHealth++;
			}
		}
		GetComponent<Health>().HealthOverride(IslandHealth);
	}

	void LoseTower() {
		Debug.Log("A tower down!");
		GetComponent<Health>().Damage(1);
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
			sr.color = new Color32(17, 106, 178, 255);
		}
	}
		//GetComponent<SpriteRenderer>().color = new Color32(60, 67, 119, 255);
    //}
}
