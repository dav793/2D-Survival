using UnityEngine;
using System.Collections;

public class ObjectAnimController : MonoBehaviour {

	public GObject linked_gobj;
	public GRObject grobj;

	public void changeClip(Animator_Clip_Pair anim, AnimationClip newClip) {
		AnimatorOverrideController ov_ctrl = new AnimatorOverrideController ();
		ov_ctrl.runtimeAnimatorController = anim.animator.runtimeAnimatorController;
		ov_ctrl [anim.getClip()] = newClip;
		anim.setClip (newClip);
		anim.animator.runtimeAnimatorController = ov_ctrl;
	}

	public static string GetPostSuffixFromFaceDirection(CardinalDirections face_direction){
		switch (face_direction) {
		case CardinalDirections.N:
			return "Back";
			break;
		case CardinalDirections.S:
			return "Front";
			break;
		default:
			return "Right";
		}
	}

	public static AnimationClip GetClipFromResources(string clip_prefix, string clip_suffix) {
		return Resources.Load ("AnimationClips/"+clip_prefix+"/"+clip_prefix+"_"+clip_suffix) as AnimationClip;
	}

	public string getBodypartClipSuffix(string bodypart) {
		string clipSuffix = "";
		switch (grobj.getActorControllerIfExists ().getHighestPriorityActionOnBodypart (bodypart.ToLower())) {
		case "idle":
			clipSuffix = "Id" + GetPostSuffixFromFaceDirection(grobj.getActorControllerIfExists().face_direction);
			break;
		case "running":
			clipSuffix = "Run" + GetPostSuffixFromFaceDirection(grobj.getActorControllerIfExists().face_direction);
			break;
		}
		return clipSuffix;
	}

	public virtual void updateClips() {}

}
