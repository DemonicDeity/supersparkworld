﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the hitbox for Slash Up
public class SlashUpScript : MonoBehaviour {

    public int damage; // Damage sender
    public object[] knockBackSender; // Knockback packager
    public Vector2 knockBack; // Knockback direction
    public int knockBackTimer; // Knockback duration

    void Start () {
        damage = 1;
        knockBack = new Vector2(0, 6);
        knockBackTimer = 30;
        knockBackSender = new object[2];
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            // If the hitbox connects, send damage, knockback, and knockback time
            knockBackSender[0] = new Vector2(knockBack.x* transform.parent.parent.localScale.x, knockBack.y);
            knockBackSender[1] = knockBackTimer;
            coll.gameObject.SendMessage("applyDamage", damage, SendMessageOptions.DontRequireReceiver);
            coll.gameObject.SendMessage("applyKnockBack", knockBackSender, SendMessageOptions.DontRequireReceiver);
        }
    }

}
