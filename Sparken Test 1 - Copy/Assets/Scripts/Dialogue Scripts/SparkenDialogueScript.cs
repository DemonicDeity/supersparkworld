using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Dialogue holder for Sparken remarks
public class SparkenDialogueScript {

    private string[][] sparkenRemarks; // Holds all of the Sparken's remarks on the scenes
    private string[][] sparkenDeathRemarks; // Holds all of the Sparken's Death Quotes
    private int loadedRemarks; // Holds the remarks being loaded in

    Scene scene; // Gets the current scene for requesting remarks
    string sceneName; // Gets the current scene name for requesting remarks

    bool loaded = false; // Determines if the dialogue has been loaded yet

    // Loads all of the data into arrays
    public void loadRemarks()
    {
        sparkenRemarks = new string[10][];
        sparkenRemarks[0] = new string[]
        {
            "I'd better look around...",
            "Nice waterfall though!"
        };
        sparkenRemarks[1] = new string[]
        {
            "This forest is pretty dense.",
            "And if I miss a jump...",
            "Well, it's a long way down.",
            "Ha... haha..."
        };
        sparkenRemarks[2] = new string[]
        {
            "The forest is clearing out.",
            "Well, a little bit.",
            "We must be getting closer now!",
            "...Who's excited?"
        };


        sparkenDeathRemarks = new string[10][];
        sparkenDeathRemarks[0] = new string[]
        {
            "This is not the end..."
        };
        sparkenDeathRemarks[1] = new string[]
        {
            "I'll be back..."
        };
        sparkenDeathRemarks[2] = new string[]
        {
            "You inspire the fury of a god..."
        };
        sparkenDeathRemarks[3] = new string[]
        {
            "I am Sparken! I... cannot... fall..."
        };
        sparkenDeathRemarks[4] = new string[]
        {
            "Wait for me..."
        };
        sparkenDeathRemarks[5] = new string[]
        {
            "For this, you are damned..."
        };
        sparkenDeathRemarks[6] = new string[]
        {
            "Ughh! Y-you'll suffer for this!"
        };
        sparkenDeathRemarks[7] = new string[]
        {
            "God does- does not die!"
        };
        sparkenDeathRemarks[8] = new string[]
        {
            "Ahh- You... You're mine now!"
        };

    }

    // Retrieves a remark based on the current scene
    public string[] getRemarks()
    {
        if (loaded == false) {
            loadRemarks();
            loaded = true;
        }

        scene = SceneManager.GetActiveScene();
        sceneName = scene.name;
        if (sceneName == "scene 1")
        {
            loadedRemarks = 0;
        }
        else if (sceneName == "scene 2")
        {
            loadedRemarks = 1;
        }
        else if (sceneName == "scene 3")
        {
            loadedRemarks = 2;
        }
        return sparkenRemarks[loadedRemarks];
    }

    // Gets the Sparken's remark for when he dies randomly out of a pool
    public string[] getDeathRemarks()
    {
        if (loaded == false)
        {
            loadRemarks();
            loaded = true;
        }
        int doesThisWork = Random.Range(0, 9);
        loadedRemarks = doesThisWork;
        return sparkenDeathRemarks[loadedRemarks];
    }

}
