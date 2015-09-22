using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GEquippableItem : GItem {

	public Dictionary<CharacterSlots, string> resource_identifiers;

	public GEquippableItem() {
		//resource_identifiers = new Dictionary<EquippableSlot, string> ();
	}

	public override bool isEquippable() {
		return true;
	}

}
