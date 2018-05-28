using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the hitbox for the Lightning Eyes
public class LightningEyesHitboxScript : MonoBehaviour {

    public int damage; // Lightning Eyes damage

	void Start () {
        damage = 2;
	}

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            // Sends damage if it connects with enemy
            coll.gameObject.SendMessage("applyDamage", damage, SendMessageOptions.DontRequireReceiver);
        }
    }

}
