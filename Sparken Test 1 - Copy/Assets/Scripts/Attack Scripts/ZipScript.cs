using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the hitbox for Zip
public class ZipScript : MonoBehaviour {

    Rigidbody2D rb2d; // Sparken Rigidbody
    Vector2 stepHorizontal; // Speed horizontally
    Vector2 stepVertical; // Speed vertically
    Vector2 stepStop; // Speed in place
    public bool stepForwardTrigger; // Switch for turning on speed

    private Animator animator; // Sparken animator

    string lastDirection; // Last direction pressed

    void Awake()
    {
        rb2d = transform.parent.GetComponentInParent<Rigidbody2D>();
        stepHorizontal = new Vector2(10, 0);
        stepVertical = new Vector2(0, 10);
        stepStop = new Vector2(0, 0);
        stepForwardTrigger = false;

        animator = transform.parent.parent.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        stepForwardTrigger = true;  // Turns on movement while hitbox is enabled
        transform.parent.parent.gameObject.layer = 9; // Sets layer to invisible while hitbox is active

        lastDirection = transform.parent.parent.gameObject.GetComponent<AnimationTestScript>().lastDirectionPressed; // Gets last direction pressed

    }
    private void OnDisable()
    {
        stepForwardTrigger = false; // Sets speed off while hitbox is inactive
        rb2d.velocity = stepStop; // Resets velocity
        transform.parent.parent.gameObject.layer = 11; // Sets layer to character while hitbox is inactive
        animator.SetInteger("Action", 0); // Resets animation
        transform.parent.parent.transform.rotation = Quaternion.Euler(1, 1, 1); // Resets rotation
    }
    private void FixedUpdate()
    {
        // Changes velocity while stepforward is active
        if (stepForwardTrigger == true)
        {

            Vector2 v = rb2d.velocity;

            // These four functions change the rotation of the Sparken and then the direction he flies in for a short burst depending on last input.

            if (lastDirection == "Right" || lastDirection == "Left")
            {

                v.x = stepHorizontal.x * transform.parent.parent.localScale.x;
                v.y = stepHorizontal.y;
                transform.parent.parent.transform.rotation = Quaternion.Euler(1, 1, -90 * transform.parent.parent.localScale.x);
            }

            else if (lastDirection == "Up")
            {

                v.x = stepVertical.x * transform.parent.parent.localScale.x;
                v.y = stepVertical.y;
                transform.parent.parent.transform.rotation = Quaternion.Euler(1, 1, 1);

            }
            else if (lastDirection == "Down")
            {

                v.x = stepVertical.x * transform.parent.parent.localScale.x;
                v.y = -stepVertical.y;
                transform.parent.parent.transform.rotation = Quaternion.Euler(1, 1, 180);

            }

            else
            {
            }

            rb2d.velocity = v;
        }

    }
}
