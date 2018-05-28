using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to trigger dialogue. 
public class DialogueTrigger : MonoBehaviour {

    public DialoguesScript dialogues;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Triggers dialogue in DialogueManager
    public void triggerDialogue()
    {
        FindObjectOfType<DialogueManager>().startDialogue(dialogues);
    }
}
