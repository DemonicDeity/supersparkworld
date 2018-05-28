using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Building Exit is called when the Sparken exits the building animation.
public class BuildingExitScript : StateMachineBehaviour {


	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        // If the building animation was finished before the Lightning Conductor was finished, it sets the Lightning Conductor to destroy itself.
	if (LightningConductorController.lightningConductorController.status == 0)
        {
            LightningConductorController.lightningConductorController = null;
        }
	}
}
