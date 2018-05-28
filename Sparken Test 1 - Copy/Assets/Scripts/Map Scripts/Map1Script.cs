using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// The script for controlling all of the cutscenes and changes in Map 1
public class Map1Script : MonoBehaviour {

    GameObject sparken; // The Sparken
    GameObject rocklet; // The Rocklet of the scene
    DialogueManager dialogueManager; // The Dialogue Manager
    CutsceneDialogueScript cutsceneDialogue; // The Cutscenedialogue Script used to pull the cutscenes

    public float deathLevel; // Sets the Deathlevel of the map

	// Use this for initialization
	void Start () {
        sparken = GameObject.FindGameObjectWithTag("Player");
        rocklet = GameObject.Find("Rocklet");
        dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();
        cutsceneDialogue = new CutsceneDialogueScript();

        deathLevel = -4;

        // If the player exited in the middle of the first cutscene, roll it back
        if (GameControllerScript.gameController.getCutsceneTrigger(0) == 1)
        {
            GameControllerScript.gameController.setCutsceneTrigger(0, 0);
        }
        // If the player finished the cutscene with the rocklet, set it to hostile
        if (GameControllerScript.gameController.getCutsceneTrigger(1) == 3)
        {
            rocklet.GetComponent<RockletController>().hostile = true;
        }
        // If the player has already defeated the Rocklet, don't spawn another.
        else if (GameControllerScript.gameController.getCutsceneTrigger(1) == 4)
        {
           rocklet.SetActive(false);
        }
        GameControllerScript.gameController.setDeathLevel(deathLevel);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        // Starts the first cutscene when the player falls onto the screen
        if (sparken.GetComponent<Animator>().GetInteger("Action") == 0 && GameControllerScript.gameController.getCutsceneTrigger(0) == 0)
        {
            GameControllerScript.gameController.setCutsceneTrigger(0, 1);
            dialogueManager.startCutsceneDialogue(cutsceneDialogue.getCutsceneRemarks(0), 0);
        }
        // Starts the second cutscene when the player reaches the right side of the stage
		if (sparken.transform.position.x > 10.5 && GameControllerScript.gameController.getCutsceneTrigger(1) == 0)
        {
            GameControllerScript.gameController.setCutsceneTrigger(1, 1);
            dialogueManager.startCutsceneDialogue(cutsceneDialogue.getCutsceneRemarks(1), 1);
            rocklet.GetComponent<RockletController>().hostile = false;
        }
        // Sets the Rocklet to hostile when the player finishes the dialogue
        else if (GameControllerScript.gameController.getCutsceneTrigger(1) == 2)
        {
            rocklet.GetComponent<RockletController>().hostile = true;
            GameControllerScript.gameController.setCutsceneTrigger(1, 3);
        }
        // Updates the cutscene when the player kills the rocklet
        else if (GameControllerScript.gameController.getCutsceneTrigger(1) == 3 && GameObject.Find("Rocklet") == null)
        {
            GameControllerScript.gameController.setCutsceneTrigger(1, 4);
        }
        // Moves the player to the next map
        if (sparken.transform.position.x > 15f && GameControllerScript.gameController.getCutsceneTrigger(1) == 4)
        {
            GameControllerScript.gameController.setWalkedOrDied(true);
            GameControllerScript.gameController.setSparkenPlace( new Vector3(-14.5f, 1.6f, 0));
            GameControllerScript.gameController.setCurrentHealth();
            SceneManager.LoadScene("scene 2");
        }
	}
}
