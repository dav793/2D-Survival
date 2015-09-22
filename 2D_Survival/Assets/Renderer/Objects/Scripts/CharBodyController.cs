using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharBodyController : ObjectAnimController {
	
	// Head
	public Animator_Clip_Pair Hat;
	public Animator_Clip_Pair Hair;
	public Animator_Clip_Pair Eyewear;
	public Animator_Clip_Pair Mask;
	public Animator_Clip_Pair Head;

	//Body
	public Animator_Clip_Pair Neck;

	// Arms
	public Animator_Clip_Pair RHand;
	public Animator_Clip_Pair LHand;
	public Animator_Clip_Pair JSleeves;
	public Animator_Clip_Pair Gloves;
	public Animator_Clip_Pair Sleeves;
	public Animator_Clip_Pair Arms;

	// Torso
	public Animator_Clip_Pair Jacket;
	public Animator_Clip_Pair Bodysuit;
	public Animator_Clip_Pair Torso;

	// Feet
	public Animator_Clip_Pair Shoes;
	public Animator_Clip_Pair Feet;

	// Legs
	public Animator_Clip_Pair Pants;
	public Animator_Clip_Pair Legs;

	public void Init(GObject gobj) {
		linked_gobj = gobj;
		grobj = linked_gobj.renderedGameObject.GetComponent<GRObject> ();
		initAnimClipPairs ();
		setDefaultClips ();
	 }

	public override void updateClips() {
		updateBodyClips ();
		updateEquippedItemClips ();
	}

	void initAnimClipPairs() {
		Hat.setClip (AnimationControllers.AnimControllers.defaultClip);
		Hair.setClip (AnimationControllers.AnimControllers.defaultClip);
		Eyewear.setClip (AnimationControllers.AnimControllers.defaultClip);
		Mask.setClip (AnimationControllers.AnimControllers.defaultClip);
		Head.setClip (AnimationControllers.AnimControllers.defaultClip);
		Neck.setClip (AnimationControllers.AnimControllers.defaultClip);
		RHand.setClip (AnimationControllers.AnimControllers.defaultClip);
		LHand.setClip (AnimationControllers.AnimControllers.defaultClip);
		JSleeves.setClip (AnimationControllers.AnimControllers.defaultClip);
		Gloves.setClip (AnimationControllers.AnimControllers.defaultClip);
		Sleeves.setClip (AnimationControllers.AnimControllers.defaultClip);
		Arms.setClip (AnimationControllers.AnimControllers.defaultClip);
		Jacket.setClip (AnimationControllers.AnimControllers.defaultClip);
		Bodysuit.setClip (AnimationControllers.AnimControllers.defaultClip);
		Torso.setClip (AnimationControllers.AnimControllers.defaultClip);
		Shoes.setClip (AnimationControllers.AnimControllers.defaultClip);
		Feet.setClip (AnimationControllers.AnimControllers.defaultClip);
		Pants.setClip (AnimationControllers.AnimControllers.defaultClip);
		Legs.setClip (AnimationControllers.AnimControllers.defaultClip);
	}

	void setDefaultClips() {
		setDefaultClip (CharBodyPart.Hair, Hair);
		setDefaultClip (CharBodyPart.Head, Head);
		setDefaultClip (CharBodyPart.Arms, Arms);
		setDefaultClip (CharBodyPart.Torso, Torso);
		setDefaultClip (CharBodyPart.Feet, Feet);
		setDefaultClip (CharBodyPart.Legs, Legs);
	}

	void setDefaultClip(CharBodyPart slot, Animator_Clip_Pair animClip) {
		string res_identifier = ((GCharacter)linked_gobj).equipped_items.getDefaultResourceIdentifier (slot, (GCharacter)linked_gobj);
		if (res_identifier != "invalid") {
			changeAnimClip (animClip, GetClipFromResources (res_identifier, "IdRight"));
		}
	}

	void updateBodyClips() {
		updateBodypartClip(CharBodyPart.Hair, Hair, "Hair");
		updateBodypartClip(CharBodyPart.Head, Head, "Head");
		updateBodypartClip(CharBodyPart.Arms, Arms, "Arms");
		updateBodypartClip(CharBodyPart.Torso, Torso, "Torso");
		updateBodypartClip(CharBodyPart.Feet, Feet, "Feet");
		updateBodypartClip(CharBodyPart.Legs, Legs, "Legs");
	}

	void updateBodypartClip(CharBodyPart slot, Animator_Clip_Pair animClipPair, string bodypart) {
		string res_identifier = ((GCharacter)linked_gobj).equipped_items.getDefaultResourceIdentifier (slot, (GCharacter)linked_gobj);
		if(res_identifier != "invalid" && animClipPair.getClip().name != getClipName(res_identifier, bodypart)) {
			changeAnimClip (animClipPair, GetClipFromResources(res_identifier, getBodypartClipSuffix(bodypart)));
		}
	}

	void updateEquippedItemClips() {
		EquippedSlots equipped_items = ((GCharacter)linked_gobj).equipped_items;
		GEquippableItem[] slots = equipped_items.getSlots ();
		for (int i = 0; i < slots.Length; ++i) {
			if(equipped_items.slotContainsAnyItem ((CharacterSlots)i)) {
				foreach(KeyValuePair<CharacterSlots, string> res in equipped_items.getAtSlot((CharacterSlots)i).resource_identifiers) {
					string res_identifier = res.Value;
					updateEquippedItemsAtSlot (res.Key, res_identifier);
				}
			}
			else {
				// No item present at slot i
			}
		}
	}

	void updateEquippedItemsAtSlot(CharacterSlots slot, string resource_identifier) {
		/*if (getAnimClipPair (slot) == null) {
			Debug.Log (slot);
		}*/
		if (getAnimClipPair (slot).getClip ().name != getClipName (resource_identifier, getBodypartName (slot))) {
			changeAnimClip(getAnimClipPair (slot), GetClipFromResources(resource_identifier, getBodypartClipSuffix(getBodypartName(slot))));
		}
	}

	Animator_Clip_Pair getAnimClipPair(CharacterSlots slot) {
		switch (slot) {
		case CharacterSlots.Bodysuit:
			return Bodysuit;
			break;
		case CharacterSlots.Eyewear:
			return Eyewear;
			break;
		case CharacterSlots.Gloves:
			return Gloves;
			break;
		case CharacterSlots.Hair:
			return Hair;
			break;
		case CharacterSlots.Hat:
			return Hat;
			break;
		case CharacterSlots.JacketTorso:
			return Jacket;
			break;
		case CharacterSlots.JSleeves:
			return JSleeves;
			break;
		case CharacterSlots.LHand:
			return LHand;
			break;
		case CharacterSlots.RHand:
			return RHand;
			break;
		case CharacterSlots.Shoes:
			return Shoes;
			break;
		case CharacterSlots.Sleeves:
			return Sleeves;
			break;
		case CharacterSlots.Mask:
			return Mask;
			break;
		case CharacterSlots.Pants:
			return Pants;
			break;
		}
		return null;
	}

	string getBodypartName(CharacterSlots slot) {
		switch (slot) {
		case CharacterSlots.Bodysuit:
			return "torso";
			break;
		case CharacterSlots.Eyewear:
			return "head";
			break;
		case CharacterSlots.Gloves:
			return "arms";
			break;
		case CharacterSlots.Hair:
			return "head";
			break;
		case CharacterSlots.Hat:
			return "head";
			break;
		case CharacterSlots.JacketTorso:
			return "torso";
			break;
		case CharacterSlots.JSleeves:
			return "arms";
			break;
		case CharacterSlots.LHand:
			return "arms";
			break;
		case CharacterSlots.RHand:
			return "arms";
			break;
		case CharacterSlots.Shoes:
			return "feet";
			break;
		case CharacterSlots.Sleeves:
			return "arms";
			break;
		case CharacterSlots.Mask:
			return "head";
			break;
		case CharacterSlots.Pants:
			return "legs";
			break;
		}
		return null;
	}

}
