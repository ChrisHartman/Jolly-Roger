using UnityEngine;
using System;
using System.Collections.Generic;


/// <summary>
/// Implements player control of tanks, as well as collision detection.
/// </summary>
public class ShipController : MonoBehaviour {
    /// <summary>
    /// How hard to speed up with sail lowered
    /// </summary>
    public float DefaultForce = 30f;

    public float MaxSlowSpeed = 1.6f;
    public float MaxFastSpeed = 3.0f;
    
     /// <summary>
    /// How much additional force the sail should provide
    /// </summary>
    public float ForceIncrease = 1f;

    /// <summary>
    /// How hard to turn
    /// </summary>
    public float TurnTorque = 1f;
    public float Smoothing = 5f;
    /// <summary>
    /// Keyboard controls for the player.
    /// </summary>
    public KeyCode ForwardKey, AltForwardKey, LeftKey, AltLeftKey, RightKey, AltRightKey, BackKey, AltBackKey, SpeedUpKey;
    private bool RaisedSail;
    private float ForwardForce;
    private float MaxSpeed;
    private Rigidbody2D boatRb;
    private CameraController cameraController;

    /// <summary>
    /// Weapon controls and variables 
    /// </summary>
    public KeyCode FireKey = KeyCode.Space;
    public KeyCode IncrementWeaponSelection = KeyCode.E;
    public KeyCode DecrementWeaponSelection = KeyCode.Q;
    public List<GameObject> weapons = new List<GameObject>();
    private GameObject activeWeapon;
    private int weaponIndex = 0;

    internal void Start() {
        GetComponent<Health>().OnDeath += Die;
        MaxSpeed = MaxSlowSpeed;
        boatRb = GetComponent<Rigidbody2D>();
        cameraController = FindObjectOfType<CameraController>();
        ForwardForce = DefaultForce;
        RaisedSail = false; 
    }

    public void RaiseSail() {
        ForwardForce = DefaultForce + ForceIncrease;
        if (activeWeapon != null) {
                    activeWeapon.GetComponent<AreaWeapon>().Disable();
                    activeWeapon = null; 
        }
        MaxSpeed = MaxFastSpeed;
        RaisedSail = true;
        cameraController.ZoomOut();
    }

    public void LowerSail() {
        ForwardForce = DefaultForce;
        RaisedSail = false;
        cameraController.ZoomIn();
        MaxSpeed = MaxSlowSpeed;
    }

    internal void FixedUpdate() {
        // This keeps the boat from drifting around
        var s_right = Vector2.Dot(boatRb.velocity, transform.right);
        boatRb.AddForce(-1f * s_right * transform.right);

        // Directional Controls
        if(Input.GetKey(ForwardKey) || Input.GetKey(AltForwardKey)) {
            boatRb.AddForce(transform.up * ForwardForce * Time.fixedDeltaTime);
        }
        if (Input.GetKey(BackKey) || Input.GetKey(AltBackKey)) {
            // this could maybe make the ship slow down? 
            //boatRb.AddForce(transform.up * -ForwardForce * Time.fixedDeltaTime);
        }
        if (Input.GetKey(LeftKey) || Input.GetKey(AltLeftKey)) {
            boatRb.AddTorque(TurnTorque*Time.fixedDeltaTime);
        }
        if (Input.GetKey(RightKey) || Input.GetKey(AltRightKey)) {
            boatRb.AddTorque(-TurnTorque*Time.fixedDeltaTime);
        }
        if (boatRb.velocity.magnitude > MaxSpeed) {
            boatRb.velocity = Vector3.Slerp(boatRb.velocity, boatRb.velocity.normalized * MaxSpeed, Time.fixedDeltaTime*Smoothing);
        }
    }

    void Die() {
        Destroy(this.gameObject);
    }

    internal void Update()
    {
        if (Input.GetKeyDown(SpeedUpKey)) {
            RaiseSail();
        } else if (!Input.GetKey(SpeedUpKey) && RaisedSail) {
            LowerSail();
        }
        // I'm doing this input in Update so that the frame the key is pressed isn't missed 
        if (!RaisedSail) {
            if (Input.GetKeyDown(FireKey))
            {
                    if (activeWeapon == null)
                    {
                        // create a new instance of our weapon 
                        activeWeapon = Instantiate(weapons[weaponIndex], this.transform, false); 
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
        // change the selected weapon
        if (Input.GetKeyDown(IncrementWeaponSelection))
        {
            weaponIndex++;
            // wrap around if the player scrolls past the largest index
            if (weaponIndex >= weapons.Count)
            {
                weaponIndex = 0;
            }
            // change weapon display
            // TODO make this less horrible when i'm not tired  
            GameObject.Find("Active Weapon Display").GetComponent<WeaponDisplay>().ChangeActiveWeapon(weapons[weaponIndex].name); 
        }
        else if (Input.GetKeyDown(DecrementWeaponSelection))
        {
            weaponIndex--;
            // wrap around if player scrolls past zero
            if (weaponIndex < 0)
            {
                weaponIndex = weapons.Count - 1; 
            }
            // change weapon display
            // TODO make this less horrible when i'm not tired  
            GameObject.Find("Active Weapon Display").GetComponent<WeaponDisplay>().ChangeActiveWeapon(weapons[weaponIndex].name);
        }
    }
    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    internal void OnCollisionEnter(Collision other)
    {
        Debug.Log("ouchie");
        GetComponent<Health>().Damage(10f);
    }
}
