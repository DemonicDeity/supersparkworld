using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Controls the position of the pause menu
public class PauseMenuController : MonoBehaviour {

    static PauseMenuController pauseMenuController; // Sets singleton
    GameObject mainCamera; // Sets maincamera

    Vector3 setPosition;

    // Use this for initialization
    void Awake()
    {
        if (pauseMenuController == null)
        {
            DontDestroyOnLoad(gameObject);
            pauseMenuController = this;
        }
        else if (pauseMenuController != this)
        {
            Destroy(gameObject);
        }
    }

    void Start () {
        mainCamera = GameObject.Find("Main Camera");

        setPosition.x = mainCamera.transform.position.x;
        setPosition.y = mainCamera.transform.position.y;
        setPosition.z = -2;
        transform.position = setPosition;

    }

    private void OnEnable()
    {
        //Centers the pause menu on the camera
        setPosition.x = mainCamera.transform.position.x;
        setPosition.y = mainCamera.transform.position.y;
        transform.position = setPosition;

    }

    // Update is called once per frame
    void Update () {
		
	}
}
