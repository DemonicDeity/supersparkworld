using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for controlling the Seismic Tremor. Conditions for moving and being destroyed mainly.
public class SeismicTremorWaveControllerScript : MonoBehaviour {

    Rigidbody2D rb2d; // Rigidbody for Seismic Tremor

    public int leftOrRight; // Determines which direction it travels in

    Vector2 leftDirection; // Left Vector
    Vector2 rightDirection; // Right Vector

    int duration; // Length of time Seismic Tremor sits on the screen

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();

        leftDirection = new Vector2(-4, 0);
        rightDirection = new Vector2(4, 0);

        duration = 180;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        // Determines which direction it's supposed to travel in, then goes that direction
		if (leftOrRight == -1)
        {
            rb2d.velocity = leftDirection;
        }
        else if (leftOrRight == 1)
        {
            rb2d.velocity = rightDirection;
        }

        // Destroys the gameobject at the end of the duration
        if (duration > 0)
        {
            duration--;
        }
        else
        {
            Destroy(gameObject);
        }
	}


    private void OnCollisionEnter2D(Collision2D coll)
    {
        // If the Seismic Tremor hits a Finish tag, it is removed
        if (coll.collider.gameObject.tag == "Finish")
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        // If the Seismic Tremor leaves the ground, it is removed
        if (coll.collider.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
