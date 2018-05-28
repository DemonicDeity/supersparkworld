using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the hitbox for the Overhead Shine
public class OverheadShineScript : MonoBehaviour {

    public int damage; // Damage sender
    public object[] knockBackSender; // Knockback packager
    public Vector2 knockBack; // Knockback direction
    public int knockBackTimer; // Knockback duration
    float[] knockBackDirection; // Used for directional calculation

    void Start()
    {
        damage = 1;
        knockBack = new Vector2(4, 8);
        knockBackTimer = 30;
        knockBackSender = new object[2];

        knockBackDirection = new float[3];
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            // If the hitbox connects, find knockback direction in relation to enemy, send damage, knockback, and knockback time
            knockBackDirection[0] = (coll.gameObject.transform.position.x - transform.parent.parent.position.x);
            knockBackDirection[1] = (coll.gameObject.transform.position.y - transform.parent.parent.position.y);
            knockBackDirection[2] = (Mathf.Sqrt(Mathf.Pow(knockBackDirection[0], 2) + Mathf.Pow(knockBackDirection[1], 2)));
            knockBackDirection[0] = knockBackDirection[0] / knockBackDirection[2];
            knockBackDirection[1] = knockBackDirection[1] / knockBackDirection[2];

            knockBackSender[0] = new Vector2(knockBack.x * knockBackDirection[0], knockBack.y * knockBackDirection[1]);
            knockBackSender[1] = knockBackTimer;
            coll.gameObject.SendMessage("applyDamage", damage, SendMessageOptions.DontRequireReceiver);
            coll.gameObject.SendMessage("applyKnockBack", knockBackSender, SendMessageOptions.DontRequireReceiver);
        }
    }
}
