using UnityEngine;
using System.Collections;

public class AnimationControllers : MonoBehaviour {

	public static AnimationControllers AnimControllers;
	
	public AnimationClip defaultClip;

	//public CharBodyPart_AnimClips Rabbit1;

	public void Awake() {
		AnimControllers = this;
	}

}

[System.Serializable]
public class CharBodyPart_AnimClips{
	public AnimationClip idle_back;
}
