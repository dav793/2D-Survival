using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OBJ_Shoes1 : GEquippableItem {

	public OBJ_Shoes1() {
		resource_identifiers = new Dictionary<CharacterSlots, string> () {
			{CharacterSlots.Shoes, "RedShoes1"}
		};
	}

	public override string getDebug() {
		return "Shoes1";
	}

}
