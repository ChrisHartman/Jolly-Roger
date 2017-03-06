using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BasicAIShipController : MonoBehaviour {
	public float ForwardForce = 30f;
    public float MaxSpeed = 1.2f;
    public float TurnTorque = 1f;
    public float Smoothing = 5f;
	public float WaypointThreshold = 3f;
	public float FireCooldown = 5f;
	public float FireDistance = 3f;
	public float SideFireTime = 1f;
	public float FrontFireTime = 1.5f;
	public float pathUpdateDelay = 3f;
	public float chaseStartThreshold = 6f;
	public float chaseAbandonThreshold = 20f;
	public float TargetDistance = 4f;
	public float pscale = 3f;

	public float Gold = 5;
    public float Fabric = 5;
    public float Metal = 5;
    public float Wood = 5;
	private float pathUpdateTime;
	private bool chasing;
	public Waypoint[] path;
	private GameObject ship;
	public GameObject BasicProjectile;
	public int CurrentWaypoint;
    private Rigidbody2D boatRb;
	private float coolDownTimer;
    private CameraController cameraController;
	public List<Waypoint> currentPath;
	public enum State{patrolling, chasing, pathing}
	public State state;

	// Use this for initialization
	void Start () {
		pathUpdateTime = 0;
		currentPath = new List<Waypoint>(path);
		ship = GameObject.Find("Ship");
		CurrentWaypoint = 0;
		GetComponent<Health>().OnDeath += Die;
		boatRb = GetComponent<Rigidbody2D>();
		state = State.patrolling;
	}

	/* 
	This feels way too simple to add a behavior tree or anything,
    So we're gonna use a good old switch statement and state variable
	to kinda encode a finite state machine and hope it's close to right
	*/
	void StateUpdate () {
		bool shipVisible = (!WaypointBlocked(ship.transform));
		bool shipNearby = (Vector2.Distance(this.transform.position, ship.transform.position) < chaseStartThreshold);
		bool shipFar = (Vector2.Distance(this.transform.position, ship.transform.position) > chaseAbandonThreshold);
		switch (state) {
			case State.patrolling:
				PatrollingUpdate();
				if (shipVisible && shipNearby) {
					ChasingInit();
				}
				break;
			case State.chasing:
				ChasingUpdate();
				if (!shipVisible) {
					PathingInit();
				} else if (shipFar) {
					PatrollingInit();
				}
				break;
			case State.pathing:
				PathingUpdate();
				if (shipFar) {
					PatrollingInit();
				} else if (shipVisible) {
					ChasingInit();
				}
				break;
		}
	}

	void PatrollingUpdate() {
		if (!FollowPathIfPossible()) {
			FindWaypoint();
		}
	}

	void PatrollingInit() {
		//Debug.Log("switching to patrolling");
		state = State.patrolling;
		currentPath = new List<Waypoint>(path);
		CurrentWaypoint = 0;
	}

	void ChasingUpdate() {
		if (Vector2.Distance(transform.position, ship.transform.position) > TargetDistance) {
			MoveToward(ship.transform);
		} else {
			GetInAttackPosition(ship.transform);
		}
		FireProjectileIfPossible();
	}
	void ChasingInit() {
		////Debug.Log("switching to chasing");
		state = State.chasing;
	}

	void PathingInit() {
		////Debug.Log("switching to pathing");
		currentPath = Waypoint.FindPath(this.transform.position, ship.transform.position);
		if (currentPath == null) {
			PatrollingInit();
		} else {
			CurrentWaypoint = 0;
			state = State.pathing;
		}
	}

	void PathingUpdate() {
		if (!FollowPathIfPossible()) {
			PathingInit();
		}

	}
	void Update () {
		var s_right = Vector2.Dot(boatRb.velocity, transform.right);
        boatRb.AddForce(-1f * s_right * transform.right);
		StateUpdate();
	}

	bool FollowPathIfPossible() {
		if (Vector2.Distance(transform.position, currentPath[CurrentWaypoint].transform.position) < WaypointThreshold) 
		{
			CurrentWaypoint++;
			if (CurrentWaypoint >= currentPath.Count) {
				//Debug.Log("out of path");
				return false;
			}
		}
		if (!WaypointBlocked(currentPath[CurrentWaypoint].transform)) {
			MoveToward(currentPath[CurrentWaypoint].transform); 
		} else {
			//Debug.Log("path blocked");
			return false;
		}
		return true;
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
		GetComponent<AudioSource>().Play();
		ps.Init(transform.position, direction, new Vector3(pscale, pscale, 1), airTime);
    }

	bool WaypointBlocked(Transform waypoint) {
		// Debug.Log(Waypoint.WaypointBlocked(waypoint.position, this.transform.position));
		return Waypoint.WaypointBlocked(waypoint.position, this.transform.position);
	} 

	void MoveToward(Transform target) {
		TurnToward(target);
		if (Vector2.Angle(transform.up, target.position - transform.position) < 30f) {
			boatRb.AddForce(transform.up*ForwardForce*Time.deltaTime);
		}
	}
	void TurnToward(Transform target) {
		if (Vector2.Angle(transform.up, target.position - transform.position) > 10f) {
			if (Vector3.Cross(transform.up, target.position - transform.position).z > 0) {
				boatRb.AddTorque(TurnTorque*Time.deltaTime);
			} else {
				boatRb.AddTorque(-TurnTorque*Time.deltaTime);
			} 
		}
	}
	void GetInAttackPosition(Transform target) {
		if (!(Physics2D.Raycast(transform.position, transform.up, 2f, LayerMask.GetMask("Land","Player")))) {
			boatRb.AddForce(transform.up*ForwardForce*Time.deltaTime);
		}
		float angle = Vector2.Angle(transform.up, target.position - transform.position);
		if (angle < 10f || (angle > 80f && angle < 100f)) {
			return;
		}
		if (angle < 45f || angle > 90f) {
			if (Vector3.Cross(transform.up, target.position - transform.position).z > 0) {
				boatRb.AddTorque(TurnTorque*Time.deltaTime);
			} else {
				boatRb.AddTorque(-TurnTorque*Time.deltaTime);
			} 
		} else {
			if (Vector3.Cross(transform.up, target.position - transform.position).z > 0) {
				boatRb.AddTorque(-TurnTorque*Time.deltaTime);
			} else {
				boatRb.AddTorque(TurnTorque*Time.deltaTime);
			} 
		}
	}

	Waypoint FindWaypoint() {
		for (int i = 0; i < path.Length; i++) {
			if (!WaypointBlocked(currentPath[i].transform)) {
				CurrentWaypoint = i;
				return currentPath[i];
			}
		}
		return currentPath[0];
	}
	
	void Die() {
    //Debug.Log("Dying!");
    	Destroy(this.gameObject);
    }
}
