using UnityEngine;

/// <summary>
/// The ordnance shot by the tanks
/// </summary>
public class fireProjectile : MonoBehaviour {
    /// <summary>
    /// Who shot us
    /// </summary>
    public GameObject Creator;
    /// <summary>
    /// How fast to move
    /// </summary>
    public float Speed = 3f;
    private float distance;

    private Vector3 InitialPos;
    private Vector3 Direction;

    /// <summary>
    /// Do dammage if hitting player
    /// </summary>
    /// <param name="collision"></param>
    internal void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<ShipController>() != null)
        {  
            collision.gameObject.GetComponent<Health>().Damage(.5f);
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Start the projectile moving.
    /// </summary>
    /// <param name="creator">Who's shooting</param>
    /// <param name="pos">Where to place the projectile</param>
    /// <param name="direction">Direction to move in (unit vector)</param>
    public void Init(GameObject creator, Vector3 pos, Vector3 target, float d)
    {
        Creator = creator;
        distance = d;
        transform.position = pos;
		InitialPos=pos;
        Direction = target-pos;
        GetComponent<Rigidbody2D>().velocity = Direction.normalized * Speed;
    }

    internal void Update(){

        if(Vector3.Distance(transform.position,InitialPos)>distance){

            Destroy(gameObject);
        }
    }
}
