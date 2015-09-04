using UnityEngine;
using System.Collections;

public class AnimationControllers : MonoBehaviour {

	public static AnimationControllers AnimControllers;

	public RuntimeAnimatorController player;
	public RuntimeAnimatorController character;
	public RuntimeAnimatorController animal;

	public void Awake() {
		AnimControllers = this;
	}

}
