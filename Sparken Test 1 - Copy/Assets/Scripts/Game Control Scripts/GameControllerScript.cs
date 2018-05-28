using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System;
using UnityEngine.SceneManagement;


// A universal class across the game which holds the permanent data. Includes: hitpoints, cutscenes, loading, saving, refreshing, ect
public class GameControllerScript : MonoBehaviour {

    public static GameControllerScript gameController; // Singleton design
    public SavePointControllerScript savePointController; // A script which contains save points
    public int currentHealth; // Saves the Sparken's current health
    public int maximumHealth; // Saves the Sparken's maximum health
    public int lastCheckpoint; // Saves the Sparken's last checkpoint
    public float deathLevel; // The level at which the player dies if it falls
    public bool walkedOrDied; // Determines whether the player walked into the next area or died
    public int[] cutsceneTriggers; // Holds the triggers for the cutscenes
    public Vector3 sparkenPlace; // Saves the Sparken's respawn place
    public bool gamePause; // Determines if the game is paused
    public bool cutscenePaused; // Determines if the game is in a cutscene

	// Use this for initialization
	void Awake () {
		if (gameController == null)
        {
            DontDestroyOnLoad(gameObject);
            gameController = this;
            walkedOrDied = true;
            cutsceneTriggers = new int[20];
            lastCheckpoint = -1;
            savePointController = new SavePointControllerScript();
}
        else if (gameController != this)
        {
            Destroy(gameObject);
        }
	}
    private void Start()
    {
    }
    
    //Sets the health tracker to the Sparken's health
    public void setCurrentHealth()
    {
        currentHealth = GameObject.Find("Sparken").GetComponent<PlayerController>().hitpoints;
    }
    //Sets the health tracker to an incoming int
    public void loadCurrentHealth(int incomingCurrentHealth)
    {
        currentHealth = incomingCurrentHealth;
    }
    //Gets the current health tracker
    public int getCurrentHealth()
    {
        return currentHealth;
    }
    //Sets the maximum health tracker to the Sparken's health
    public void setMaximumHealth()
    {
        maximumHealth = GameObject.Find("Sparken").GetComponent<PlayerController>().hitpointMaximum;
    }
    //Sets the maximum health tracker to an incoming int
    public void loadMaximumHealth(int incomingMaximumHealth)
    {
        maximumHealth = incomingMaximumHealth;
    }
    //Gets the maximum health tracker
    public int getMaximumHealth()
    {
        return maximumHealth;
    }
    //Sets last checkpoint
    public void setLastCheckpoint(int incomingLastCheckpoint)
    {
        lastCheckpoint = incomingLastCheckpoint;
    }
    //Gets last checkpoint
    public int getLastCheckpoint()
    {
        return lastCheckpoint;
    }
    //Sets walkedordied
    public void setWalkedOrDied(bool incomingWalkedOrDied)
    {
        walkedOrDied = incomingWalkedOrDied;
    }
    //Gets walkedordied
    public bool getWalkedOrDied()
    {
        return walkedOrDied;
    }
    //Sets all of the cutscenetriggers
    public void setCutsceneTriggers(int[] incomingCutsceneTriggers)
    {
        Array.Copy(incomingCutsceneTriggers, cutsceneTriggers, cutsceneTriggers.Length);
    }
    //Sets one cutscenetrigger
    public void setCutsceneTrigger(int key, int incomingCutsceneTrigger)
    {
        cutsceneTriggers[key] = incomingCutsceneTrigger;
    }
    //Gets one cutscene trigger
    public int getCutsceneTrigger(int key)
    {
        return cutsceneTriggers[key];
    }
    //Sets the sparken's place
    public void setSparkenPlace(Vector3 newPlace)
    {
        sparkenPlace = newPlace;
    }
    //Gets the sparken's place
    public Vector3 getSparkenPlace()
    {
        return sparkenPlace;
    }
    //Sets the deathlevel
    public void setDeathLevel(float incomingDeathLevel)
    {
        deathLevel = incomingDeathLevel;
    }
    //Updates cutscenetriggers based on specific conditions
    public void updateCutsceneState(int currentCutscene)
    {
        if (currentCutscene == 1)
        {
            if (cutsceneTriggers[0] == 1)
            {
                cutsceneTriggers[0] = 2;
            }
            if (cutsceneTriggers[1] == 1)
            {
                cutsceneTriggers[1] = 2;
            }
        }
        if (currentCutscene == 6)
        {
            if (cutsceneTriggers[6] == 1)
            {
                cutsceneTriggers[6] = 2;
            }
            if (cutsceneTriggers[6] == 3)
            {
                cutsceneTriggers[6] = 4;
            }
        }
    }
    //Sets cutscenepaused and sends cutscene state to sparken
    public void cutsceneEnter()
    {
        cutscenePaused = true;
        GameObject.Find("Sparken").SendMessage("setCutscene", !cutscenePaused, SendMessageOptions.DontRequireReceiver);
    }
    //Sets cutscenepaused and sends cutscene state to sparken
    public void cutsceneExit()
    {
        cutscenePaused = false;
        GameObject.Find("Sparken").SendMessage("setCutscene", !cutscenePaused, SendMessageOptions.DontRequireReceiver);
    }

    // Update is called once per frame
    void Update () {
        // Pauses and unpauses the game on pressing escape
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (!gamePause)
            {
                PauseGame();
            }
            else if (gamePause)
            {
                ContinueGame();
            }
        }
    }

    // Pauses the game by setting the timescale to 0 and freezing character movement. It also activates the pause menu.
    public void PauseGame()
    {
        gamePause = true;
        Time.timeScale = 0;
        GameObject.Find("Sparken").SendMessage("setPaused", !gamePause, SendMessageOptions.DontRequireReceiver);
        GameObject.Find("Canvas").transform.GetComponentInChildren(typeof(PauseMenuController), true).gameObject.SetActive(true);
        GameObject.Find("Canvas").transform.GetComponentInChildren(typeof(TextController), true).gameObject.SetActive(false);
    }
    // Unpauses the game by resetting the timescale. It also de-activates the pause menu.
    public void ContinueGame()
    {
        gamePause = false;
        Time.timeScale = 1;
        GameObject.Find("Sparken").SendMessage("setPaused", !gamePause, SendMessageOptions.DontRequireReceiver);
        GameObject.Find("Canvas").transform.GetComponentInChildren(typeof(PauseMenuController), true).gameObject.SetActive(false);
        GameObject.Find("Canvas").transform.GetComponentInChildren(typeof(TextController), true).gameObject.SetActive(true);
    }

    // Loads scene from the last checkpoint
    public void loadFromCheckPoint()
    {
        ContinueGame();
        DialogueManager.dialogueManager.endDialogue();
        SceneManager.LoadScene((string)savePointController.getSavePoints(getLastCheckpoint())[0]);
    }

    // Loads the game from a save and then resets to the last checkpoint
    public void reloadFromSave()
    {
            DialogueManager.dialogueManager.endDialogue();
            loadGame();
            setSparkenPlace((Vector3)savePointController.getSavePoints(getLastCheckpoint())[1]);
            SceneManager.LoadScene((string)savePointController.getSavePoints(getLastCheckpoint())[0]);
            ContinueGame();
    }
    // Loads game from a save when on the title screen
    public void loadGameFromSave()
    {
        loadGame();
        setSparkenPlace((Vector3)savePointController.getSavePoints(getLastCheckpoint())[1]);
        SceneManager.LoadScene((string)savePointController.getSavePoints(getLastCheckpoint())[0]);
    }

    // Starts the game at beginning
    public void beginGame()
    {
        SceneManager.LoadScene("scene 1");
    }

    // Saves the game then exits out of the application
    public void exitGame()
    {
        saveGame();
        Application.Quit();
    }

    // Saves the game. More detail explained in steps.
    public void saveGame()
    {
        BinaryFormatter bf = new BinaryFormatter(); // Creates a Binary Formatter to hold the data
        FileStream file = File.Create(Application.persistentDataPath + "/playerinfo.dat"); //Creates a file to save the information to.
        setMaximumHealth(); // Sets maximum Health
        setCurrentHealth(); // Sets current health
        // Creates a PlayerData file that stores HP, checkpoints, and cutscenes
        PlayerData data = new PlayerData(currentHealth, maximumHealth, lastCheckpoint, cutsceneTriggers);
        bf.Serialize(file, data); // serializes and puts the data into a file
        file.Close(); // closes the file
    }

    // Checks to see if a save exists to load from
    public bool saveExists()
    {
        if (File.Exists(Application.persistentDataPath + "/playerinfo.dat"))
        {
            return true;
        }
        return false;
    }

    // Load the game. More detail explained in steps.
    public void loadGame()
    {
        //Checks if a save file exists
        if (File.Exists(Application.persistentDataPath + "/playerinfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter(); // Creates a Binary Formatter to load data
            FileStream file = File.Open(Application.persistentDataPath + "/playerinfo.dat", FileMode.Open); // Opens the save file 
            PlayerData data = (PlayerData)bf.Deserialize(file); // Creates a PlayerData file and deserializes the file into it
            file.Close(); // Closes the file

            data.setGame(); // Sends the datafile to set the game
        }

    }
}

//Serializable file which stores all playerdata for a game.
[Serializable]
public class PlayerData
{
    int currentHealth;
    int maximumHealth;
    int lastCheckpoint;
    int[] cutsceneTriggers;
    //Sets all PlayerData
    public PlayerData (int currentHealth, int maximumHealth, int lastCheckpoint, int[] cutsceneTriggers)
    {
        this.currentHealth = currentHealth;
        this.maximumHealth = maximumHealth;
        this.lastCheckpoint = lastCheckpoint;
        this.cutsceneTriggers = cutsceneTriggers;
    }
    //Moves all PlayerData into the game
    public void setGame()
    {
        GameControllerScript.gameController.loadCurrentHealth(currentHealth);
        GameControllerScript.gameController.loadMaximumHealth(maximumHealth);
        GameControllerScript.gameController.setLastCheckpoint(lastCheckpoint);
        GameControllerScript.gameController.setCutsceneTriggers(cutsceneTriggers);
    }
}
