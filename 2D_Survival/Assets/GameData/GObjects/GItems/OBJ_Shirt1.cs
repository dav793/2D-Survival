using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OBJ_Shirt1 : GEquippableItem {

	public OBJ_Shirt1() {
		resource_identifiers = new Dictionary<CharacterSlots, string> () {
			{CharacterSlots.Bodysuit, "BrownVest1"},
			{CharacterSlots.Sleeves, "BrownSleeves1"}
		};
	}

	public override string getDebug() {
		return "Shirt1";
	}

}
