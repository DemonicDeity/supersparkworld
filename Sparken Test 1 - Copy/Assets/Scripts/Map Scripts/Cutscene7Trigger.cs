using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the seventh cutscene if the player enters the area
public class Cutscene7Trigger : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D coll)
    {
        // If the player enters the area and the cutscene hasn't been triggered yet
        if (coll.gameObject.tag == "Player" && GameControllerScript.gameController.getCutsceneTrigger(6) == 0)
        {
            //Trigger the cutscene in the Map3Script
            transform.GetComponentInParent<Map3Script>().cutscene7Trigger();
        }
    }
}
