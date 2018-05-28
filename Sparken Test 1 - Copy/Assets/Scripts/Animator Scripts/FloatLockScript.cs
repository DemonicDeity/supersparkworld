using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script meant to hold player in place while the state is active. Current not functioning correctly.
public class FloatLockScript : StateMachineBehaviour {

    GameObject sparken; // Grabs the Sparken
    Rigidbody2D rb2d; // Grabs the Sparken's rigidbody
    Vector2 freeze; // V2 which holds player's position

    public void Awake()
    {
        sparken = GameObject.FindGameObjectWithTag("Player");
        rb2d = sparken.GetComponent<Rigidbody2D>();
        freeze = new Vector2(0, 0);
    }

    // On entering, it freezes the Sparken's position.
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        rb2d.gravityScale = 0;
        rb2d.velocity = freeze;
    }

	// On exiting, it sets it back to normal.
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        rb2d.gravityScale = 1;
    }

}
