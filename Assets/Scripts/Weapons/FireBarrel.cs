using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBarrel : MonoBehaviour
{
    /// <summary>
    /// How much damage we do
    /// </summary>
    public float damage = 5f;

    /// <summary>
    /// Explosion created when we damage something
    /// </summary>
    public GameObject explosionPrefab; 

    internal void Die()
    {
        Destroy(gameObject); 
    }

    // If an enemy runs into us, damage it
    internal void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Health>() != null && collision.gameObject.GetComponent<ShipController>() == null)
        {
            collision.gameObject.GetComponent<Health>().Damage(damage);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity); 
            Die();
        }
    }
}
