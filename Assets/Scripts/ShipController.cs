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

    public float IslandDamage = 5f;
    public float MaxSlowSpeed = 1.6f;
    public float MaxFastSpeed = 3.0f;
    public float FastCollisionDamage = 5f;
    public float SlowCollisionDamage = 10f;
    public float SlowDrag = 70f;
    private float NormalDrag;
    
     /// <summary>
    /// How much additional force the sail should provide
    /// </summary>
    public float ForceIncrease = 1f;

    /// <summary>
    /// How hard to turn
    /// </summary>
    public float TurnTorque = 1f;
    public float Smoothing = 5f;
    public float IncapacitatedTime = .5f;
    public float IslandCollisionVelocityThreshold = .8f;
    
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
    private float MoveTime;
    private bool Incapacitated; 

    internal void Start() {
        MaxSpeed = MaxSlowSpeed;
        GetComponent<Health>().OnDeath += Die;
        Incapacitated = false;
        MoveTime = 0;
        boatRb = GetComponent<Rigidbody2D>();
        cameraController = FindObjectOfType<CameraController>();
        ForwardForce = DefaultForce;
        RaisedSail = false; 
        NormalDrag = boatRb.drag;
        GameObject.Find("Active Weapon Display").GetComponent<WeaponDisplay>().ChangeActiveWeapon(weapons[weaponIndex].name);
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
        if (!Incapacitated) {
            MaxSpeed = MaxSlowSpeed;
        }
    }

    internal void FixedUpdate() {
        // This keeps the boat from drifting around
        var s_right = Vector2.Dot(boatRb.velocity, transform.right);
        boatRb.AddForce(-1f * s_right * transform.right);

        // Directional Controls
        if(Input.GetKey(ForwardKey) || Input.GetKey(AltForwardKey)) {
            boatRb.AddForce(transform.up * ForwardForce * Time.fixedDeltaTime);
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
        Debug.Log("Dying!");
        Destroy(this.gameObject);

        Debug.Log("Returning to main menu...");
        GameObject.Find("Level Manager").GetComponent<LevelManager>().LoadLevel("Main Menu"); 
    }
    
    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D other)
    {
        // This isn't implemented great, but the goal is to make it hitting an Island
        // hurt you, stop you, and allow you to leave
        if (other.gameObject.tag == "Island") {
            if (other.relativeVelocity.magnitude > IslandCollisionVelocityThreshold && Time.time > MoveTime) {
                MaxSpeed = 0;
                Incapacitated = true;
                GetComponent<Health>().Damage(IslandDamage);
                MoveTime = Time.time + IncapacitatedTime;
                if (RaisedSail) LowerSail();
            }
        } else {
            GetComponent<Health>().Damage(5f);
        }

        // float damage = SlowCollisionDamage;
        // if (RaisedSail) {
        //     damage = FastCollisionDamage;
        // }
        // GetComponent<Health>().Damage(damage);
    }

    internal void Update()
    {
        if (Incapacitated) {
            if (Time.time > MoveTime) {
                Incapacitated = false;
                MaxSpeed = MaxSlowSpeed;
                if (RaisedSail) {
                    MaxSpeed = MaxFastSpeed;
                }
            }
        }
        if (Input.GetKeyDown(BackKey) || Input.GetKeyDown(AltBackKey)) { 
            boatRb.drag = SlowDrag;
        } else if (Input.GetKeyUp(BackKey) || Input.GetKeyDown(AltBackKey)) {
            boatRb.drag = NormalDrag;
        }
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

}
