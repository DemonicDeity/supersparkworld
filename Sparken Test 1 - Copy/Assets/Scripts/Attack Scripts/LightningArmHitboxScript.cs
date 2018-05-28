using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the hitbox for the Lightning Arm Hitbox
public class LightningArmHitboxScript : MonoBehaviour {

    public int damage; // Damage sender
    public object[] knockBackSenderEnemy; // Knockback packager
    public Vector2 knockBackEnemy; // Knockback direction
    public int knockBackTimerEnemy; // Knockback duration

    void Start () {
        damage = 3;

        knockBackEnemy = new Vector2(3, 2);
        knockBackTimerEnemy = 10;
        knockBackSenderEnemy = new object[2];
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
