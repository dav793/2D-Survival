using UnityEngine;
using System.Collections;

public class animTest1 : MonoBehaviour {

	public AnimationClip anim_clip1;
	public AnimationClip anim_clip2;
	public AnimationClip anim_clip3;
	public Animator anim_head;
	public Animator anim_torso;	

	void Start() {

		AnimatorOverrideController ov_ctrl = new AnimatorOverrideController ();
		ov_ctrl.runtimeAnimatorController = anim_head.runtimeAnimatorController;

		ov_ctrl ["clip3"] = anim_clip2;

		anim_head.runtimeAnimatorController = ov_ctrl;

	}

}
