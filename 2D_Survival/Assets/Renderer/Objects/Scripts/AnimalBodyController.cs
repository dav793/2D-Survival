using UnityEngine;
using System.Collections;

public class AnimalBodyController : ObjectAnimController {

	//Body
	public Animator_Clip_Pair Body;

	public void Init(GObject gobj) {
		linked_gobj = gobj;
		grobj = linked_gobj.renderedGameObject.GetComponent<GRObject> ();
		setDefaultAnims ();
	}

	public override void updateClips() {
		updateBody ();
	}

	void setDefaultAnims() {
		
		Body.setClip (AnimationControllers.AnimControllers.defaultClip);
		changeClip (Body, GetClipFromResources ("Rabbit1", "IdBack"));
		
	}

	void updateBody() {

		string newClipPrefix = "Rabbit1";
		string newClipSuffix = getBodypartClipSuffix("Torso");
		
		if (Body.getClip () != GetClipFromResources (newClipPrefix, newClipSuffix)) {
			changeClip (Body, GetClipFromResources (newClipPrefix, newClipSuffix));
		}

	}

}
