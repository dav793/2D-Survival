using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneralGObjectController : MonoBehaviour {

	public CardinalDirections face_direction;
	public SpriteRenderer spr_renderer;
	public GObject obj;

	public virtual void execTick() {

	}
	
	void FixedUpdate () {
		execTick ();
	}
	
	public void Init() {
		spr_renderer = obj.renderedGameObject.GetComponent<SpriteRenderer> ();
		spr_renderer.sprite = obj.sprite;

		updateSprite ();
	}

	public void linkGObject(GObject gobj) {
		obj = gobj;
	}

	public virtual void updateSprite() {

	}

	public static string GetPreSuffixFromOrientation(CardinalDirections orientation){
		switch (orientation) {
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

	public static Sprite GetSpriteFromResources(string sprite_prefix, string sprite_suffix) {
		string separator = "_";
		if (sprite_suffix == "") {
			separator = "";
		}

		Sprite[] textures = Resources.LoadAll<Sprite> ("Sprites/" + sprite_prefix + "/" + sprite_prefix + separator + sprite_suffix);
		if (textures.Length == 0) {
			Debug.LogError("Error loading resource: Sprites/" + sprite_prefix + "/" + sprite_prefix + separator + sprite_suffix);
			return null;
		}
		return textures[0];
	}

}
