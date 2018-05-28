using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script for controlling the position of the dialogue box
public class DialogueBoxScript : MonoBehaviour {

    DialogueManager dialogueManager; // The Dialogue manager
    GameObject following; // Gameobject that the dialogue box appears above
    Vector3 position; // Position of the dialogue box

	// Use this for initialization
	void Start () {
        dialogueManager = GameObject.Find("Dialogue").GetComponent<DialogueManager>();
    }
	
	// Update is called once per frame
	void LateUpdate () {

        // If the dialoguemanager currently has a source
        if (dialogueManager.source != null)
        {
            // Sets the position of the dialogue box 1 unit above the sources head
            following = dialogueManager.source; 
            position = following.transform.position;
            position.y = position.y + 1;
            gameObject.transform.position = position;
        }
	}
}
