using UnityEngine;
using System;


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

    private bool RaisedSail;
    //public float ForwardSpeed = 1f;
    /// <summary>
    /// How fast to turn
    /// </summary>
    public float TurnTorque = 1f;


    /// <summary>
    /// Keyboard controls for the player.
    /// </summary>
    public KeyCode ForwardKey, AltForwardKey, LeftKey, AltLeftKey, RightKey, AltRightKey, BackKey, AltBackKey, SpeedUpKey;
    public KeyCode FireKey = KeyCode.Space; 
    public GameObject weapon;
    private GameObject activeWeapon; 
    private Rigidbody2D boatRb;

    private CameraController cameraController;
    /// <summary>
    /// Current rotation of the tank (in degrees).
    /// We need this because Unity's 2D system is built on top of its 3D system and so they don't
    /// give you a method for finding the rotation that doesn't require you to know what a quaternion
    /// is and what Euler angles are.  We haven't talked about those yet.
    /// </summary>

    internal void Start() {
        boatRb = GetComponent<Rigidbody2D>();
        cameraController = FindObjectOfType<CameraController>();
        NormalForce = ForwardForce;
        RaisedSail = false; 
    }

    public void RaiseSail() {
        ForwardForce = NormalForce + ForceIncrease;
        if (activeWeapon != null) {
                    activeWeapon.GetComponent<AreaWeapon>().Disable();
                    activeWeapon = null; 
        }
        RaisedSail = true;
        cameraController.ZoomOut();
    }

    public void LowerSail() {
        ForwardForce = NormalForce;
        RaisedSail = false;
        cameraController.ZoomIn();
    }

    internal void FixedUpdate() {
        //transform.position += (transform.up * velocity * Time.deltaTime);
        var s_right = Vector2.Dot(boatRb.velocity, transform.right);
        boatRb.AddForce(-1f * s_right * transform.right);
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
    }
}
