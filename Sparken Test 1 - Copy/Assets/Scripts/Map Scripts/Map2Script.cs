using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map2Script : MonoBehaviour {

    GameObject sparken; // Gets the Sparken
    DialogueManager dialogueManager; // Gets the Dialogue Manager
    CutsceneDialogueScript cutsceneDialogue; // Creates a cutsceneDialogue
    public float deathLevel; // sets the deathlevel

    // Use this for initialization
    void Start()
    {
        sparken = GameObject.FindGameObjectWithTag("Player");
        dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();
        cutsceneDialogue = new CutsceneDialogueScript();

        deathLevel = GetComponent<Renderer>().bounds.min.y;

        GameControllerScript.gameController.setDeathLevel(deathLevel);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // When the Sparken reaches the left side of the stage, load scene 1
        if (sparken.transform.position.x < -15f)
        {
            GameControllerScript.gameController.setWalkedOrDied(true);
            GameControllerScript.gameController.setSparkenPlace(new Vector3(14, 0.3f, 0));
            GameControllerScript.gameController.setCurrentHealth();
            SceneManager.LoadScene("scene 1");
        }

        // When the Sparken reaches the right side of the stage, load scene 2
        if (sparken.transform.position.x > 15.3f && sparken.transform.position.y > 0)
        {
            GameControllerScript.gameController.setWalkedOrDied(true);
            GameControllerScript.gameController.setSparkenPlace(new Vector3(-14.8f, 5.3f, 0));
            GameControllerScript.gameController.setCurrentHealth();
            SceneManager.LoadScene("scene 3");
        }
        // When the Sparken reaches the right side of the platform, trigger the cutscene
        if (sparken.transform.position.x > -8.5f && GameControllerScript.gameController.getCutsceneTrigger(2) == 0)
        {
            GameControllerScript.gameController.setCutsceneTrigger(2, 1);
            dialogueManager.startCutsceneDialogue(cutsceneDialogue.getCutsceneRemarks(2), 2);
        }
    }
    // Manually trigger the fourth cutscene from a trigger
    public void cutscene4Trigger()
    {
        if (GameControllerScript.gameController.getCutsceneTrigger(3) == 0)
        {
            GameControllerScript.gameController.setCutsceneTrigger(3, 1);
            dialogueManager.startCutsceneDialogue(cutsceneDialogue.getCutsceneRemarks(3), 3);
        }
    }
    // Manually trigger the fifth the cutscene from a trigger
    public void cutscene5Trigger()
    {
        if (GameControllerScript.gameController.getCutsceneTrigger(4) == 0)
        {
            GameControllerScript.gameController.setCutsceneTrigger(4, 1);
            dialogueManager.startCutsceneDialogue(cutsceneDialogue.getCutsceneRemarks(4), 4);
        }
    }
}
