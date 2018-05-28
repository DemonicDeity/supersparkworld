using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Simple script to update the last checkpoint crossed.
public class CheckpointControllerScript : MonoBehaviour {

    Animator animator;
    public int checkpointNumber;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            // If the player is ariving at a new checkpoint
            if (GameControllerScript.gameController.getLastCheckpoint() != checkpointNumber)
            {
                animator.SetTrigger("ShrineEnterTrigger"); // Set checkpoint animation
                GameControllerScript.gameController.setLastCheckpoint(checkpointNumber); // Set this as the last checkpoint crossed
                GameControllerScript.gameController.saveGame(); // Save the game
            }
        }
    }
}
