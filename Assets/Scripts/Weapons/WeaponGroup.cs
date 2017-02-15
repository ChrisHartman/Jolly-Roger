using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGroup : MonoBehaviour {

    /// <summary>
    /// Contains multiple weapons that are intended to fire simultaneously
    /// </summary>

    public void Activate()
    {
        // get all children
        foreach (Transform weapon in transform)
        {
            // have each child activate individually
            weapon.GetComponent<AreaWeapon>().Activate();

            // now destroy the weapon group
            Destroy(this.gameObject); 
        }
    }
}
