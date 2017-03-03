using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour {
    // A simple script for automating small animations 

    // the frames of the animation 
    public List<Sprite> frames = new List<Sprite>();
    private int currentFrame = 0; 

    // whether the animation should loop 
    public bool loop = false;

    // how much time (in seconds) should pass between frames of the animation 
    public float frameTime = 0.5f;
    private float passedTime = 0f; 

    internal void Update()
    {
        // add the time between frames to passedTime
        passedTime += Time.deltaTime; 

        // if enough time has passed, reset the timer and change the sprite 
        if (passedTime >= frameTime)
        {
            passedTime = 0f;
            UpdateSprite(); 
        }
    }

    // change sprite to next sprite in list
    // if there are no more sprites in list, either exit or loop 
    private void UpdateSprite()
    {
        // increment frame counter
        currentFrame++; 
        
        if (currentFrame < frames.Count)
        {
            // advance animation state
            gameObject.GetComponent<SpriteRenderer>().sprite = frames[currentFrame]; 
        }
        else
        {
            // if we're looping, set currentFrame to 0 and advance animation state
            if (loop)
            {
                currentFrame = 0;
                gameObject.GetComponent<SpriteRenderer>().sprite = frames[currentFrame]; 
            }
            else
            {
                // otherwise, we're done with the animation, so destroy the object 
                Destroy(gameObject); 
            }
        }
    }
}
