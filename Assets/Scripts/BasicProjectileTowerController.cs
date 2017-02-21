using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectileTowerController : MonoBehaviour {

	public float FireCooldown = 3f;
	    
	public float coolDownTimer = 0;

    public GameObject BasicProjectile;
	private GameObject ship;
    public GameObject islandGO;

	// Use this for initialization
	void Start () {
        ship = FindObjectOfType<ShipController>().gameObject;
		GetComponent<Health>().OnDeath += Die;
	}

	void FireProjectileIfPossible(){

		if (ship == null) {
            return;
        }
        float dist = Vector3.Distance(ship.transform.position,transform.position);
        if (Time.time > coolDownTimer && dist < 5f) {

            FireProjectile();
            //Debug.Log(FireCooldown);
            coolDownTimer = Time.time + FireCooldown;
        }
    }

    /// <summary>
    /// Really and truly fire the projectile.
    /// </summary>
    void FireProjectile() {

        var go = Instantiate(BasicProjectile) ;
        var ps = go.GetComponent<BasicProjectile>();
		ps.Init(gameObject, transform.position, ship.transform.position);
    }

    internal void Update(){
        
		FireProjectileIfPossible();
    }

    void Die() {
        GameObject.Find("Ship").GetComponent<ShipController>().giveGold(5);
		GameObject.Find("Ship").GetComponent<ShipController>().giveMetal(5);
        islandGO.GetComponent<Health>().Damage(1);
        Destroy(this.gameObject);
    }
}
