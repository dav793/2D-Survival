using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquippedSlots {
	
	GEquippableItem[] slots;
	
	public EquippedSlots() {
		int num_slots = System.Enum.GetNames (typeof(CharacterSlots)).Length;
		slots = new GEquippableItem[num_slots];
		for (int i = 0; i < num_slots; ++i) {
			slots[i] = null;
		}
	}
	
	public GEquippableItem[] getSlots() {
		return slots;
	}
	
	public void equipItem(GEquippableItem item) {
		foreach(KeyValuePair<CharacterSlots, string> entry in item.resource_identifiers) {
			if(slotContainsAnyItem(entry.Key)) {
				unequipSlot(entry.Key);
			}
			slots[(int)entry.Key] = item;
		}
	}
	
	public void unequipSlot(CharacterSlots slot) {
		GEquippableItem currentItem = slots [(int)slot];
		foreach (KeyValuePair<CharacterSlots, string> entry in currentItem.resource_identifiers) {
			slots[(int)entry.Key] = null;
		}
	}
	
	public bool slotContainsAnyItem(CharacterSlots slot) {
		if (slots [(int)slot] == null) {
			return false;
		}
		return true;
	}
	
	public GEquippableItem getAtSlot(CharacterSlots slot) {
		if (slots [(int)slot] == null) {
			return null;
		} 
		return slots[(int)slot];
	} 
	
	public string getDefaultResourceIdentifier(CharBodyPart slot, GCharacter character) {
		switch(slot) {
		case CharBodyPart.Arms:
			return "MaleArms1";
			break;
		case CharBodyPart.Head:
			return "MaleHead1";
			break;
		case CharBodyPart.Hair:
			return "MaleHairBrown1";
			break;
		case CharBodyPart.Torso:
			return "MaleTorso1";
			break;
		case CharBodyPart.Legs:
			return "MaleLegs1";
			break;
		case CharBodyPart.Feet:
			return "MaleFeet1";
			break;
		}
		return "invalid";
	}
	
	public void printEquippedItems() {
		string output = "";
		
		for (int i = 0; i < System.Enum.GetNames (typeof(CharacterSlots)).Length; ++i) {
			string itemName = "None";
			if(slotContainsAnyItem((CharacterSlots)i)) {
				itemName = slots[i].getDebug();
			}
			output = output + (CharacterSlots)i + ": " + itemName + "\n";
		}
		
		Debug.Log (output);
	}
	
}
