using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sets the fifth cutscene if the player enters the area
public class Cutscene5TriggerScript : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D coll)
    {
        // If the player enters the area and the cutscene hasn't been triggered yet
        if (coll.gameObject.tag == "Player" && GameControllerScript.gameController.getCutsceneTrigger(4) == 0)
        {
            // The cutscene is triggered in Map2Script
            transform.GetComponentInParent<Map2Script>().cutscene5Trigger();
        }
    }
}
