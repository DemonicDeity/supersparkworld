using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for controlling the Red Balloon
public class RedBalloonController : MonoBehaviour {

    int hitpoints; // Red Balloon hp
    Animator animator; // Red Balloon animator
    int state; // Red Balloon state

	void Start () {
        hitpoints = 2;
        state = 0;
        animator = GetComponent<Animator>();
	}

    public void applyDamage(int damage)
    {
        // Red Balloon takes one damage from every hit
        hitpoints = hitpoints - 1;

        // If Red Balloon is at 1 hp, set to upset
        if (state == 0 && hitpoints == 1)
        {
            state = 1;
            animator.SetInteger("state", state);
        }
        // If the Red Balloon is at 0 hp, set to explode
        else if (hitpoints >= 0)
        {
            state = 2;
            animator.SetInteger("state", state);
        }
    }

    // Destroy Red Balloon
    public void destroyBalloon()
    {
        Destroy(gameObject);
    }
}
