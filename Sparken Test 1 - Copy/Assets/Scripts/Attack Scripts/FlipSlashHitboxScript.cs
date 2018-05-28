using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the hitbox for the Flip Slash
public class FlipSlashHitboxScript : MonoBehaviour {

    public int damage; // Damage sender
    public object[] knockBackSenderEnemy; // Knockback packager
    public Vector2 knockBackEnemy; // Knockback direction
    public int knockBackTimerEnemy; // Knockback duration
    Vector2 knockBackSelf; // Knockback direction for self

    void Start () {
        damage = 2;

        knockBackSelf = new Vector2(0, 6);

        knockBackEnemy = new Vector2(0, -3);
        knockBackTimerEnemy = 10;
        knockBackSenderEnemy = new object[2];


    }

    private void OnTriggerEnter2D(Collider2D coll)
    {

        if (coll.gameObject.tag == "Enemy")
        {
            // If the hitbox connects, send damage, knockback, and knockback time
            knockBackSenderEnemy[0] = new Vector2(knockBackEnemy.x * transform.parent.parent.localScale.x, knockBackEnemy.y);
            knockBackSenderEnemy[1] = knockBackTimerEnemy;
            coll.gameObject.SendMessage("applyKnockBack", knockBackSenderEnemy, SendMessageOptions.DontRequireReceiver);
            coll.gameObject.SendMessage("applyDamage", damage, SendMessageOptions.DontRequireReceiver);
            
            knockBackSelf = new Vector2(knockBackSelf.x * transform.parent.parent.localScale.x, knockBackSelf.y);

            transform.parent.parent.SendMessage("applySelfKnockBack", knockBackSelf, SendMessageOptions.DontRequireReceiver);
        }
    }

}
