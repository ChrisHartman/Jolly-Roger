using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

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
    public KeyCode ForwardKey, AltForwardKey, LeftKey, AltLeftKey, RightKey, AltRightKey, BackKey, AltBackKey, SpeedUpKey, AlternateSpeedUpKey;
    private bool RaisedSail;
    private float ForwardForce;
    private float MaxSpeed;
    private Rigidbody2D boatRb;
    private CameraController cameraController;

    /// <summary>
    /// Weapon controls and variables 
    /// </summary>
    public KeyCode FireKey = KeyCode.Space;
    /*
    public KeyCode IncrementWeaponSelection = KeyCode.E;
    public KeyCode DecrementWeaponSelection = KeyCode.Q;
    public KeyCode AlternateIncrementWeaponSelection;
    public KeyCode AlternateDecrementWeaponSelection;
    */
    public KeyCode Hotkey1 = KeyCode.J;
    public KeyCode Hotkey2 = KeyCode.K;
    public KeyCode Hotkey3 = KeyCode.L;
    private Image weaponIcon1, weaponIcon2, weaponIcon3; 
    public List<GameObject> weapons = new List<GameObject>();
    private GameObject activeWeapon;
    private int weaponIndex = 0;
    private float MoveTime;
    private bool Incapacitated;
    private List<float> cooldownTimers = new List<float>();

    private float Gold;
    private float Fabric;
    private float Metal;
    private float Wood;

    internal void Start() {
        MaxSpeed = MaxSlowSpeed;
        GetComponent<Health>().OnDeath += Die;
        GetComponent<Health>().OnHit += GetComponent<AudioSource>().Play;
        Incapacitated = false;
        boatRb = GetComponent<Rigidbody2D>();
        cameraController = FindObjectOfType<CameraController>();
        ForwardForce = DefaultForce;
        RaisedSail = false; 
        NormalDrag = boatRb.drag;

        for (int i = 0; i < weapons.Count; i++)
        {
            cooldownTimers.Add(0f); 
        }
        weaponIcon1 = GameObject.Find("Weapon Icon 1").GetComponent<Image>();
        weaponIcon2 = GameObject.Find("Weapon Icon 2").GetComponent<Image>();
        weaponIcon3 = GameObject.Find("Weapon Icon 3").GetComponent<Image>(); 
    }

    public void RaiseSail() {
        ForwardForce = DefaultForce + ForceIncrease;
        if (activeWeapon != null) {
                    activeWeapon.GetComponent<WeaponGroup>().Disable();
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
        //Debug.Log("Dying!");
        Destroy(this.gameObject);

        //Debug.Log("Returning to main menu...")
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
        if (other.gameObject.tag == "Projectile") {
            GetComponent<Health>().Damage(5f);
        }

        // float damage = SlowCollisionDamage;
        // if (RaisedSail) {
        //     damage = FastCollisionDamage;
        // }
        // GetComponent<Health>().Damage(damage);
    }

    public void Crash(Collision2D other) {
         if (other.relativeVelocity.magnitude > IslandCollisionVelocityThreshold && Time.time > MoveTime) {          
            MaxSpeed = 0;
            Incapacitated = true;
            GetComponent<Health>().Damage(IslandDamage);
            MoveTime = Time.time + IncapacitatedTime;
            if (RaisedSail) LowerSail();
         }
    }

    internal void Update()
    {
        // tick down cooldown timers
        for (int i = 0; i < cooldownTimers.Count; i++)
        {
            cooldownTimers[i] -= Time.deltaTime;
        }

        // change weapon icon colors to reflect cooldowns 
        // TODO: do this any other way jesus christ 
        weaponIcon1.color = Color.Lerp(Color.white, Color.black, cooldownTimers[0] / weapons[0].GetComponent<WeaponGroup>().cooldown);
        weaponIcon2.color = Color.Lerp(Color.white, Color.black, cooldownTimers[1] / weapons[1].GetComponent<WeaponGroup>().cooldown);
        weaponIcon3.color = Color.Lerp(Color.white, Color.black, cooldownTimers[2] / weapons[2].GetComponent<WeaponGroup>().cooldown); 

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
        if (Input.GetKeyDown(SpeedUpKey) || Input.GetKeyDown(AlternateSpeedUpKey)) {
            RaiseSail();
        } else if (!((Input.GetKey(SpeedUpKey) ||   Input.GetKey(AlternateSpeedUpKey))) && RaisedSail) {
            LowerSail();
        }
        // I'm doing this input in Update so that the frame the key is pressed isn't missed 
        if (!RaisedSail) {

            // Weapons no longer based around usage of fire key
            /*
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
                    activeWeapon.GetComponent<WeaponGroup>().Activate();
                    activeWeapon = null; 
                }
            }
            */

            // TODO put the weapon codes into an enum, i just didn't because i'm lazy 
            // Instead, player controls weapons using individual keys
            // Firing Mortars NUM == 0
            if (Input.GetKeyDown(Hotkey1))
            {
                ActivateWeapon(0); 
            }
            if (Input.GetKeyUp(Hotkey1))
            {
                DeactivateWeapon(0); 
            }
            // Firing Cannons NUM == 1
            if (Input.GetKeyDown(Hotkey2))
            {
                ActivateWeapon(1); 
            }
            if (Input.GetKeyUp(Hotkey2))
            {
                DeactivateWeapon(1); 
            }
            // Dropping Fire Barrel NUM == 2
            if (Input.GetKeyDown(Hotkey3))
            {
                ActivateWeapon(2); 
            }
            if (Input.GetKeyUp(Hotkey3))
            {
                DeactivateWeapon(2); 
            }
        }
        /*
        // change the selected weapon
        if (Input.GetKeyDown(IncrementWeaponSelection) || Input.GetKeyDown(AlternateIncrementWeaponSelection))
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
        else if (Input.GetKeyDown(DecrementWeaponSelection) || Input.GetKeyDown(AlternateDecrementWeaponSelection))
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
        */
    }

    private void ActivateWeapon(int w)
    {
        if (activeWeapon == null && cooldownTimers[w] <= 0f)
        {
            activeWeapon = Instantiate(weapons[w], this.transform, false);
        }
    }

    private void DeactivateWeapon(int w)
    {
        if (activeWeapon != null)
        {
            cooldownTimers[w] = activeWeapon.GetComponent<WeaponGroup>().cooldown;
            activeWeapon.GetComponent<WeaponGroup>().Activate();
            activeWeapon = null;
        }
    }
}
