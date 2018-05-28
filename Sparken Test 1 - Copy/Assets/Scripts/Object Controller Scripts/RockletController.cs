using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockletController : MonoBehaviour {

    public bool hostile; // Determines if the Rocklet will perform attack scripts

    public int hitpoints; // Rocklet Hitpoints
    Rigidbody2D rb2d; // Rocklet rigidbody2d
    int knockBackTimer; // Time of being stuck in Knockback animation
    Vector2 knockBack; // Knockback direction
    Vector2 velocitySet; // Intermediary velocity to avoid defining new Vector2s in loops
    bool grounded; // Whether the Rocklet is on the ground or not
    GameObject sparken; // The player character GameObject, Sparken
    int action; // Current action of the Rocklet
    // Actions in the Animator: 0 = Idle, 1 = Jumping, 2 = Walking, 3 = Spinning
    Animator animator; // The Rocklet Animator
    int jumpTimer; // Timer of being in the jump animation
    Vector2 jumpV; //jump velocity

    bool facing; // Facing false = right, true = left
    Vector2 faceRight; // Defines the value for facing right
    Vector2 faceLeft; // Defines the value for facing left

    int speed; // Current top speed of the Rocklet - Speed 1 = walk, Speed 3 = spin

    int detectRange; // Range at which the Rocklet will target the player
    int spinRange; // Range at which the Rocklet will begin its spin attack

    bool alive; // Determines if the Rocklet is alive


	// Use this for initialization
	void Start () {
        hitpoints = 5;
        knockBackTimer = 0;
        knockBack = new Vector2(0, 0);
        rb2d = GetComponent<Rigidbody2D>();
        sparken = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        jumpV = new Vector2(0, 5);

        faceRight = new Vector3(1, 1, 1);
        faceLeft = new Vector3(-1, 1, 1);

        speed = 1;

        detectRange = 7;
        spinRange = 4;

        alive = true;
    }
	
	// Update is called once per frame
	void Update () {
        conditionCheck(); // Checks to see if the Rocklet is alive every frame
	}

    private void FixedUpdate()
    {
        rockletAIAction();

        if (alive == true)
        {
            setFacingDirection();
        }

    }


    private void OnCollisionEnter2D(Collision2D coll)
    {
        // On colliding with the ground, set grounded to true
        if (coll.collider.gameObject.tag == "Ground")
        {
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        //On exiting collision with the ground, set grounded to false
        if (coll.collider.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }

    // Applies damage to the Rocklet's hitpoints
    public void applyDamage (int damage)
    {
        if (hostile == true)
        {
            hitpoints = hitpoints - damage;
        }
    }

    // Applies knockback to the Rocklet, setting knockback timer and direction
    public void applyKnockBack (object[] knockBackReciever)
    {
        if (hostile == true)
        {
            knockBack = (Vector2)knockBackReciever[0];
            knockBackTimer = (int)knockBackReciever[1];
            rb2d.velocity = knockBack;
        }
    }

    // Checks to see if the Rocklet still has health
    private void conditionCheck()
    {
        if (hitpoints <= 0 && grounded == true)
        {
            rb2d.velocity = new Vector2(0, 0);
            animator.SetBool("Alive", false);
            alive = false;
        }
    }

    // AI for the Rocklet
    public void rockletAIAction()
    {
        action = animator.GetInteger("Action");

        if (knockBackTimer > 0)
        {
            rockletKnockBack();
        }
        else if (jumpTimer > 0)
        {
            rockletJump();
        }
        else if (grounded == true && hostile == true && alive == true)
        {
            rockletAttackAndMovement();
        }
        animator.SetInteger("Action", action);
    }

    // Updates the Rocklet's jump, jump timer, and jump hitbox
    private void rockletJump()
    {
        rb2d.velocity = jumpV;
        jumpTimer--;
        if (jumpTimer == 0)
        {
            action = 0;
            transform.Find("Hitboxes").Find("Jump Attack Hitbox").gameObject.SetActive(false);
        }
    }

    // Set the Rocklet's jump timer, and turns on the hitbox
    private void rockletJumpSet (int setHeight)
    {
        jumpTimer = setHeight;
        transform.Find("Hitboxes").Find("Jump Attack Hitbox").gameObject.SetActive(true);
    }

    // Sets the rocklet to its knockback direction and reduces the knockback timer
    private void rockletKnockBack()
    {
        double hurtRotation = (Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) / Mathf.PI) * 180.0;

        Vector3 hurtRotationReal = new Vector3(1, 1, (float)hurtRotation - 90);

        transform.rotation = Quaternion.Euler(hurtRotationReal);

        knockBackTimer--;

        if (knockBackTimer <= 0)
        {
            transform.rotation = Quaternion.Euler(1, 1, 1);
        }
        animator.SetInteger("Hurt Timer", knockBackTimer);
    }

    // Determines which direction the Rocklet should turn to face
    private void setFacingDirection()
    {
        if (facing == false)
        {
            transform.localScale = faceRight;
        }
        else
        {
            transform.localScale = faceLeft;
        }
    }

    // A multi-part function which determines the Rocklet's speed, direction of movement, and attempted action
    // More instruction can be found at the top of every possible outcome
    private void rockletAttackAndMovement()
    {
        velocitySet = rb2d.velocity;

        velocitySet.x = 0;

        // Determines if the Rocklet is within range of tracking the Sparken
        if (Mathf.Abs(transform.position.x - sparken.transform.position.x) < detectRange)
        {
            animator.SetInteger("Rocklet Facing", 1); // Sets the Rocklet to look to the side

            // Determines if the Rocklet is within range of spinning on the Sparken
            if (transform.position.x + spinRange > sparken.transform.position.x && transform.position.x - spinRange < sparken.transform.position.x)
            {
                // Determines if the Rocklet is below the Sparken. If so, begin a Jump Attack Action (1).
                if (transform.position.y < sparken.transform.position.y - 1)
                {
                    action = 1;
                }
                //If not, then set the Rocklet to its Spin Speed (3) and Spin Action (3).
                else
                {
                    action = 3;
                    speed = 3;
                }
            }
            // If the Rocklet is not within spin range, set its speed to a walk (1) and Walk Action (2).
            else
            {
                speed = 1;
                action = 2;
            }
            // If the Rocklet is to the left of the Sparken, turn to the right (false) and walk in that direction
            if (transform.position.x < sparken.transform.position.x)
            {
                facing = false;
                velocitySet.x = speed;
            }
            // If the Rocklet is to the right of the Sparken, turn to the left (true) and walk in that direction
            else if (transform.position.x > sparken.transform.position.x)
            {
                facing = true;
                velocitySet.x = -speed;
            }
        }
        // If the Rocklet is not within range of the Sparken, set to Idle Action (0) and to face forward (0).
        else
        {
            animator.SetInteger("Rocklet Facing", 0);
            action = 0;
        }
        // Once calculations are complete, set the Rocklet's velocity.
        rb2d.velocity = velocitySet;
    }
    
    // Destroys the Rocklet
    public void RockletDie()
    {
        Destroy(gameObject);
    }

}
