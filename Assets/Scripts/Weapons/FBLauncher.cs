using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FBLauncher : AreaWeapon {

    public GameObject fireBarrel;

    public override void Activate()
    {
        var f = Instantiate(fireBarrel, transform.position, Quaternion.identity);
        f.GetComponent<FireBarrel>().damage = power; 
    }
}
