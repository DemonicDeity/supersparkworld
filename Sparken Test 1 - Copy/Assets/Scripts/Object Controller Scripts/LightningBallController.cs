using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for controlling the Lightningball
public class LightningBallController : MonoBehaviour {

    private Animator animator; // Lightningball animator
    Rigidbody2D rb2d; //Lightningball Rigidbody
    int duration; // Duration before explosion

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Start () {
        duration = 50;
    }
	
	void FixedUpdate () {
        // Reduce duration
        if (duration > 0)
        {
            duration--;
        }
        // When duration reaches 0, explode
        else
        {
            animator.SetTrigger("LightningBallExplodeTrigger");
            rb2d.velocity = new Vector2(0, 0);
        }
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        // When the lightningball hits the ground, a wall, or enemy, it explodes
        if (coll.gameObject.tag == "Ground" || coll.gameObject.tag == "Final" || coll.gameObject.tag == "Enemy")
        {
            animator.SetTrigger("LightningBallExplodeTrigger");
            rb2d.velocity = new Vector2(0, 0);
        }

    }
    // Destroys the lightningball
    void lightningBallExplodeFunction()
    {
        Destroy(gameObject);
    }

}
