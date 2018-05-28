using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Controls the hitbox for entering the cutscene
public class Cutscene4TriggerScript : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D coll)
    {
        // If the player enters the cutscene area and the cutscene hasn't been triggered yet.
        if (coll.gameObject.tag == "Player" && GameControllerScript.gameController.getCutsceneTrigger(3) == 0)
        {
            // The cutscene is trigger in the Map2Script
            transform.GetComponentInParent<Map2Script>().cutscene4Trigger();
        }
    }
}
