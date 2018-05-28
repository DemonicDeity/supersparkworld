using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script to hold all the cutscene dialogue and send it to the dialoguemanager
public class CutsceneDialogueScript
{

    private object[][,] cutsceneRemarks; // packager to send remarks

    Scene scene; // Current scene
    string sceneName; // Name of the current scene

    bool loaded = false; // Determines if the cutsceneremarks have been loaded

    // Load dialogue for the cutscenes
    public void loadRemarks()
    {
        cutsceneRemarks = new object[10][,];
        cutsceneRemarks[0] = new string[,]
        {
            {"Where... is this?", "Sparken" },
            {"The Earth Shrine?", "Sparken" },
            {"Damn it!", "Sparken" },
            {"Well, that means I'm here for...", "Sparken" },
            {"Something!", "Sparken" },
            {"This means the world needs a god.", "Sparken" },
            {"A god like me.", "Sparken" }
        };
        cutsceneRemarks[1] = new string[,]
        {
            {"Why are you blocking my way?", "Sparken" },
            {"This way leads to the sacred village.", "Rocklet" },
            {"Only the chosen may enter.", "Rocklet" },
            {"Chosen? Chosen- Do you have any idea?", "Sparken" },
            {"What? Any idea about what?", "Rocklet" },
            {"Well, sorry to tell you this, but I'm a god.", "Sparken" },
            {"...What? Are you insane?", "Rocklet" },
            {"Hmm. You don't believe me?", "Sparken" },
            {"Of course not. Go back to...", "Rocklet" },
            {"Where did you come from...", "Rocklet" },
            {"What a shame, what a shame.", "Sparken" },
            {"I guess I'll have to SHOW you!", "Sparken" }
        };
        cutsceneRemarks[2] = new string[,]
        {
            {"Hmm... Looks like I need to jump here", "Sparken" },
            {"You do know how to jump, right?", "Sparken" },
            {"Just hit Space. It's really easy.", "Sparken" },
            {"So easy that...", "Sparken" },
            {"Nevermind.", "Sparken" },
            {"Of course, if you hit Space in the air", "Sparken" },
            {"You'll get a little boost as I zip.", "Sparken" },
            {"You have so many options!", "Sparken" },
            {"So, don't kill me, okay?", "Sparken" }
        };
        cutsceneRemarks[3] = new string[,]
        {
            {"Wow.", "Sparken" },
            {"You actually fell down here.", "Sparken" },
            {"OK, change of plans.", "Sparken" },
            {"If my normal jumps aren't good enough", "Sparken" },
            {"(though they are)", "Sparken" },
            {"If you double tap and hold Space", "Sparken" },
            {"I'll go flying!", "Sparken" },
            {"Just make sure you land.", "Sparken" },
            {"And if you don't hold it long enough...", "Sparken" },
            {"I'll only make a tiny hop.", "Sparken" },
            {"So, try to jump back up!", "Sparken" }
        };
        cutsceneRemarks[4] = new string[,]
        {
            {"Well, that wasn't too bad.", "Sparken" },
            {"Just one more jump, huh?", "Sparken" },
            {"Go for it.", "Sparken" }
        };
        cutsceneRemarks[5] = new string[,]
        {
            {"Another shrine, huh?", "Sparken" },
            {"Oh, I think I forgot to mention!", "Sparken" },
            {"When you step on one of these shrines", "Sparken" },
            {"all of your progress will be saved.", "Sparken" },
            {"Pretty handy, huh?", "Sparken" },
            {"But there's something else...", "Sparken" },
            {"The world sometimes places these to warn me", "Sparken" },
            {"that something dangerous is up ahead.", "Sparken" },
            {"...", "Sparken" },
            {"Fantastic, isn't it?", "Sparken" },
            {"Whatever, let's just keep going.", "Sparken" }
        };
        cutsceneRemarks[6] = new string[,]
        {
            {"No, punch, punch, punch, not kick!", "Rockin" },
            {"Uh, who's this guy? Hey, idiot!", "Sparken" },
            {"Huh?", "Rockin" },
            {"What the hell are you doing?", "Sparken" },
            {"OH GODS!", "Rockin" },
            {"Are you...? I can't believe this!", "Rockin" },
            {"What?", "Sparken" },
            {"YOU'RE SPARKEN AREN'T YOU?!", "Rockin" },
            {"...", "Sparken" },
            {"Yeah... Are you a, follower?", "Sparken" },
            {"Follower? I'm a HUUUUGE fan!", "Rockin" },
            {"Oh god.", "Sparken" },
            {"I've read all of the stories about you!", "Rockin" },
            {"Did you really defeat the Mechastorm?!", "Rockin" },
            {"Well, with the help of a hero, yeah.", "Sparken" },
            {"Oh yeah! A hero!", "Rockin" },
            {"Speaking of which, where is your hero now?", "Rockin" },
            {"...", "Sparken" },
            {"I... don't have one right now.", "Sparken" },
            {"Does that mean you're looking for one?", "Rockin" },
            {"...I suppose.", "Sparken" },
            {"Don't you dare-", "Sparken" },
            {"I could be your hero then!", "Rockin" },
            {"Aaaand, here we go.", "Sparken" },
            {"Sparken, I'm a master of martial arts,", "Rockin" },
            {"I pray to you every day,", "Rockin" },
            {"And I donate at least one pebble to charity a week.", "Rockin" },
            {"Hero material, huh?", "Rockin" },
            {"You're a rock.", "Sparken" },
            {"Huh?", "Rockin" },
            {"You're actually just a rock.", "Sparken" },
            {"Hmm... It sounds like you need to be convinced.", "Rockin" },
            {"No. I just need to leave.", "Sparken" },
            {"Here, a demonstration!", "Rockin" }
        };
        cutsceneRemarks[7] = new string[,]
        {
            {"We're actually doing this.", "Sparken" },
            {"Damn it.", "Sparken" }
        };
        cutsceneRemarks[8] = new string[,]
        {
            {"Well, that's that I guess.", "Sparken" },
            {"Idiotic fanatics.", "Sparken" },
            {"I've got a bad feeling about this already.", "Sparken" }
        };
        cutsceneRemarks[9] = new string[,]
        {
            {"That's a massive sign...", "Sparken" },
            {"'Sacred Village ahead'.", "Sparken" },
            {"Bad handwriting too", "Sparken" },
            {"The writer probably didn't have hands.", "Sparken" },
            {"Spooky.", "Sparken" },
            {"Anyway, looks like we're almost there.", "Sparken" }
        };
    }

    // Sends the information for cutscenes to the dialoguemanager
    public object[,] getCutsceneRemarks(int key)
    {
        if (loaded == false)
        {
            loadRemarks();
            loaded = true;
        }

        scene = SceneManager.GetActiveScene();
        sceneName = scene.name;
        return cutsceneRemarks[key];
    }

}
