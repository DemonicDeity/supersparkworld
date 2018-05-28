using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Script for controlling all dialogue. Really. 
public class DialogueManager : MonoBehaviour {

    public static DialogueManager dialogueManager; // Singleton design

    public TextMeshProUGUI dialogueText; // The dialogue text
    public GameObject source; // The gameobject speaking

    public Queue<string> sentences; // The sentences that the gameobject contains
    public FindClosestScript findClosest; // Finding the closest for determining source
    int cutsceneNumber; // Cutscene number for determining which cutscene dialogue to take
    int cutsceneSentenceNumber; // The number of the sentence in the cutscene
    object[,] cutsceneHolder; // Holds the full cutscene dialogue and source

    // Use this for initialization

    void Awake()
    {
        if (dialogueManager == null)
        {
            DontDestroyOnLoad(gameObject);
            dialogueManager = this;
        }
        else if (dialogueManager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start () {
        sentences = new Queue<string>();
        findClosest = new FindClosestScript();
        cutsceneNumber = -1;
        cutsceneSentenceNumber = 0;
    }

    // Determines if there is a sentence in the queue for randomly chatting with npcs
    public bool sentenceQueued()
    {
        if (source == null)
        {
            return false;
        }
        return true;
    }

    // Starts a dialogue with an npc with a dialoguescript
    public void startDialogue(DialoguesScript dialogues)
    {
        dialogueText.transform.parent.gameObject.SetActive(true); // sets dialogue box on
        sentences.Clear(); // clear sentences in preperation

        source = dialogues.gameObject; // Sets the source of the dialogue

        // Places all sentences from the dialoguescript into the queue
        foreach (string sentence in dialogues.sentences)
        {
            sentences.Enqueue(sentence);
        }
        displayNextSentence(); // Begins to cycle through the dialogue
    }

    // Starts dialogue with Sparken if there aren't any nearby creatures to talk to
    public void startDialogue(string[] dialogues)
    {
        dialogueText.transform.parent.gameObject.SetActive(true); // Turns on the dialoguebox
        sentences.Clear(); // Clears the sentences

        source = GameObject.Find("Sparken"); // Sets the source to Sparken

        // Queues every sentence of Sparken's dialogue
        foreach (string sentence in dialogues)
        {
            sentences.Enqueue(sentence);
        }
        displayNextSentence(); // Begins to cycle through sentences
    }

    // Cycles through queued dialogue 
    public void displayNextSentence()
    {
        // Ends dialogue if it runs out of sentences
        if (sentences.Count == 0)
        {
            endDialogue();
            return;
        }
        string sentence = sentences.Dequeue(); // Dequeues a sentences to display

        StopAllCoroutines();
        StartCoroutine(typeSentence(sentence)); // Sends sentence to be typed out
    }

    // Begins dialogue from a cutscene, setting first line and source
    public void startCutsceneDialogue(object[,] dialogues, int incomingCutsceneNumber)
    {
        GameControllerScript.gameController.cutsceneEnter(); // Stops all player movement

        cutsceneNumber = incomingCutsceneNumber; // Sets cutscene number
        dialogueText.transform.parent.gameObject.SetActive(true); // Sets dialogue box to active
        sentences.Clear(); // Clears sentences

        // Finds the nearest creature of the type that begins dialogue
        source = findClosest.GetClosestObjectByName(GameObject.Find("Sparken"), (string)dialogues[0, 1], 5);

        cutsceneHolder = dialogues; // sets the cutscene holder to the current cutscene dialogue
        displayNextCutsceneSentence(); // Cycles between dialogue
    }

    // Cycles between cutscene sentences and sources
    public void displayNextCutsceneSentence()
    {
        // Ends dialogue if there are no more sentences
        if (cutsceneSentenceNumber == cutsceneHolder.GetLength(0))
        {
            cutsceneSentenceNumber = 0;
            endDialogue();
            return;
        }

        string sentence = (string)cutsceneHolder[cutsceneSentenceNumber, 0]; // Gets the next sentence
        // Finds the nearest creature of the type
        source = findClosest.GetClosestObjectByName(GameObject.Find("Sparken"), (string)cutsceneHolder[cutsceneSentenceNumber, 1], 5);

        StopAllCoroutines();
        StartCoroutine(typeSentence(sentence)); // Begins to type the sentence
        cutsceneSentenceNumber++; // Updates current sentence
    }

    // Goes through letter by letter and displays them
    IEnumerator typeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letterInSentence in sentence.ToCharArray())
        {
            dialogueText.text += letterInSentence;
            yield return null;
        }
    }

    // Called when dialogue ends. May update cutscenes.
	public void endDialogue()
    {
        if (source != null && source.name == "Sparken")
        {
            source.SendMessage("checkOnDialogueEnd", SendMessageOptions.DontRequireReceiver);
        }
        if (cutsceneNumber >= 0)
        {
            GameControllerScript.gameController.updateCutsceneState(cutsceneNumber);
            cutsceneNumber = -1;
        }

        GameControllerScript.gameController.cutsceneExit();

        source = null;
        dialogueText.transform.parent.gameObject.SetActive(false);
    }

    // Returns of the character is in a cutscene
    public bool inCutScene()
    {
        if (cutsceneSentenceNumber > 0)
        {
            return true;
        }
        return false;
    }
}
