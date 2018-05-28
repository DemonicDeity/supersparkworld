using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Script for controlling the height of the walls.
public class StoneWallControllerScript : MonoBehaviour {

    public float desiredYPosition; // Desired place for the walls to travel to
    public float speed; // Speed at which walls travel
    public int decimalPointSpeed; // Decimal to round to (actually matters because of Unity :/ )

	// Use this for initialization
	void Start () {
        desiredYPosition = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        // Checks to see if the walls are in the correct place
        if (!inCorrectPosition())
        {
            // Raises or lowers walls to their correct location
            if (desiredYPosition < transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, (float) Math.Round(transform.position.y - speed, decimalPointSpeed), transform.position.z);
            }
            else if (desiredYPosition > transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, (float)Math.Round(transform.position.y + speed, decimalPointSpeed), transform.position.z);
            }
        }
    }

    // Sets the variables for wall movement
    public void setDesiredYPosition(float incomingYPosition, float incomingSpeed, int incomingDecimalPointSpeed)
    {
        desiredYPosition = incomingYPosition;
        speed = incomingSpeed;
        decimalPointSpeed = incomingDecimalPointSpeed;

    }

    // Checks to see if the walls are in the correct position
    public bool inCorrectPosition()
    {
        if (desiredYPosition != transform.position.y)
            return false;
        return true;
    }
}
