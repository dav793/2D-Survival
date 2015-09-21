using UnityEngine;
using System.Collections;

public class AnimationControllers : MonoBehaviour {

	public static AnimationControllers AnimControllers;

	public RuntimeAnimatorController player;
	public RuntimeAnimatorController character;
	public RuntimeAnimatorController animal;
	public RuntimeAnimatorController charBodyPart;

	public AnimationClip defaultClip;
	public CharBodyPart_AnimClips MaleHair1;
	public CharBodyPart_AnimClips MaleHead1;
	public CharBodyPart_AnimClips MaleArms1;
	public CharBodyPart_AnimClips MaleTorso1;
	public CharBodyPart_AnimClips MaleFeet1;
	public CharBodyPart_AnimClips MaleLegs1;

	public CharBodyPart_AnimClips Rabbit1;

	public void Awake() {
		AnimControllers = this;
	}

}

[System.Serializable]
public class CharBodyPart_AnimClips{
	public AnimationClip idle_back;
	public AnimationClip idle_front;
	public AnimationClip idle_right;
	public AnimationClip running_back;
	public AnimationClip running_front;
	public AnimationClip running_right;
}
