using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the hitbox for the Aerial Thunderbeam Up
public class AerialThunderBeamUpScript : MonoBehaviour {

    public int damage; // Damage sender
    public object[] knockBackSenderEnemy; // Knockback packager
    public Vector2 knockBackEnemy; // Knockback direction
    public int knockBackTimerEnemy; // Knockback duration

    void Start()
    {
        damage = 4;

        knockBackEnemy = new Vector2(7, 5);
        knockBackTimerEnemy = 30;
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
        }
    }
}
