using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteDatabase : MonoBehaviour {

	public static SpriteDatabase sprites;

	public Sprite default_object;
	public Sprite small_crate;
	public Sprite rock_item;
	public Sprite tropical_tree_1;
	public Sprite tropical_tree_2;
	public Sprite tropical_tree_3;
	public Sprite large_tree_1;
	public Sprite bush_1;
	public Sprite bush_2;
	public Sprite bush_3;

	public void Awake() {
		sprites = this;
	}

}
