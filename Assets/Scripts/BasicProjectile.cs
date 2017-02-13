using UnityEngine;

/// <summary>
/// The ordnance shot by the tanks
/// </summary>
public class BasicProjectile : MonoBehaviour {
    /// <summary>
    /// Who shot us
    /// </summary>
    public GameObject Creator;
    /// <summary>
    /// How fast to move
    /// </summary>
    public float Speed = .01f;

    private float EndTime;


    internal void Die() {
        Destroy(gameObject);
    }
    /// <summary>
    /// Do dammage if hitting player
    /// </summary>
    /// <param name="collision"></param>
    internal void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.GetComponent<ShipController>() != null || Time.time > EndTime)
        {
            Destroy(gameObject);
        }        
    }

    /// <summary>
    /// Start the projectile moving.
    /// </summary>
    /// <param name="creator">Who's shooting</param>
    /// <param name="pos">Where to place the projectile</param>
    /// <param name="direction">Direction to move in (unit vector)</param>
    public void Init(GameObject creator, Vector3 pos, Vector3 direction, float airTime = 15f)
    {
        Creator = creator;
        transform.position = pos;
        GetComponent<Rigidbody2D>().velocity = direction.normalized * Speed;
        Invoke("Die", airTime);
    }
}
