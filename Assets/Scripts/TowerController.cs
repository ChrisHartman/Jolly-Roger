using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour {

	public float FireCooldown = 3f;
	    
	private float coolDownTimer;

    public GameObject BasicProjectile;
	//public GameObject ShipController;

	// Use this for initialization
	void Start () {
		
	}

	void FireProjectileIfPossible(){

		var ship= GameObject.Find("Ship");

        float dist = Vector3.Distance(ship.transform.position,transform.position);
        if (Time.time > coolDownTimer && dist < 5f) {

            FireProjectile();
            coolDownTimer = Time.time + FireCooldown;
        }
    }

    /// <summary>
    /// Really and truly fire the projectile.
    /// </summary>
    void FireProjectile() {

        var go = Instantiate(BasicProjectile) ;
        var ps = go.GetComponent<BasicProjectile>();

		var ship= GameObject.Find("Ship");

        var up = ship.transform.position-transform.position; //Change direction
		ps.Init(gameObject, transform.position, up);
    }

    internal void Update(){

		FireProjectileIfPossible();
    }
}
