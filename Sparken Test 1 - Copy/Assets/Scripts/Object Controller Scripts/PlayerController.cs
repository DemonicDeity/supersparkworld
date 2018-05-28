using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

// A script used to control the player's movements, positions, knockback, hitpoints, cutscenes, and damage
public class PlayerController : MonoBehaviour
{

    public float speed; // Stores the player's maximum speed
    // At the current time, the Sparken has two speeds: 2 = Walking, 4 = Dashing
    private float fall; // Currently unused
    private bool direction; // Currently unused
    private int action; // The action that the Sparken is taking
    // I've attached a document which details the numbers associated with each action
    public int hitpoints; // The Sparken's current hitpoints
    public int hitpointMaximum; // The Sparken's maximum hitpoints
    private bool alive; // Whether the Sparken is alive or not
    bool controllable; // Whether the player can control Sparken 
    bool conversationable; // Whether Sparken can enter a conversation

    private Rigidbody2D rb2d; // Gets the Sparken's rigidbody
    private Animator animator; // Gets the Sparken's animator
    private AnimationTestScript animationtestscript; // Gets the Sparken's animationtestscript
    private DialogueManager dialogueManager; // Gets the Dialogue Manager
    private SparkenDialogueScript sparkenDialogueScript; // Creates a Sparken Dialogue Script
    private FindClosestScript findClosestScript; // Creates a findClosestScript
    public bool grounded; // Whether the Sparken is on the ground or not
    public Vector3 lastLanded; // Keeps the last place that the Sparken landed on the ground

    private Vector3 min, max; // Gets the bounds for the current map
    private Renderer rend; // Gets the renderer for the current map
    GameObject currentMap; // Gets the current map

    int knockBackTimer; // Sets the time the Sparken is stuck in knockback
    Vector2 knockBack; // Sets the direction that the Sparken is knocked back
    Vector2 v; // Sets the Sparken's velocity

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animationtestscript = GetComponent<AnimationTestScript>();
        dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();
        sparkenDialogueScript = new SparkenDialogueScript();
        findClosestScript = new FindClosestScript();

        currentMap = GameObject.FindGameObjectWithTag("Background");

        fall = 0;
        grounded = false;
        speed = 2;
        hitpoints = 5;
        hitpointMaximum = 8;

        knockBackTimer = 0;
        knockBack = new Vector2(0, 0);

        alive = true;
        controllable = true;
        conversationable = true;

        rend = currentMap.GetComponent<Renderer>();
        min = rend.bounds.min;
        max = rend.bounds.max;

        lastLanded = transform.position;

        // If the Sparken died, run from death script
        if (GameControllerScript.gameController.getWalkedOrDied() == false)
        {
            loadDeathPosition();
        }
        // If the Sparken is spawning on a new map
        else if (GameControllerScript.gameController.getSparkenPlace() != new Vector3(0, 0, 0))
        {
            loadPosition(GameControllerScript.gameController.getSparkenPlace());
            hitpoints = GameControllerScript.gameController.getCurrentHealth();
        }
        // If the Sparken has no declared spawn location, default to last landed
        else
        {
            godlyDescentTrigger(lastLanded);
        }

    }

    private void Update()
    {
        // If the Sparken can speak, run dialogue capabilities
        if (conversationable == true)
        {
            dialogueDecider();
        }
        // If the Sparken can act, run controllable capabilities
        if (controllable == true)
        {
            animationtestscript.animationUpdate();
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal"); // Gets the input from the arrow keys horizontally

        float moveVertical = Input.GetAxis("Vertical"); // Gets the input from the arrow keys vertically

        action = animator.GetInteger("Action"); // Gets the current action from the animator

        fallingController(); // Calls to update the Sparken's current idle position

        if (controllable == true)
        {
            // Sparken will run its dying animation upon touching the ground without hitpoints
            if (alive == false && grounded == true && knockBackTimer <= 0)
            {
                animator.SetInteger("Died", 1);
                alive = true;
                gameObject.layer = 9; // Sparken becomes invisible to stop enemies knocking him around while he's dying
                setCutscene(false); // Sets the death cutscene
            }
            // Script for normal movement on the ground. Player loses control over character when they perform actions
            else if (knockBackTimer <= 0 && grounded == true && action <= 2)
            {
                // Another note. There is no momentum on the ground, so the Sparken is set to a static velocity

                v = rb2d.velocity;

                v.x = moveHorizontal * speed;

                rb2d.velocity = v;

            }
            // Script for movement in the air. Same rule applies to actions
            else if (knockBackTimer <= 0 && grounded == false && action <= 2)
            {

                // In the air, movement is calculated using force to create a fluid movement.

                v.y = 0;
                v.x = 0;
                // checkMovable is detailed below, but the general principle is that it makes sure the player can't go over a set movement speed in the air
                if (checkMovable(moveHorizontal))
                {
                    v.x = moveHorizontal * speed;
                }

                rb2d.AddForce(v);

                rb2d.gravityScale = 1;

            }
            // If the player is being knocked back
            else if (knockBackTimer > 0)
            {

                // hurtRotation gets the rotation that the Player is being knocked back in using an arctangent
                double hurtRotation = (Mathf.Atan2(rb2d.velocity.y, rb2d.velocity.x) / Mathf.PI) * 180.0;

                // Uses hurtRotation to create a V3 which can be applied to the object. Because of Unity space, it has to be subtracted by 90.
                Vector3 hurtRotationReal = new Vector3(1, 1, (float)hurtRotation - 90);

                // Applies the hurtRotationReal
                transform.rotation = Quaternion.Euler(hurtRotationReal);

                knockBackTimer--;

                if (knockBackTimer <= 0)
                {
                    transform.rotation = Quaternion.Euler(1, 1, 1); // Resets rotation
                }

            }

            direction = animationtestscript.facing; // Currently useless. Facing is determined in AnimationTestScript
            animator.SetInteger("Hurt Time", knockBackTimer); // Sets Hurt Time


        }

        // Action 17 is Godly Descent. This will continue the animation.
        else if (action == 17)
        {
            godlyDescentContinue();
        }

        // If the Sparken has fallen below the Death Level, 
        if (transform.position.y < GameControllerScript.gameController.deathLevel)
        {
            fallBack();
        }
    }

    void OnCollisionEnter2D(Collision2D hit)
    {
        // Simple controller. Will set if the character is grounded or not. Sets the zip potential back up. Also sets the last landing spot.
        if (hit.gameObject.tag == "Ground")
        {
            grounded = true;
            animationtestscript.hasZipped = false;
            lastLanded = new Vector3(transform.position.x, transform.position.y+0.5f, transform.position.z);
        }

    }

    private void OnCollisionStay2D(Collision2D hit)
    {
        // Here as a placeholder in case something needs to happen. Currently useless
        if (hit.gameObject.tag == "Ground")
        {
        }
    }

    // Detect collision exit with floor
    void OnCollisionExit2D(Collision2D hit)
    {
        if (hit.gameObject.tag == "Ground")
        {
            grounded = false;
        }
    }

    // Jump Force applies a velocity shift upward. Used by jumping mainly.
    public void JumpForce (int force)
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, force);
    }

    // Apply Damage does just that: apply damage. It also updates the current hitpoints on the HP Tracker UI and will set the Sparken dead if they are at 0 hp.
    public void applyDamage(int damage)
    {
        hitpoints = hitpoints - damage;
        GameObject.Find("Sparken Hitpoints").SendMessage("hitPointUpdate", SendMessageOptions.DontRequireReceiver); // Sends data to the Sparken Hitpoints UI
        if (hitpoints <= 0)
        {
            alive = false;
        }
    }

    // Apply Knock Back applies knock back, and sets the knockbacktimer
    public void applyKnockBack(object[] knockBackReciever)
    {
        knockBack = (Vector2)knockBackReciever[0];
        knockBackTimer = (int)knockBackReciever[1];
        rb2d.velocity = knockBack;
    }

    // Apply Self Knockback knocks the Sparken in a direction without applying damage or knockbacktimer. Usually called by the Sparken's own attacks.
    public void applySelfKnockBack(Vector2 knockBackReciever)
    {
        knockBack = knockBackReciever;
        rb2d.velocity = knockBack;
    }

    // Controls the idlestate of the Sparken.
    void fallingController()
    {
        // If the Sparken is rising in the air
        if (rb2d.velocity.y < 0 && grounded == false)
        {
            animator.SetInteger("Idle State", 2);
        }
        // If the Sparken is falling
        else if (rb2d.velocity.y > 0 && grounded == false)
        {
            animator.SetInteger("Idle State", 3);
        }
        // If the Sparken is standing
        else
        {
            if (animator.GetInteger("Idle State") != 0 && animator.GetInteger("Idle State") != 4)
                animator.SetInteger("Idle State", 1);
        }
    }

    // Function called when the Sparken falls below the deathzone. Applies damage, then sets them back to the last landed with a Godly Descent. Also has a unique death animation.
    void fallBack()
    {
        rb2d.velocity = new Vector2(0, 0);
        applyDamage(1);
        if (alive == true)
        {
            godlyDescentTrigger(lastLanded);
        }
        // Plays unique death animation
        else if (alive == false)
        {
            setDied(4);
            gameObject.layer = 9;
            rb2d.gravityScale = 0;
            controllable = false;
            loadPosition(new Vector3(transform.position.x, GameControllerScript.gameController.deathLevel+0.4f, transform.position.z));
        }
    }

    // Checks the player's input and compares it the velocity of the Sparken. If the input would put the Sparken over the velocity limit dictated by speed, stop input.
    // Works in both directions!
    bool checkMovable(float moveHorizontal)
    {
        return (Mathf.Abs(rb2d.velocity.x) < Mathf.Abs(moveHorizontal * speed) || rb2d.velocity.x > speed && moveHorizontal < 0 || rb2d.velocity.x < -speed && moveHorizontal > 0);
    }

    // Just resets an action to 0 in the animator
    public void resetActionAtEnd()
    {
        animator.SetInteger("Action", 0);
    }

    // Sets the character dead in the animator
    public void setDied(int died)
    {
        animator.SetInteger("Died", died);
    }

    // This function controls the dialogue of the character. More detail below.
    private void dialogueDecider()
    {
        // R is the conversation update button
        if (Input.GetKeyDown(KeyCode.R))
        {
            // If the player is already in a cutscene, update the cutscene
            if (dialogueManager.inCutScene())
            {
                dialogueManager.displayNextCutsceneSentence();
            }
            // If the dialogue manager doesn't have a conversation queued yet
            else if (!dialogueManager.sentenceQueued())
            {
                // If there is a character within range, begin a conversation with it
                if (findClosestScript.GetClosestObject(gameObject, "Enemy", 3) != null)
                {
                    dialogueManager.startDialogue(findClosestScript.GetClosestObject(gameObject, "Enemy", 3).GetComponent<DialoguesScript>());
                }
                // If there is not a character within range, have Sparken talk to himself
                else
                {
                    dialogueManager.startDialogue(sparkenDialogueScript.getRemarks());
                }
            }
            // If the dialogue manager has a conversation queued that isn't a cutscene
            else
            {
                dialogueManager.displayNextSentence();
            }
        }
    }

    // Gets the death dialogue from the dialogue script
    public void setDeadDialogue ()
    {
        dialogueManager.startDialogue(sparkenDialogueScript.getDeathRemarks());
    }

    // This function is called when a conversation stops. Potentional for much more, but just currently kills Sparken when he finishes his death lines.
    public void checkOnDialogueEnd()
    {
        if (animator.GetInteger("Died") == 2)
        {
            animator.SetInteger("Died", 3);
        }

    }

    // A function which prepares the gamecontroller to reload the scene when Sparken dies.
    public void reloadAfterDeath()
    {
        GameControllerScript.gameController.setWalkedOrDied(false); // When the scene is loaded, sparken will spawn from death location
        GameControllerScript.gameController.loadFromCheckPoint(); // Load the scene from the last checkpoint
    }

    // Gets the position of the last checkpoint and sets Sparken's position to it
    public void loadDeathPosition()
    {
        transform.position = (Vector3)GameControllerScript.gameController.savePointController.getSavePoints(GameControllerScript.gameController.getLastCheckpoint())[1];
    }

    // Sets Sparken to a new position
    public void loadPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    // Triggers a Godly Descent. Sets correct action, sets controllable, and sets the player to be intangible.
    // Then moves the Sparken to the correct position and drops him from the top of the screen.
    public void godlyDescentTrigger(Vector3 loadingPlace)
    {
        animator.SetInteger("Action", 17);
        controllable = false;
        conversationable = false;
        gameObject.layer = 8;
        loadPosition(new Vector3(loadingPlace.x, max.y, loadingPlace.z));
    }

    // Continues a Godly Descent. Moves the Sparken's position until he is just above the desired location.
    public void godlyDescentContinue()
    {
        // The Sparken falls until he is a bit over the lastLanded position
        if (transform.position.y > lastLanded.y+0.2f)
        {
            rb2d.velocity = new Vector2(0, -15);
        }
        // When the Sparken reaches the lastLanded position
        else
        {
            gameObject.layer = 11; // Sets Sparken to the character layer
            rb2d.velocity = new Vector2(0, 0); // Resets velocity
            animator.SetInteger("Action", 0); // Resets action
            controllable = true; // Resets control
            conversationable = true; // Resets conversation
        }
    }

    // Sets the variables to a cutscene. 
    public void setCutscene(bool incomingControllable)
    {
        controllable = incomingControllable;
        animator.SetInteger("Action", 0);
        rb2d.velocity = new Vector2(0, 0);
    }

    // Sets the variables to a pause
    public void setPaused(bool incomingControllable)
    {
        controllable = incomingControllable;
        conversationable = incomingControllable;
    }

}