using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Basic Function: Controls Animation, Actions, and Player Inputs
// Long Function: This is pretty complicated. You'll have to read through all of it to really figure out what it does
public class AnimationTestScript : MonoBehaviour {


    private Animator animator; // Gets the animator
    Rigidbody2D rb2; // Gets the rigidbody
    private PlayerController playercontroller; // Gets the PlayerController
    private SpriteRenderer sprite; // Gets the sprite
    private Vector3 faceRight; // V3 for facing right
    private Vector3 faceLeft; // V3 for facing left
    private DialogueManager dialogueManager; // Gets the dialoguemanager
    private FindClosestScript findClosestScript; // Creates a FindClosestScript
    private SparkenDialogueScript sparkenDialogueScript; // Creates a SparkenDialogueScript
    private ProjectileController projectileController; // Gets the projectilecontroller
    public bool facing; // Sets the facing direction
    public int moving; // Sets whether or not Sparken is moving
    private float doubleTapTime; // Time minimum for double taps
    private string lastButtonPressed; // Holds the last button pressed
    private float lastTapTime; // Holds the last time a button was pressed
    public bool doubletap; // Boolean to determine if there's been a doubletap
    public string lastDirectionPressed; // Holds the last directional input
    public bool hasZipped; // Boolean to determine if the Sparken has a Zip or not
    private int action; // Gets and sets the Sparken's action
    private int idleState; // Gets and sets the Sparken's idle state
    private bool grounded; // Gets the grounded condition



    // Use this for initialization


    void Start () {
        animator = GetComponent<Animator>();
        rb2 = GetComponent<Rigidbody2D>();
        facing = false;
        moving = 0;
        doubleTapTime = 0.2f;
        hasZipped = false;

        playercontroller = GetComponent<PlayerController>();
        projectileController = GetComponent<ProjectileController>();
        sprite = GetComponent<SpriteRenderer>();
        faceRight = new Vector3(1, 1, 1);
        faceLeft = new Vector3(-1, 1, 1);
        dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();
        findClosestScript = new FindClosestScript();
        sparkenDialogueScript = new SparkenDialogueScript();
    }
	
	// Update is called once per frame
	public void animationUpdate () {

        action = animator.GetInteger("Action"); // Gets the current action
        idleState = animator.GetInteger("Idle State"); // Gets the current Idle State

        grounded = playercontroller.grounded; // Gets the current grounded state
        
        // Performs all actions
        doubleTapDecider();
        movingDecider();
        keyDownDecider();
        keyUpDecider();
        facingDecider();

        animator.SetInteger("Action", action); // Sets the action
        animator.SetInteger("Idle State", idleState); // Sets the idle state
    }

    // Function to determine if the player has doubletapped a button
    private void doubleTapDecider()
    {
        // Called on any keydown
        if (Input.anyKeyDown)
        {
            // Compares current button to last pressed and determines if timeframe is correct for a doubletap
            if (lastButtonPressed == Input.inputString && Time.time - lastTapTime < doubleTapTime)
            {
                doubletap = true;
            }
            else
            {
                doubletap = false;
            }
            // Sets the last button pressed stats
            lastButtonPressed = Input.inputString;
            lastTapTime = Time.time;
        }
    }

    //Determines if Sparken is moving or not
    private void movingDecider()
    {
        // If right or left are being pressed, then the character is moving
        if (Input.GetKey("right") || Input.GetKey("left"))
        {
            moving = 1;
        }
        else
        {
            moving = 0;
        }

        // If the character is moving on the ground, but stuck in an idle animation, they are set to a walking animation
        if (moving == 1 && action == 0 && grounded == true)
        {
            action = 1;
            playercontroller.speed = 2;
        }
        // If the character is not moving, but they are playing a run animation, stop animation
        else if (moving == 0 && action == 1 && grounded == true)
        {
            action = 0;
        }
        // If the character is running in the air, they are set to stop running in the air
        else if (action == 1 && grounded == false)
        {
            action = 0;
        }
    }

    // On a keydown press, determine what action Sparken will take depending on key press
    private void keyDownDecider()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded == true)
            {
                // If the character is standing still or running, allow them to jump
                if (action == 0 || action == 1)
                {
                    action = 3; // Jump
                }
                // If the character is jumping, allow them to begin a boost jump
                else if (action == 3)
                {
                    action = 4; // Boost Jump
                }
            }
            // If the character is airbound and hasn't zipped yet, allow them to zip
            else if (grounded == false && hasZipped == false)
            {
                hasZipped = true; // Sparken cannot zip twice in the air
                action = 16; // Zip
            }
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            lastDirectionPressed = "Down"; // Sets directional input
            // Set the character to face forward if they are facing sideways on the ground
            if (grounded == true && idleState != 0 && action == 0)
            {
                idleState = 0; // Forward facing
            }
            // Sets the character to crouch if they are facing forward on the ground
            else if (idleState == 0 && grounded == true)
            {
                idleState = 4; // Crouch
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            lastDirectionPressed = "Up"; // Sets directional input
            // Sets the character to rising slash if they are slashing
            if (action == 9)
            {
                action = 13; // Rising slash
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            lastDirectionPressed = "Right"; // Sets directional input
            idleState = 1; // Sets the character to look to the side
            facing = false; // Sets the character to look to the right
            // If character is on the ground doing nothing, set it to walk
            if (grounded == true && action == 0 && doubletap == false)
            {
                action = 1; // Walk
                playercontroller.speed = 2; // Walk speed
            }
            // If the character is on the ground walking, set it to dash
            else if (grounded == true && action == 1 && doubletap == true || action == 2)
            {
                action = 2; // Dash
                playercontroller.speed = 5; // Dash speed
            }
            // If the character is slashing, set them to Slash Through
            else if (action == 9)
            {
                action = 15; // Slash Through
            }

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            lastDirectionPressed = "Left"; // Sets directional input
            idleState = 1; // Sets the character to look to the side
            facing = true; // Sets the character to look to the left
            // If character is on the ground doing nothing, set it to walk
            if (grounded == true && action == 0 && doubletap == false)
            {
                action = 1; // Walk
                playercontroller.speed = 2; // Walk speed
            }
            // If the character is on the ground walking, set it to dash
            else if (grounded == true && action == 1 && doubletap == true || action == 2)
            {
                action = 2; // Dash
                playercontroller.speed = 5; // Dash speed
            }
            // If the character is slashing, set them to Slash Through
            else if (action == 9)
            {
                action = 15; // Slash Through
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            if (grounded == true)
            {
                // If the last direction was down without an action, set them to building the Lightning Conductor
                if (lastDirectionPressed == "Down" && action < 2)
                {
                    action = 19; // Building
                    projectileController.createLightningConductor(); // Begins to construct a Lightning Conductor
                }
                // If the character is facing forward without an action, set them to Spark Shine
                if (idleState == 0 && action < 2)
                {
                    action = 6; // Spark Shine
                }
                // If the character is facing to the side without an action, set them to Thunder Touch
                else if (idleState == 1 && action < 2)
                {
                    action = 7; // Thunder Touch
                }
                // If the character is dashing, set them to Dash Attack
                else if (action == 2)
                {
                    action = 5; // Dash Attack
                }
                // If the character is walking, set them to Shock Saw (Turn off because it's ugly)
                /*else if (action == 1)
                {
                    action = 8; // Shock Saw
                }*/

            }
            else
            {
                // If the character is in the air, set them to Thunderball Drop
                action = 13; // Thunderball Drop
            }
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (grounded == true)
            {
                // If the character is running and the last direction pressed was down, perform a Heel Slide
                if (action == 1 && lastDirectionPressed == "Down")
                {
                    action = 20; // Heel Slide
                }
                // If the character is facing forward without an action, set them to Slash Startup
                else if (idleState == 1 && action < 2)
                {
                    action = 9; // Slash Startup
                }
                // If the character is in Slash Startup, set them to Thunderarm
                else if (action == 9)
                {
                    action = 11; // Thunderarm
                }
            }
            else
            {
                // If the character is in the air, set them to flip slash
                action = 12; // Flip Slash
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (grounded == true)
            {
                // If the last direction pressed was up without an action, set to Overhead Shine
                if (lastDirectionPressed == "Up" && action < 2)
                {
                    action = 18; // Overhead Shine
                }
                // If the last direction pressed was down without an action, set to Electropulse
                else if (lastDirectionPressed == "Down" && action < 2)
                {
                    action = 21; // Electropulse
                }
                // If the character is facing the side without an action, set them to Lightning Eyes
                else if (idleState == 1 && action < 2)
                {
                    action = 10; // Lightning Eyes
                }
            }
            else
            {
                // If the character is in the air, set them to Aerial Lightning Cannon
                action = 14; // Aerial Lightning Cannon
            }
        }
    }

    // Controls when a key is released and determines Action
    private void keyUpDecider()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (grounded == true)
            {
                // If the character has finished charging a Boost Jump, jump very far upward
                if (action == 4 && animator.GetCurrentAnimatorStateInfo(0).IsName("Boosting Jump Right"))
                {
                    playercontroller.JumpForce(10); // Calls jumpforce to propel upward
                    action = 0; // Resets action
                }
                // If the character has not yet finished charging, jump very short upward
                else if (action == 4)
                {
                    action = 0; // Resets action
                    playercontroller.JumpForce(3); // Calls jumpforce to propel upward 
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            lastDirectionPressedReset(); // Resets last directional
            // If the character is crouching, stop crouching
            if (idleState == 4)
            {
                idleState = 0; // Neutral facing
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            lastDirectionPressedReset(); // Resets last directional
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            lastDirectionPressed = null; // Resets last directional
            // If the character is running or dashing, stop moving
            if (moving == 0 && action == 1 || moving == 0 && action == 2) {
                action = 0;
                playercontroller.speed = 2;
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            lastDirectionPressedReset(); // Resets last directional
            // If the character is running or dashing, stop moving
            if (moving == 0 && action == 1 || moving == 0 && action == 2)
            {
                action = 0;
                playercontroller.speed = 2;
            }
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            // If the last action was building, stop building.
            if (action == 19)
            {
                action = 0;
            }
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            // If the last action was Lightning Eyes, stop Lightning Eyes
            if (action == 10)
            {
                action = 0;
            }
        }
    }

    // Sets the character to the direction they're supposed to face
    private void facingDecider()
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

    // Checks to see if any direction key is being pressed down
    void lastDirectionPressedReset()
    {
        if (!Input.GetKey("up") && !Input.GetKey("down") && !Input.GetKey("right") && !Input.GetKey("left"))
        {
            lastDirectionPressed = null;
        }
    }

}