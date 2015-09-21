using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CharacController : ActorController {

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
				{"hair", 0},
				{"eyes", 0},
				{"face", 0},
				{"head", 0},
				{"neck", 0},
				{"arms", 0},
				{"torso", 0},
				{"feet", 0},
				{"legs", 0}
			}},

			{"running", new Dictionary<string, int>() {
				{"hair", 1},
				{"eyes", 1},
				{"face", 1},
				{"head", 1},
				{"neck", 1},
				{"arms", 1},
				{"torso", 1},
				{"feet", 1},
				{"legs", 1}
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
		output = output + "hair: " + getHighestPriorityActionOnBodypart("hair") + "\n\t";
		output = output + "eyes: " + getHighestPriorityActionOnBodypart("eyes") + "\n\t";
		output = output + "face: " + getHighestPriorityActionOnBodypart("face") + "\n\t";
		output = output + "head: " + getHighestPriorityActionOnBodypart("head") + "\n\t";
		output = output + "neck: " + getHighestPriorityActionOnBodypart("neck") + "\n\t";
		output = output + "arms: " + getHighestPriorityActionOnBodypart("arms") + "\n\t";
		output = output + "torso: " + getHighestPriorityActionOnBodypart("torso") + "\n\t";
		output = output + "feet: " + getHighestPriorityActionOnBodypart("feet") + "\n\t";
		output = output + "legs: " + getHighestPriorityActionOnBodypart("legs") + "\n";
		UIManager.UI.DebugConsole1.transform.Find("Text").GetComponent<Text> ().text = UIManager.UI.DebugConsole1.transform.Find("Text").GetComponent<Text> ().text + output;
	}
	//END TESTS

}
