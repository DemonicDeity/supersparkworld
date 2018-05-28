using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to control camera position and movements
public class CameraControl : MonoBehaviour {

    static CameraControl cameraControl;

    public float backgroundSize; // Size of background
    private Transform player; // Sparken transform
    private Camera camera; // The camera. This is unnecessary I think.
    public bool isFollowing; // Sets whether the camera follows the player
    private Vector3 originalOffset; // The original offset of the camera (if offset changes)
    private Vector3 offset; // Offset position of the Camera
    private Vector3 min, max; // Minimum and Maximum positions of the camera
    private Renderer rend; // The renderer of the background
    private GameObject map1; // The gameobject of the background
    private Vector3 cameraPosition; // New position of the camera when it shakes
    public int cameraShakeDuration; // Duration the camera shakes for

    float edgeOfLeft;
    float edgeOfRight;
    float edgeOfTop;
    float edgeOfBottom;


    // Singleton design pattern
    void Awake()
    {
        if (cameraControl == null)
        {
            DontDestroyOnLoad(gameObject);
            cameraControl = this;
        }
        else if (cameraControl != this)
        {
            Destroy(gameObject);
        }
    }

    // Used to reload values on scene changes
    void reload () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        map1 = GameObject.FindGameObjectWithTag("Background");
        rend = map1.GetComponent<Renderer>();
        camera = GetComponent<Camera>();
        offset = new Vector3(0, 1f, -8);
        originalOffset = new Vector3(0, 1f, -8);
        cameraPosition = offset;

        isFollowing = true;

        min = rend.bounds.min;
        max = rend.bounds.max;

        edgeOfBottom = 3.7f;
        edgeOfTop = 5.5f;
        edgeOfLeft = 6.3f;
        edgeOfRight = 6.3f;

    }
	
	// Update is called once per frame
	void LateUpdate () {
        // Reloads if it does not have values
        if (player == null)
        {
            reload();
        }
        
        // Determines whether to follow the player
        if (isFollowing == true)
        {
            
            // Call cameraShake to determine offset changes
            cameraShake();

            // Large set of functions which determine if the Camera has hit the edge of the scene. If it has, it will lock to the edge of the scene.
            if (player.transform.position.x > min.x + edgeOfLeft && player.transform.position.x < max.x - edgeOfRight)
            {
                cameraPosition.x = player.transform.position.x + offset.x;
            }
            else if (player.transform.position.x < min.x + edgeOfLeft)
            {
                cameraPosition.x = min.x + edgeOfLeft + offset.x;
            }
            else if (player.transform.position.x > max.x - edgeOfRight)
            {
                cameraPosition.x = max.x - edgeOfRight + offset.x;
            }
            if (player.transform.position.y > min.y + edgeOfBottom && player.transform.position.y < max.y - edgeOfTop)
            {
                cameraPosition.y = player.transform.position.y + offset.y;
            }
            else if (player.transform.position.y < min.y + edgeOfBottom)
            {
                cameraPosition.y = min.y + edgeOfBottom + offset.y;
            }
            else if (player.transform.position.y > max.y - edgeOfTop)
            {
                cameraPosition.y = max.y - edgeOfTop + offset.y;
            }

            transform.position = cameraPosition;
        }
    }

    // Controls the camerashake by randomizing a y distance for the offset
    public void cameraShake()
    {
        if (cameraShakeDuration > 0)
        {
            float randomShake = Random.Range(-0.1f, 0.1f);
            offset.y = originalOffset.y + randomShake;
            cameraShakeDuration--;
            if (cameraShakeDuration == 0)
            {
                offset.y = originalOffset.y;
            }
        }
    }

    // Public function to set the shakeduration
    public void setShakeDuration(int newShakeDuration)
    {
        cameraShakeDuration = newShakeDuration;
    }

}
