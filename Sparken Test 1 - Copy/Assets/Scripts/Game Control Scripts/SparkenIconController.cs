using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the health icons of Sparken 
public class SparkenIconController : MonoBehaviour {

    public Sprite healthActive; // Sprite for active life
    public Sprite healthInactive; // Sprite for deactive life

    Animator iconAnimator; // Animator for the Sparken Life Icons
    Animator sparkenAnimator; // Animator for Sparken
    GameObject sparken; // Gets Sparken
    PlayerController sparkenController; // Gets Sparken's Playercontroller
    int sparkenState; // Holds Sparken's state
    int action; // Holds Sparken's action
    int hurtTimer; // Holds Sparken's hurt timer
    int hitpoints; // Holds Sparken's hitpoints
    int hitpointMaximum; // Holds Sparken's hitpointmaximum
    int hitpointIcons; // Holds the number of hitpoint icons there are

	// Reloads the Sparken Icons with data
	void reload () {
        sparken = GameObject.FindGameObjectWithTag("Player");
        iconAnimator = GetComponent<Animator>();
        sparkenAnimator = sparken.GetComponent<Animator>();
        sparkenController = sparken.GetComponent<PlayerController>();

        hitPointUpdate();
    }
	
	// Update is called once per frame
	void Update () {
        // Reloads the Sparken Icons if there is not Sparken
        if (sparken == null)
        {
            reload();
        }
        action = sparkenAnimator.GetInteger("Action");
        hurtTimer = sparkenAnimator.GetInteger("Hurt Time");
        hitpoints = sparkenController.hitpoints;
        hitpointMaximum = sparkenController.hitpointMaximum;

        // If Sparken is dead, set face to dead
        if (hitpoints <= 0)
        {
            sparkenState = 3;
        }
        // If Sparken is hurt, set face to hurt
        else if (hurtTimer > 0)
        {
            sparkenState = 2;
        }
        // If Sparken is attacking, set face to attacking
        else if (action > 4)
        {
            sparkenState = 1;
        }
        // If Sparken is inactive, do nothing
        else
        {
            sparkenState = 0;
        }
        // Send the state to the animator
        iconAnimator.SetInteger("Sparken State", sparkenState);
    }

    // Updates the icons for Sparken's life to invisible, inactive, and active
    public void hitPointUpdate()
    {
        hitpoints = sparkenController.hitpoints;
        hitpointMaximum = sparkenController.hitpointMaximum;

        hitpointIcons = transform.childCount - 1;

        while (hitpointIcons >= 0)
        {
            if (hitpoints >= hitpointIcons + 1)
            {
                transform.GetChild(hitpointIcons).gameObject.GetComponent<SpriteRenderer>().sprite = healthActive;
            }
            else if (hitpointMaximum >= hitpointIcons + 1)
            {
                transform.GetChild(hitpointIcons).gameObject.GetComponent<SpriteRenderer>().sprite = healthInactive;
            }
            else
            {
                transform.GetChild(hitpointIcons).gameObject.SetActive(false);
            }
            hitpointIcons--;
        }
    }
}
