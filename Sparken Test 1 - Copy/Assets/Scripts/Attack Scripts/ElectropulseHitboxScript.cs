using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the hitbox for the Electropulse
public class ElectropulseHitboxScript : MonoBehaviour {

    public int damage; // Damage sender
    public object[] knockBackSender; // Knockback packager
    public Vector2 knockBack; // Knockback direction
    public int knockBackTimer; // Knockback duration
    int direction; // Direction of knockback from character

    void Start()
    {
        damage = 2;
        knockBack = new Vector2(4, 2);
        knockBackTimer = 30;
        knockBackSender = new object[2];
        direction = 1;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            // Enemies are knocked away from Sparken
            if (transform.parent.parent.transform.position.x < coll.gameObject.transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            // If the hitbox connects, send damage, knockback, and knockback time
            knockBackSender[0] = new Vector2(knockBack.x * direction, knockBack.y);
            knockBackSender[1] = knockBackTimer;
            coll.gameObject.SendMessage("applyDamage", damage, SendMessageOptions.DontRequireReceiver);
            coll.gameObject.SendMessage("applyKnockBack", knockBackSender, SendMessageOptions.DontRequireReceiver);
        }
    }
}
