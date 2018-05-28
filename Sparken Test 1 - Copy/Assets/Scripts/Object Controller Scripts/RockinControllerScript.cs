using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for controlling everything about the Rockin
public class RockinControllerScript : MonoBehaviour {

    public bool hostile; // Determines if Rockin is aggresive

    public int hitpoints; // Rockin Hitpoints
    Rigidbody2D rb2d; // Rockin rigidbody2d
    int knockBackTimer; // Time of being stuck in Knockback animation
    Vector2 knockBack; // Knockback direction
    Vector2 velocitySet; // Intermediary velocity to avoid defining new Vector2s in loops
    bool grounded; // Whether the Rockin is on the ground or not
    GameObject sparken; // The player character GameObject, Sparken
    GameObject camera; // The camera GameObject
    public int action; // Current action of the Rockin

    Animator animator; // The Rocklet Animator
    //int jumpTimer; // Timer of being in the jump animation
    Vector2 jumpV; //jump velocity

    public bool facing; // Facing false = right, true = left
    Vector2 faceRight; // Defines the value for facing right
    Vector2 faceLeft; // Defines the value for facing left

    int speed; // Current top speed of the Rockin - Speed 1 = walk, Speed 3 = spin

    int detectRange; // Range at which the Rockin will target the player
    Vector3 forwardPunchRangeMin; // Minimum trigger range for Rockin Forward Punch
    Vector3 forwardPunchRangeMax; // Maximum trigger range for Rockin Forward Punch

    Vector3 aerialSmashDownRangeMin; // Minimum trigger range for Rockin Smash Down
    Vector3 aerialSmashDownRangeMax; // Maximum trigger range for Rockin Smash Down

    Vector3 aerialBackStrikeRangeMin; // Minimum trigger range for Rockin Back Strike
    Vector3 aerialBackStrikeRangeMax; // Maximum trigger range for Rockin Back Strike

    bool alive; // Whether the Rockin is Alive or not

    int timeBetweenActions; // Length of time between random actions

    int timeBetweenGuards; // Length of time between Guard actions
    int resetGuardTime; // Reset time for guard actions
    bool guarding; // Determine if Rockin is guarding

    void Start()
    {
        hitpoints = 10;
        knockBackTimer = 0;
        knockBack = new Vector2(0, 0);
        rb2d = GetComponent<Rigidbody2D>();
        sparken = GameObject.FindGameObjectWithTag("Player");
        camera = GameObject.Find("Main Camera");
        animator = GetComponent<Animator>();
        jumpV = new Vector2(0, 5);

        faceRight = new Vector3(1, 1, 1);
        faceLeft = new Vector3(-1, 1, 1);

        speed = 1;

        detectRange = 7;
        forwardPunchRangeMin = new Vector3(-0.5f, -0.4f, 0);
        forwardPunchRangeMax = new Vector3(2, 0.4f, 0);

        aerialSmashDownRangeMin = new Vector3(0, -0.4f, 0);
        aerialSmashDownRangeMax = new Vector3(1, 0.4f, 0);

        aerialBackStrikeRangeMin = new Vector3(-2f, -0.2f, 0);
        aerialBackStrikeRangeMax = new Vector3(0, 0.2f, 0);

        timeBetweenActions = 100;

        timeBetweenGuards = 100;
        resetGuardTime = 200;
        guarding = false;

        alive = true;
    }

    void Update()
    {
        conditionCheck(); // Checks to see if the Rockin is alive every frame

    }

    private void FixedUpdate()
    {
        // Rockin moves and attacks if it is alive
        if (alive == true)
        {
            actionTimeUpdate();

            rockinAIAction();

            setFacingDirection();
        }

        // Rockin is destroyed if it is dead and on the ground
        else if (alive == false && grounded == true)
        {
            animator.SetBool("Alive", false);
            knockBackTimer = 0;
            animator.SetInteger("Hurt Timer", knockBackTimer);
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

    // Applies damage to the Rockin's hitpoints
    public void applyDamage(int damage)
    {
        if (hostile == true)
        {
            // If Rockin can guard, damage is not applied
            if (!guardCheck())
            {
                hitpoints = hitpoints - damage;
            }
        }
    }

    // Applies knockback to the Rockin, setting knockback timer and direction
    public void applyKnockBack(object[] knockBackReciever)
    {
        if (hostile == true)
        {
            // If Rockin can guard, knockback is not applied
            if (!guardCheck())
            {
                knockBack = (Vector2)knockBackReciever[0];
                knockBackTimer = (int)knockBackReciever[1];
                rb2d.velocity = knockBack;
            }
        }
    }

    // Checks to see if the Rockin still has health
    private void conditionCheck()
    {
        if (hitpoints <= 0 && grounded == true)
        {
            rb2d.velocity = new Vector2(0, 0);
            alive = false;
        }
    }

    // AI for the Rockin
    public void rockinAIAction()
    {
        action = animator.GetInteger("Action");

        // If knockbacktimer is greater than 0, apply knockback and do not act
        if (knockBackTimer > 0)
        {
            rockinKnockBack();
        }
        else
        {
            // If Rockin detects a player, begin to move and attack
            if (rockinRangeDetect() && hostile == true)
            {
                rockinMovement();
                if (grounded == true)
                {
                    rockinGroundedAttack();
                }
                else if (grounded == false)
                {
                    rockinAerialAttack();
                }
            }
        }
        animator.SetInteger("Action", action);
    }

    // Updates the Rockin's jump, jump timer, and jump hitbox
    public void rockinJump(int force)
    {
        Vector2 jump = new Vector2(0, force);
        rb2d.AddForce(jump);
    }

    // Sets the Rockin to its knockback direction and reduces the knockback timer
    private void rockinKnockBack()
    {
        // Determines rotation for knockback
        double hurtRotation = (Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) / Mathf.PI) * 180.0;

        // Converts rotation to a v3
        Vector3 hurtRotationReal = new Vector3(1, 1, (float)hurtRotation - 90);

        // Applies rotation to Rockin
        transform.rotation = Quaternion.Euler(hurtRotationReal);

        knockBackTimer--;

        if (knockBackTimer == 0)
        {
            // Reset Rockin to correct rotation
            transform.rotation = Quaternion.Euler(1, 1, 1);
        }
        animator.SetInteger("Hurt Timer", knockBackTimer);
    }

    // Determines if Sparken is in range and turns to face him
    bool rockinRangeDetect()
    {
        if (Mathf.Abs(transform.position.x - sparken.transform.position.x) < detectRange)
        {
            if (grounded != true)
            {
                animator.SetInteger("Idle State", 3); // Sets Rockin to rising
            }
            else
            {
                animator.SetInteger("Idle State", 1); // Sets Rockin to look to the side
            }
            return true;
        }
        else
        {
            animator.SetInteger("Idle State", 0); // Sets Rockin to look ahead
            action = 0;
            return false;
        }
    }

    // AI Controller for determining Grounded Attack
    private void rockinGroundedAttack()
    {
            animator.SetInteger("Idle State", 1);

        // If Rockin is prepared to perform a special action
        if (timeBetweenActions == 0 && action < 3 && grounded == true)
        {
            timeBetweenActions = (Random.Range(0, 100) + 100); // Resets special action cooldown
            int actionDecider = Random.Range(0, 2); // Randomly determine an attack to make
            if (actionDecider == 0)
            {
                action = 1; // Rockin jumps
            }
            else if (actionDecider == 1)
            {
                action = 4; // Rockin performs seismic stomp
            }
            else
            {
            }
        }
            //  Check if the Sparkin is in range of Forward Punch
            else if (((transform.position.x - sparken.transform.position.x) * -transform.localScale.x) < (forwardPunchRangeMax.x) && ((transform.position.x - sparken.transform.position.x) * -transform.localScale.x) > (forwardPunchRangeMin.x) &&
                (transform.position.y + forwardPunchRangeMax.y) > sparken.transform.position.y && (transform.position.y + forwardPunchRangeMin.y) < sparken.transform.position.y)
            {
                action = 3;
            }
            // If the Rockin is not within range, have it set to walk toward the player
            else if (action == 0 || action == 2)
            {
                speed = 1;
                action = 2; // walking
            }
    }

    // AI Controller for making attacks in the air
    void rockinAerialAttack()
    {
        // Really long calculation for determining if Sparken is in range of a Smash Down
        if (((transform.position.x - sparken.transform.position.x) * -transform.localScale.x) < (aerialSmashDownRangeMax.x) && ((transform.position.x - sparken.transform.position.x) * -transform.localScale.x) > (aerialSmashDownRangeMin.x) &&
                (transform.position.y + aerialSmashDownRangeMax.y) > sparken.transform.position.y && (transform.position.y + aerialSmashDownRangeMin.y) < sparken.transform.position.y)
        {
            action = 5; // Set to Smash Down
        }
        // Another long calculation for determining if Sparken is in range of Back Strike
        else if (((transform.position.x - sparken.transform.position.x) * -transform.localScale.x) < (aerialBackStrikeRangeMax.x) && ((transform.position.x - sparken.transform.position.x) * -transform.localScale.x) > (aerialBackStrikeRangeMin.x) &&
                (transform.position.y + aerialBackStrikeRangeMax.y) > sparken.transform.position.y && (transform.position.y + aerialBackStrikeRangeMin.y) < sparken.transform.position.y)
        {
            action = 6; // Set to Back Strike
        }
    }

    // AI Controller for movement
    void rockinMovement()
    {
        velocitySet = rb2d.velocity;

        velocitySet.x = 0;

        // If Rockin isn't locked in an Attack
        if (action < 3)
        {
            // Next conditionals determine position of Sparken and turn to move and face him
            if (transform.position.x < sparken.transform.position.x)
            {
                if (grounded == true)
                    facing = false;

                velocitySet.x = speed;
            }
            else if (transform.position.x > sparken.transform.position.x)
            {
                if (grounded == true)
                    facing = true;
                velocitySet.x = -speed;
            }
        }
        if (grounded == true)
        {
            rb2d.velocity = velocitySet;
        }
        else if (checkMovable(velocitySet.x))
        {
            velocitySet.y = 0;
            rb2d.AddForce(velocitySet);
        }
    }

    // Checks to see if Rockin is guarding
    bool guardCheck()
    {
        if (timeBetweenGuards == 0 && grounded == true)
        {
            guarding = true;
            timeBetweenGuards = resetGuardTime;
            animator.SetInteger("Action", 8);
        }

        return guarding;
    }

    // Determines which direction the Rockin should turn to face
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

    // Just counts down time between actions
    void actionTimeUpdate()
    {
        if (timeBetweenActions > 0)
        {
            timeBetweenActions--;
        }
        if (timeBetweenGuards > 0)
        {
            guarding = false;
            timeBetweenGuards--;
        }
    }

    // Checks to see if Rockin is movable
    bool checkMovable(float moveHorizontal)
    {
        return (Mathf.Abs(rb2d.velocity.x) < Mathf.Abs(moveHorizontal * speed) || rb2d.velocity.x > speed && moveHorizontal < 0 || rb2d.velocity.x < -speed && moveHorizontal > 0);
    }

    // Sets screen to shake
    public void quakeFootTrigger()
    {
        camera.SendMessage("setShakeDuration", 60, SendMessageOptions.DontRequireReceiver);
    }

    // Will destroy Rockin in a more dramatic way. Currently unused.
    public void RockinDie()
    {
        Destroy(gameObject);
    }
}
