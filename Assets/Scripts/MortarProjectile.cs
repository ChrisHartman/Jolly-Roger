using UnityEngine;

/// <summary>
/// The ordnance shot by the tanks
/// </summary>
public class MortarProjectile : MonoBehaviour {
    /// <summary>
    /// Who shot us
    /// </summary>
    public GameObject Creator;
	

	public float MortarSpeed = 3f;

    private Vector3 InitialPos;
    private Vector3 Target;
    private Vector3 Direction;
	private float explosionTime;

    /// <summary>
    /// Start the projectile moving.
    /// </summary>
    /// <param name="creator">Who's shooting</param>
    /// <param name="pos">Where to place the projectile</param>
    /// <param name="direction">Direction to move in (unit vector)</param>
    public void Init(GameObject creator, Vector3 pos, Vector3 target)
    {
        Creator = creator;
        transform.position = target;
        Target=target;
		explosionTime = Time.time + MortarSpeed;

		Destroy(GetComponent<CircleCollider2D>());
    }

    internal void Update(){

		if(Time.time> explosionTime){

			 gameObject.AddComponent<CircleCollider2D>();
            
			if(Time.time> (explosionTime + 1f)){

				Destroy(gameObject);
			}
        }
    }
}
