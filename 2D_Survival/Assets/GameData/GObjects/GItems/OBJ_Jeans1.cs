using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OBJ_Jeans1 : GEquippableItem {

	public OBJ_Jeans1() {
		resource_identifiers = new Dictionary<CharacterSlots, string> () {
			{CharacterSlots.Pants, "BlackPants1"}
		};
	}

	public override string getDebug() {
		return "Jeans1";
	}

}
