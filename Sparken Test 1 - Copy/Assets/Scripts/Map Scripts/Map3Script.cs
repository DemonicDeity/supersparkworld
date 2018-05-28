using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Controls all cutscenes and changes on the third map
public class Map3Script : MonoBehaviour {

    GameObject sparken; // Holds the player character
    GameObject rockin; // Holds the Rockin of the scene
    DialogueManager dialogueManager; //Holds the dialoguemanager
    CutsceneDialogueScript cutsceneDialogue; //Creates a cutscene manager
    public float deathLevel; // Holds the level at which the player dies in the level
    StoneWallControllerScript[] stonewallcontrollers;

    // Use this for initialization
    void Start()
    {
        sparken = GameObject.FindGameObjectWithTag("Player");
        rockin = GameObject.Find("Rockin");
        dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();
        cutsceneDialogue = new CutsceneDialogueScript();
        stonewallcontrollers = transform.GetComponentsInChildren<StoneWallControllerScript>();

        deathLevel = GetComponent<Renderer>().bounds.min.y;

        GameControllerScript.gameController.setDeathLevel(deathLevel); // Sets the level at which the player dies in the level

        rockin.GetComponent<RockinControllerScript>().hostile = false; // Sets the Rockin to be unhostile at first

        if (GameControllerScript.gameController.getCutsceneTrigger(6) == 6)
        {
            GameObject.Find("Rockin").SetActive(false);
        }

    }

    // Update is called once per frame
    void FixedUpdate () {

        // When the Sparken reaches the left side of the stage, load scene 1
        if (sparken.transform.position.x < -15f)
        {
            GameControllerScript.gameController.setWalkedOrDied(true);
            GameControllerScript.gameController.setSparkenPlace(new Vector3(15.2f, 0.45f, 0));
            GameControllerScript.gameController.setCurrentHealth();
            SceneManager.LoadScene("scene 2");
        }

        // If the Rockin Conversation part 1 has ended
        if (GameControllerScript.gameController.getCutsceneTrigger(6) == 2)
        {
            //Set the walls to the raised position
            foreach(StoneWallControllerScript i in stonewallcontrollers)
            {
                i.setDesiredYPosition(-0.4f, 0.2f, 1);
            }
            // Update cutscene
            GameControllerScript.gameController.setCutsceneTrigger(6, 3);
            // Begin Rockin Conversation part 2
            dialogueManager.startCutsceneDialogue(cutsceneDialogue.getCutsceneRemarks(7), 6);
        }
        // If Rockin conversation has ended
        else if (GameControllerScript.gameController.getCutsceneTrigger(6) == 4)
        {
            rockin.GetComponent<RockinControllerScript>().hostile = true; // Set rockin to hostile
            GameControllerScript.gameController.setCutsceneTrigger(6, 5); // Set the cutscene to the fight
        }
        // If the Rockin was defeated
        else if (GameControllerScript.gameController.getCutsceneTrigger(6) == 5 && GameObject.Find("Rockin") == null)
        {
            GameControllerScript.gameController.setCutsceneTrigger(6, 6); // Update cutscene
            dialogueManager.startCutsceneDialogue(cutsceneDialogue.getCutsceneRemarks(8), 6); // Set end of battle dialogue
            //Lower the walls
            foreach (StoneWallControllerScript i in stonewallcontrollers)
            {
                i.setDesiredYPosition(-12f, 0.2f, 1);
            }
        }
        // If the fight is still triggered, but the player hasn't entered the arena yet
        else if (GameControllerScript.gameController.getCutsceneTrigger(6) == 5 && sparken.transform.position.x > -2.3f)
        {
            rockin.GetComponent<RockinControllerScript>().hostile = true;
            //Raise the walls
            foreach (StoneWallControllerScript i in stonewallcontrollers)
            {
                i.setDesiredYPosition(-0.4f, 0.2f, 1);
            }
        }
    }

    //Updates cutscene and begins dialogue
    public void cutscene6Trigger()
    {
        if (GameControllerScript.gameController.getCutsceneTrigger(5) == 0)
        {
            GameControllerScript.gameController.setCutsceneTrigger(5, 1);
            dialogueManager.startCutsceneDialogue(cutsceneDialogue.getCutsceneRemarks(5), 5);
        }
    }

    //Updates cutscene and begins dialogue
    public void cutscene7Trigger()
    {
        if (GameControllerScript.gameController.getCutsceneTrigger(6) == 0)
        {
            GameControllerScript.gameController.setCutsceneTrigger(6, 1);
            dialogueManager.startCutsceneDialogue(cutsceneDialogue.getCutsceneRemarks(6), 6);
        }
    }

    //Updates cutscene and begins dialogue
    public void cutscene8Trigger()
    {
        if (GameControllerScript.gameController.getCutsceneTrigger(7) == 0)
        {
            GameControllerScript.gameController.setCutsceneTrigger(7, 1);
            dialogueManager.startCutsceneDialogue(cutsceneDialogue.getCutsceneRemarks(9), 7);
        }
    }
}
