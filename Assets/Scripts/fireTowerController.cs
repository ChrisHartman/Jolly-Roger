using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fireTowerController : MonoBehaviour {

    public GameObject fireProjectile;
	public float FireCooldown = .2f;
    public float FireDistance = 1.5f;

	private float coolDownTimer;
    private GameObject ship;

	// Use this for initialization
	void Start () {
        ship= GameObject.Find("Ship");
		GetComponent<Health>().OnDeath += Die;
	}

	void FireProjectileIfPossible(){
        float dist = Vector3.Distance(ship.transform.position,transform.position);
        if (Time.time > coolDownTimer && dist < 4f) {
            FireProjectile();
			coolDownTimer = Time.time + FireCooldown;
        }
    }

    /// <summary>
    /// Really and truly fire the projectile.
    /// </summary>
    void FireProjectile() {

        var go = Instantiate(fireProjectile) ;
        var ps = go.GetComponent<fireProjectile>();
		var ship= GameObject.Find("Ship");
		ps.Init(gameObject, transform.position, ship.transform.position, FireDistance);
    }

    internal void Update(){

		FireProjectileIfPossible();
    }

    void Die() {
        Destroy(this.gameObject);
    }
}
