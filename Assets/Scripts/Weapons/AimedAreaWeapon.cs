using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimedAreaWeapon : AreaWeapon {
    // A version of the area weapon that moves back and forth in order to require aiming

    // how far we want to move the collider
    public float moveDistance = 1f;
    // how many fractions we want to divide the movement into (more fractions = longer movement time)
    public float interpolateFrac = 0.1f; 

    // original position
    private Vector3 startingPos;
    // ending position 
    private Vector3 endPos;

    // whether we're increasing (heading toward endPos) or decreasing (heading toward startingPos)
    private bool increasing = true;
    // how far we've gone 
    private float distTraveled = 0;

    // switches between travelling in the y-axis and the x-axis
    public bool yAxis = false;

    override internal void Start()
    {
        // currently doesn't do anything 
        base.Start();

        // get the original position of the collider 
        startingPos = transform.localPosition;
        if (yAxis == false)
        {
            endPos = new Vector3(transform.localPosition.x + moveDistance, transform.localPosition.y);
        }
        else
        {
            endPos = new Vector3(transform.localPosition.x, transform.localPosition.y + moveDistance); 
        }
    }

    override internal void Update()
    {
        // currently doesn't do anything 
        base.Update();

        // move 
        if (increasing)
        {
            distTraveled += interpolateFrac; 
            if (distTraveled >= 1f)
            {
                distTraveled = 1f;
                increasing = false; 
            }
        }
        else
        {
            distTraveled -= interpolateFrac; 
            if (distTraveled <= 0f)
            {
                distTraveled = 0f;
                increasing = true; 
            }
        }

        transform.localPosition = Vector3.Lerp(startingPos, endPos, distTraveled); 
    }
}
