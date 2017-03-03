using UnityEngine;

/// <summary>
/// The ordnance shot by the tanks
/// </summary>
public class PlayerShipProjectile : MonoBehaviour {
    /// <summary>
    /// Who shot us
    /// </summary>
    public GameObject Creator;
    /// <summary>
    /// How fast to move
    /// </summary>
    public float Speed = .01f;
    private Vector3 defaultScale = new Vector3(3,3,1);
    /// <summary>
    /// How much damage we do
    /// </summary>
    public float damage = 5f; 

    private float EndTime;


    internal void Die() {
        Destroy(gameObject);
    }
    /// <summary>
    /// Do dammage if hitting an enemy 
    /// </summary>
    /// <param name="collision"></param>
    internal void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<Health>() != null && collision.gameObject.GetComponent<ShipController>() == null)
        {
            collision.gameObject.GetComponent<Health>().Damage(damage);
            Die(); 
        }        
    }

    /// <summary>
    /// Start the projectile moving.
    /// </summary>
    /// <param name="creator">Who's shooting</param>
    /// <param name="pos">Where to place the projectile</param>
    /// <param name="direction">Direction to move in (unit vector)</param>
    public void Init(Vector3 pos, Vector3 direction, float airTime = 15f)
    {
        transform.position = pos;
        GetComponent<Rigidbody2D>().velocity = direction.normalized * Speed;
        Invoke("Die", airTime);
    }

}
