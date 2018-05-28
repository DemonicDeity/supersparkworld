using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A script meant to reset all hitboxes when an attack starts. I don't think it works with how the animator is set up unfortunately.
public class ResetHitboxes : StateMachineBehaviour {

    int childrenNumber; // Number of hitboxes

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        childrenNumber = animator.gameObject.transform.Find("Hitboxes").childCount - 1;

        // Loops through children and sets them to inactive
        while (childrenNumber >= 0)
        {
            animator.gameObject.transform.Find("Hitboxes").GetChild(childrenNumber).gameObject.SetActive(false);
            childrenNumber--;
        }
    }
}
