using UnityEngine;

/// <summary>
/// Implements player control of tanks, as well as collision detection.
/// </summary>
public class ShipController : MonoBehaviour {

    public float health = 100f;
    /// <summary>
    /// How fast to drive
    /// </summary>
    public float ForwardForce = 1f;
    private float NormalForce;
    public float ForceIncrease = 1f;
    //public float ForwardSpeed = 1f;
    /// <summary>
    /// How fast to turn
    /// </summary>
    public float TurnTorque = 1f;

    /// <summary>
    /// Keyboard controls for the player.
    /// </summary>
    private Rigidbody2D boatRb;
    public KeyCode ForwardKey, LeftKey, RightKey, BackKey, SpeedUpKey;
    public GameObject weapon;
    private GameObject activeWeapon; 
    public KeyCode FireKey = KeyCode.Space; 

    /// <summary>
    /// Current rotation of the tank (in degrees).
    /// We need this because Unity's 2D system is built on top of its 3D system and so they don't
    /// give you a method for finding the rotation that doesn't require you to know what a quaternion
    /// is and what Euler angles are.  We haven't talked about those yet.
    /// </summary>

    internal void Start() {
        boatRb = GetComponent<Rigidbody2D>();
        NormalForce = ForwardForce;
        
    }
    internal void FixedUpdate() {
        //transform.position += (transform.up * velocity * Time.deltaTime);
        if (Input.GetKeyDown(SpeedUpKey)) {
            ForwardForce = NormalForce + ForceIncrease;
        } else if (Input.GetKeyUp(SpeedUpKey)) {
            ForwardForce = NormalForce;
        }
        var s_right = Vector2.Dot(boatRb.velocity, transform.right);
        boatRb.AddForce(-1f * s_right * transform.right);
        if(Input.GetKey(ForwardKey)) {
            boatRb.AddForce(transform.up * ForwardForce * Time.fixedDeltaTime);
        }
        if (Input.GetKey(BackKey)) {
            // this could maybe make the ship slow down? 
            //boatRb.AddForce(transform.up * -ForwardForce * Time.fixedDeltaTime);
        }
        if (Input.GetKey(LeftKey)) {
            boatRb.AddTorque(TurnTorque*Time.fixedDeltaTime);
        }
        if (Input.GetKey(RightKey)) {
            boatRb.AddTorque(-TurnTorque*Time.fixedDeltaTime);
        }
    }

    internal void Update()
    {
        // I'm doing this input in Update so that the frame the key is pressed isn't missed 
        if (Input.GetKeyDown(FireKey))
        {
            if (activeWeapon == null)
            {
                // create a new instance of our weapon 
                activeWeapon = Instantiate(weapon, this.transform, false); 
            }
        }
        if (Input.GetKeyUp(FireKey))
        {
            if (activeWeapon != null)
            {
                activeWeapon.GetComponent<AreaWeapon>().Activate();
                activeWeapon = null; 
            }
        }
    }

    internal void OnTriggerEnter2D(Collider2D other) {
        
    }
}
