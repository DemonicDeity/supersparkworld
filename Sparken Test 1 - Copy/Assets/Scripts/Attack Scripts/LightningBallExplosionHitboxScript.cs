using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the hitbox for the Lightningball Explosion Hitbox
public class LightningBallExplosionHitboxScript : MonoBehaviour {

    public int damage; // Damage sender
    public object[] knockBackSenderEnemy; // Knockback packager
    public Vector2 knockBackEnemy; // Knockback direction
    public int knockBackTimerEnemy; // Knockback duration
    float[] knockBackDirection; // Used for directional calculation

    void Start () {
        damage = 1;

        knockBackEnemy = new Vector2(4, 4);
        knockBackTimerEnemy = 5;
        knockBackSenderEnemy = new object[2];

        knockBackDirection = new float[3];
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            // If the hitbox connects, find knockback direction in relation to enemy, send damage, knockback, and knockback time
            coll.gameObject.SendMessage("applyDamage", damage, SendMessageOptions.DontRequireReceiver);
            knockBackDirection[0] = (coll.gameObject.transform.position.x - transform.position.x);
            knockBackDirection[1] = (coll.gameObject.transform.position.y - transform.position.y);
            knockBackDirection[2] = (Mathf.Sqrt(Mathf.Pow(knockBackDirection[0], 2) + Mathf.Pow(knockBackDirection[1], 2)));
            knockBackDirection[0] = knockBackDirection[0] / knockBackDirection[2];
            knockBackDirection[1] = knockBackDirection[1] / knockBackDirection[2];

            knockBackSenderEnemy[0] = new Vector2(knockBackEnemy.x * knockBackDirection[0], knockBackEnemy.y * knockBackDirection[1]);
            knockBackSenderEnemy[1] = knockBackTimerEnemy;
            coll.gameObject.SendMessage("applyKnockBack", knockBackSenderEnemy, SendMessageOptions.DontRequireReceiver);
        }
    }

}
