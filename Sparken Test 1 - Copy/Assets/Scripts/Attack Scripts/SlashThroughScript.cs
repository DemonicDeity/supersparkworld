using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the hitbox for the Slash Through
public class SlashThroughScript : MonoBehaviour {

    public int damage; // Damage sent
    Rigidbody2D rb2d; // Sparken Rigidbody
    Vector2 stepForward; // Speed while hitbox is active
    public bool stepForwardTrigger; // Switch for turning on speed

    private Animator animator; // Sparken animator

    void Awake()
    {
        damage = 1;
        rb2d = transform.parent.GetComponentInParent<Rigidbody2D>();
        stepForward = new Vector2(20, 0);
        stepForwardTrigger = false;

        animator = transform.parent.parent.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        stepForwardTrigger = true; // Sets speed on while hitbox is active
        transform.parent.parent.gameObject.layer = 9; // Sets layer to invisible while hitbox is active
    }
    private void OnDisable()
    {
        stepForwardTrigger = false; // Sets speed off while hitbox is inactive
        transform.parent.parent.gameObject.layer = 11; // Sets layer to character while hitbox is inactive

        // Resets velocity
        Vector2 v = rb2d.velocity;

        v.x = 0 * transform.parent.parent.localScale.x;

        rb2d.velocity = v;
    }
    private void FixedUpdate()
    {
        // Disables speed if action is exited
        if (animator.GetInteger("Action") != 15)
        {
            OnDisable();
        }
        // Changes velocity while stepforward is active
        if (stepForwardTrigger == true)
        {

            Vector2 v = rb2d.velocity;

            v.x = stepForward.x * transform.parent.parent.localScale.x;
            v.y = stepForward.y;

            rb2d.velocity = v;
        }

    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            // If the hitbox connects, send damage
            coll.gameObject.SendMessage("applyDamage", damage, SendMessageOptions.DontRequireReceiver);

        }
    }
}
