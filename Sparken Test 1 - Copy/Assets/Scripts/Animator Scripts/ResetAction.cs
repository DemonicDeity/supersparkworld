using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script which just sets action to 0 when an animation finishes
public class ResetAction : StateMachineBehaviour {


    public int calledAction; // Currently unused
    int childrenNumber; // Currently unused
    int action;

    // Resets action to 0
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetInteger("Action", 0);
    }
}
