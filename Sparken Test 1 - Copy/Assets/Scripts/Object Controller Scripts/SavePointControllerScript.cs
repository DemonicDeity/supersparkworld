using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script to control the scene and position of save points
public class SavePointControllerScript {

    string[] savePointScene; // The scenes that the save points are on
    Vector3[] savePointPlace; // The position that the save points are at
    object[] loadedSavePoint; // Packages the scenes and positions to send to the controller
    bool loaded = false; // Determines if the savepoints have been loaded yet

    // Places all of the save point data into the arrays
    private void loadSavePoints()
    {
        loadedSavePoint = new object[2];
        savePointScene = new string[10];
        savePointPlace = new Vector3[10];
        savePointScene[0] = "Scene 1";
        savePointPlace[0] = new Vector3(0.05f, 0.3f, 0);
        savePointScene[1] = "scene 3";
        savePointPlace[1] = new Vector3(-10, 0.53f, 0);
    }

    //Load the save point data of a index and send it back
    public object[] getSavePoints(int loadingIndex)
    {
        if (loaded != true)
        {
            loadSavePoints();
            loaded = true;
        }
        loadedSavePoint[0] = savePointScene[loadingIndex];
        loadedSavePoint[1] = savePointPlace[loadingIndex];
        return loadedSavePoint;
    }
}
