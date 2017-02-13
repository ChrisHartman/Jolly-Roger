using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MortarTowerController : MonoBehaviour {

	public float FireCooldown = 3f;
	    
	private float coolDownTimer;

    public GameObject MortarProjectile;
	//public GameObject ShipController;

	// Use this for initialization
	void Start () {
		GetComponent<Health>().OnDeath += Die;
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

        var go = Instantiate(MortarProjectile) ;
        var ps = go.GetComponent<MortarProjectile>();
		var ship= GameObject.Find("Ship");
		ps.Init(gameObject, transform.position, ship.transform.position);
    }

    internal void Update(){

		FireProjectileIfPossible();
    }

    void Die() {
        Destroy(this.gameObject);
    }
}
