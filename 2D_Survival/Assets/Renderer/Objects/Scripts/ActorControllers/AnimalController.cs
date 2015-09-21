using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class AnimalController : ActorController {

	public Dictionary <string, Dictionary <string, int>> bodypart_anim_priorities;

	public override void execTick() {
		base.execTick ();
		//printDebug ();
	}
	
	void FixedUpdate() {
		execTick ();
	}

	void OnEnable() {
		base.Init ();
		Init ();
	}

	public void Init() {
		bodypart_anim_priorities = new Dictionary<string, Dictionary <string, int>> () {
			
			{"idle", new Dictionary<string, int>() {
					{"head", 0},
					{"torso", 0}
				}},
			
			{"running", new Dictionary<string, int>() {
					{"head", 1},
					{"torso", 1}
				}}
			
		};
	}

	public override Dictionary <string, Dictionary <string, int>> getBodypartAnimPriorities() {
		return bodypart_anim_priorities;
	}

	// FOR TESTS
	public override void printDebug() {
		base.printDebug ();
		string output = "\t";
		output = output + "head: " + getHighestPriorityActionOnBodypart("head") + "\n\t";
		output = output + "torso: " + getHighestPriorityActionOnBodypart("torso") + "\n\t";
		UIManager.UI.DebugConsole1.transform.Find("Text").GetComponent<Text> ().text = UIManager.UI.DebugConsole1.transform.Find("Text").GetComponent<Text> ().text + output;
	}
	//END TESTS

}
