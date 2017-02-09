using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaWeapon : MonoBehaviour {
    // Targets an area, on activation hits everything in the area
    // a collider defines the area the attack hits in 
    // NOTE: Currently, the scale and position of the attack GameObject (not collider) defines where the attack hits relative to the boat. 

    // a list containing all applicable objects in the collider
    List<GameObject> targetedObj = new List<GameObject>();

    public float power;

    virtual internal void Start() { }
    virtual internal void Update () { }
    
    // add objects to the targeted list when they enter the collider 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!targetedObj.Contains(other.gameObject))
        {
            // TODO: Add a way of making sure the object is targetable
            // best way is probably to check if it has a health component (when those exist)
            targetedObj.Add(other.gameObject);
            Debug.Log(other.gameObject.name + " added to target list!"); 
        }
    }

    // remove objects from the targeted list when they exit the collider
    void OnTriggerExit2D(Collider2D other)
    {
        if (targetedObj.Contains(other.gameObject))
        {
            targetedObj.Remove(other.gameObject);
            Debug.Log(other.gameObject.name + " removed from target list."); 
        }
    }

    // Have the weapon fire on all currently targeted game objects
    public void Activate()
    {
        foreach(GameObject g in targetedObj)
        {
            this.Attack(g); 
        }

        // now that our attack has been carried out, destroy this object 
        Destroy(this.gameObject); 
    }

    public void Disable() {
        Destroy(this.gameObject);
    }

    // Code that defines how an attack is carried out 
    private void Attack(GameObject other)
    {
        // TODO: decide how we want to do this 
        Debug.Log("Attacking " + other.name + " with " + this.gameObject.name); 
        if (other.GetComponent<Health>() != null) {
            other.GetComponent<Health>().Damage(power);
        }
    }

    
}
