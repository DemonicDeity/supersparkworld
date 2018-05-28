using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls the sixth cutscene if the player enters the cutscene area
public class Cutscene6Trigger : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D coll)
    {
        //If the player enters the cutscene area and it hasn't been triggered yet
        if (coll.gameObject.tag == "Player" && GameControllerScript.gameController.getCutsceneTrigger(5) == 0)
        {
            // The cutscene is triggered in the Map3Script
            transform.GetComponentInParent<Map3Script>().cutscene6Trigger();
        }
    }
}
