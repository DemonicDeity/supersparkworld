using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script called when existing a jump
public class JumpExit : StateMachineBehaviour {


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        //Sets action to 0 if action is not equal to 4
        if (animator.GetInteger("Action") != 4)
        {
            animator.SetInteger("Action", 0);
        }
	}
}
