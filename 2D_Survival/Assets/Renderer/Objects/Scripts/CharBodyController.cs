using UnityEngine;
using System.Collections;

public class CharBodyController : ObjectAnimController {
	
	// Head
	public Animator_Clip_Pair Face;
	public Animator_Clip_Pair Hair;
	public Animator_Clip_Pair Glasses;
	public Animator_Clip_Pair Eyes;
	public Animator_Clip_Pair Head;

	//Body
	public Animator_Clip_Pair Neck;

	// Arms
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
		setDefaultClips ();
	 }

	public override void updateClips() {
		updateHair ();
		updateHead ();
		updateArms ();
		updateTorso ();
		updateFeet ();
		updateLegs ();
	}

	void setDefaultClips() {

		Hair.setClip (AnimationControllers.AnimControllers.defaultClip);
		changeClip (Hair, GetClipFromResources ("MaleHairBrown1", "IdRight"));

		Head.setClip (AnimationControllers.AnimControllers.defaultClip);
		changeClip (Head, GetClipFromResources ("MaleHead1", "IdBack"));

		Arms.setClip (AnimationControllers.AnimControllers.defaultClip);
		changeClip (Arms, GetClipFromResources ("MaleArms1", "IdBack"));

		Torso.setClip (AnimationControllers.AnimControllers.defaultClip);
		changeClip (Torso, GetClipFromResources ("MaleTorso1", "IdBack"));

		Feet.setClip (AnimationControllers.AnimControllers.defaultClip);
		changeClip (Feet, GetClipFromResources ("MaleFeet1", "IdBack"));

		Legs.setClip (AnimationControllers.AnimControllers.defaultClip);
		changeClip (Legs, GetClipFromResources ("MaleLegs1", "IdBack"));

	}

	void updateHair() {

		string newClipPrefix = "MaleHairBrown1";
		string newClipSuffix = getBodypartClipSuffix("Hair");
		
		if (Hair.getClip () != GetClipFromResources (newClipPrefix, newClipSuffix)) {
			changeClip (Hair, GetClipFromResources (newClipPrefix, newClipSuffix));
		}

	}

	void updateHead() {

		string newClipPrefix = "MaleHead1";
		string newClipSuffix = getBodypartClipSuffix("Head");
		
		if (Head.getClip () != GetClipFromResources (newClipPrefix, newClipSuffix)) {
			changeClip (Head, GetClipFromResources (newClipPrefix, newClipSuffix));
		}

	}

	void updateArms() {
		
		string newClipPrefix = "MaleArms1";
		string newClipSuffix = getBodypartClipSuffix("Arms");
		//Debug.Log (newClipSuffix);
		if (Arms.getClip () != GetClipFromResources (newClipPrefix, newClipSuffix)) {
			changeClip (Arms, GetClipFromResources (newClipPrefix, newClipSuffix));
		}
		
	}

	void updateTorso() {
		
		string newClipPrefix = "MaleTorso1";
		string newClipSuffix = getBodypartClipSuffix("Torso");
		
		if (Torso.getClip () != GetClipFromResources (newClipPrefix, newClipSuffix)) {
			changeClip (Torso, GetClipFromResources (newClipPrefix, newClipSuffix));
		}
		
	}

	void updateFeet() {
		
		string newClipPrefix = "MaleFeet1";
		string newClipSuffix = getBodypartClipSuffix("Feet");
		
		if (Feet.getClip () != GetClipFromResources (newClipPrefix, newClipSuffix)) {
			changeClip (Feet, GetClipFromResources (newClipPrefix, newClipSuffix));
		}
		
	}

	void updateLegs() {
		
		string newClipPrefix = "MaleLegs1";
		string newClipSuffix = getBodypartClipSuffix("Legs");
		
		if (Legs.getClip () != GetClipFromResources (newClipPrefix, newClipSuffix)) {
			changeClip (Legs, GetClipFromResources (newClipPrefix, newClipSuffix));
		}
		
	}

}
