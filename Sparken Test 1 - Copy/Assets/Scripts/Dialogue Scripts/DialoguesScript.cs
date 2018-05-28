using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that holds dialogue for characters on the map
[System.Serializable]
public class DialoguesScript : MonoBehaviour {

    public string name;

    [TextArea(3, 10)]
    public string[] sentences;
}
