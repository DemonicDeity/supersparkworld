using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashHitboxScript : MonoBehaviour {

    public int damage; // Damage sent
    Rigidbody2D rb2d; // Sparken's rigidbody
    Vector2 stepForward; // Speed while performing the attack
    Vector2 stopStepForward; // Speed when Sparken stops movement
    public bool stepForwardTrigger; // Switch for speed during the attack

    object[] knockBackSenderEnemy; // Packager for enemy knockback
    Vector2 knockBackEnemy; // Enemy knockback direction
    int knockBackTimerEnemy; // Enemy knockback time

    // Use this for initialization
    void Awake () {
        damage = 1;
        rb2d = transform.parent.GetComponentInParent<Rigidbody2D>();
        stepForward = new Vector2(20, 0);
        stopStepForward = new Vector2(0, 0);
        stepForwardTrigger = false;

        knockBackEnemy = new Vector2(4, 2);
        knockBackTimerEnemy = 15;
        knockBackSenderEnemy = new object[2];

    }
	
    private void OnEnable()
    {
        stepForwardTrigger = true; // Turns on movement while hitbox is enabled
    }
    private void OnDisable()
    {
        stepForwardTrigger = false; // Turns off movement when hitbox is disabled

        // Resets velocity
        Vector2 v = rb2d.velocity;

        v.x = stopStepForward.x * transform.parent.parent.localScale.x;

        rb2d.velocity = v;
    }
    private void FixedUpdate()
    {
        // Changes velocity while stepforward is active
        if (stepForwardTrigger == true)
        {
            Vector2 v = rb2d.velocity;

            v.x = stepForward.x * transform.parent.parent.localScale.x;

            rb2d.velocity = v;
        }

    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            // If the hitbox connects, send damage, knockback, and knockback time
            coll.gameObject.SendMessage("applyDamage", damage, SendMessageOptions.DontRequireReceiver);
            knockBackSenderEnemy[0] = new Vector2(knockBackEnemy.x * transform.parent.parent.localScale.x, knockBackEnemy.y);
            knockBackSenderEnemy[1] = knockBackTimerEnemy;
            coll.gameObject.SendMessage("applyKnockBack", knockBackSenderEnemy, SendMessageOptions.DontRequireReceiver);

        }
    }

}
