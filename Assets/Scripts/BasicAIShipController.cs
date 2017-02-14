using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicAIShipController : MonoBehaviour {
	public float ForwardForce = 30f;
    public float MaxSpeed = 1.2f;
	

    public float TurnTorque = 1f;
    public float Smoothing = 5f;
	public float WaypointThreshold = 1f;
	public float FireCooldown = 5f;
	public float FireDistance = 3f;
	public float SideFireTime = 1f;
	public float FrontFireTime = 1.5f;
	public Transform[] path;
	public GameObject ship;
	public GameObject BasicProjectile;
	public int CurrentWaypoint;
    private Rigidbody2D boatRb;
	private float coolDownTimer;
    private CameraController cameraController;

	// Use this for initialization
	void Start () {
		CurrentWaypoint = 0;
		GetComponent<Health>().OnDeath += Die;
		boatRb = GetComponent<Rigidbody2D>();
	}

	void Update () {
		FireProjectileIfPossible();
	}

	void FixedUpdate () {
		var s_right = Vector2.Dot(boatRb.velocity, transform.right);
        boatRb.AddForce(-1f * s_right * transform.right);
		if (Vector2.Distance(transform.position, path[CurrentWaypoint].position) < WaypointThreshold) 
		{
			CurrentWaypoint++;
			if (CurrentWaypoint == path.Length) {
				CurrentWaypoint = 0;
			}
		}
		if (!WaypointBlocked(path[CurrentWaypoint])) {
			MoveToward(path[CurrentWaypoint]); 
		} else {
			MoveToward(FindWaypoint());
		}
	}

	void FireProjectileIfPossible() {
		if (ship == null) {
            return;
        }
        if (Time.time > coolDownTimer)
		{
			if (Physics2D.Raycast(transform.position, transform.up, FireDistance, 1<<9))
			{
            	FireProjectile(transform.up,FrontFireTime);
        	} else if (Physics2D.Raycast(transform.position, transform.right, FireDistance, 1<<9)) {
				FireProjectile(transform.right, SideFireTime);
			} else if (Physics2D.Raycast(transform.position, -transform.right, FireDistance, 1<<9)) {
				FireProjectile(-transform.right, SideFireTime);
			}
		}
	}
	void FireProjectile(Vector3 direction, float airTime) {
		coolDownTimer = Time.time + FireCooldown;
        var go = Instantiate(BasicProjectile) ;
        var ps = go.GetComponent<ShipProjectile>();
		ps.Init(gameObject, transform.position, direction, airTime);
    }

	bool WaypointBlocked(Transform waypoint) {
		// Debug.Log(Physics2D.Linecast(this.transform.position, waypoint.position, 1<<8).point);
		return Physics2D.Linecast(this.transform.position, waypoint.position, 1<<10);
	} 

	void MoveToward(Transform target) {
		if (Vector2.Angle(transform.up, target.position - transform.position) > 10f) {
			if (Vector3.Cross(transform.up, target.position - transform.position).z > 0) {
				boatRb.AddTorque(TurnTorque*Time.fixedDeltaTime);
			} else {
				boatRb.AddTorque(-TurnTorque*Time.fixedDeltaTime);
			}
		} else {
			boatRb.AddForce(transform.up*ForwardForce*Time.fixedDeltaTime);
		}
	}

	Transform FindWaypoint() {
		for (int i = 0; i < path.Length; i++) {
			if (!WaypointBlocked(path[i])) {
				CurrentWaypoint = i;
				return path[i];
			}
		}
		return this.transform;
	}
	
	void Die() {
        Debug.Log("Dying!");
        Destroy(this.gameObject);
    }
}
