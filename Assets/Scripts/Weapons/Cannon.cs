using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : AreaWeapon {

    // GameObject of the cannonball that this weapon fires 
    public GameObject cannonball;

    public override void Activate()
    {
        var c = Instantiate(cannonball);
        // instead of using the attack method, this weapon instantiates a cannonball 
        GameObject ship = GameObject.Find("Ship");
        c.GetComponent<PlayerShipProjectile>().Init(ship.transform.position, 
            transform.position - ship.transform.position, 2.5f);
        c.GetComponent<PlayerShipProjectile>().damage = power; 
    }
}
