using UnityEngine;
using System.Collections;

public class GeneralGObjectController : MonoBehaviour {

	public CardinalDirections face_direction;
	public SpriteRenderer spr_renderer;

	GObject obj;

	public virtual void execTick() {

	}
	
	void FixedUpdate () {
		execTick ();
	}
	
	public void Init() {
		spr_renderer = obj.renderedGameObject.GetComponent<SpriteRenderer> ();
		spr_renderer.sprite = obj.sprite;
	}

	public void linkGObject(GObject gobj) {
		obj = gobj;
	}

}
