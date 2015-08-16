using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteDatabase : MonoBehaviour {

	public static SpriteDatabase sprites;

	public Sprite default_object;
	public Sprite small_crate;
	public Sprite rock_item;

	public void Awake() {
		sprites = this;
	}

}
