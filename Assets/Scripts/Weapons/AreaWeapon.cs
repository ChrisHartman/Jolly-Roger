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

    // colors to show whether the weapon is aiming at anything 
    public Color baseCol;
    public Color targetingCol;

    // a prefab for the explosion that plays on damaging opponents 
    public GameObject explosionPrefab;

    virtual internal void Start() { }
    virtual internal void Update () { }
    
    // add objects to the targeted list when they enter the collider 
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!targetedObj.Contains(other.gameObject) && other.gameObject.GetComponent<Health>())
        {
            targetedObj.Add(other.gameObject);
            Debug.Log(other.gameObject.name + " added to target list!");
            gameObject.GetComponent<SpriteRenderer>().color = targetingCol; 
        }
    }

    // remove objects from the targeted list when they exit the collider
    void OnTriggerExit2D(Collider2D other)
    {
        if (targetedObj.Contains(other.gameObject))
        {
            targetedObj.Remove(other.gameObject);
            Debug.Log(other.gameObject.name + " removed from target list."); 
            // if there's nothing left to target, return to base color. 
            if (targetedObj.Count == 0)
            {
                gameObject.GetComponent<SpriteRenderer>().color = baseCol; 
            }
        }
    }

    // Have the weapon fire on all currently targeted game objects
    virtual public void Activate()
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
        if (other != null && other.GetComponent<Health>() != null) {
            Debug.Log("Attacking " + other.name + " with " + this.gameObject.name); 
            other.GetComponent<Health>().Damage(power);

            // create a smaller explosion at the target site
            GameObject e = Instantiate(explosionPrefab, other.transform.position, Quaternion.identity);
            e.transform.localScale = e.transform.localScale * 0.6f; 
        }
    }

    
}
