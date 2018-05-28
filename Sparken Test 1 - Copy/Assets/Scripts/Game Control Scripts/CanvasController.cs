using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script simply sets the canvas as a singleton.

public class CanvasController : MonoBehaviour {


    //Creates the canvas singleton
    static CanvasController canvasController;

    void Awake()
    {
        if (canvasController == null)
        {
            DontDestroyOnLoad(gameObject);
            canvasController = this;
        }
        else if (canvasController != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
