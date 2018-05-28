using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for controlling the Lightning Conductor
public class LightningConductorController : MonoBehaviour {

    public static LightningConductorController lightningConductorController; // Singleton for Lightning Conductor
    Animator animator; // Lightning Conductor animator
    public int status; // Status of Lightning Conductor

	void Awake () {
        lightningConductorController = this;
        animator = GetComponent<Animator>();

        animator.SetTrigger("Setup"); // Lightning Conductor begins constructing

        status = 0;
	}
	
	void Update () {
        // If a new Lightning Conductor is made, destroy itself
		if (lightningConductorController != this)
        {
            animator.SetTrigger("Deconstruct");
        }
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        // If an enemy collides with the Lightning Conductor, begin explosion
        if (coll.gameObject.tag == "Enemy")
        {
            animator.SetTrigger("Explode");
        }
    }

    // Destroys Lightning Conductor
    public void destroyLightningConductor()
    {
        Destroy(gameObject);
    }

    // Sets Lightning Conductor status
    public void setStatus(int incomingStatus)
    {
        status = incomingStatus;
    }
}
