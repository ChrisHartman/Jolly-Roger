using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamController : MonoBehaviour {
	public float damage = 5;
	public float damageSpeed = 1f; 

	// Use this for initialization
	void Start () {
		
	}

	/// <summary>
	/// Sent when an incoming collider makes contact with this object's
	/// collider (2D physics only).
	/// </summary>
	/// <param name="other">The Collision2D data associated with this collision.</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.GetComponent<Health>() != null 
			&& other.relativeVelocity.magnitude >= damageSpeed
			&& other.gameObject.tag != "Island"
			) 
		{
			other.gameObject.GetComponent<Health>().Damage(damage);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
