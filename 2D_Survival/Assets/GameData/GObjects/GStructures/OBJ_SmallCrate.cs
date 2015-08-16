using UnityEngine;
using System.Collections;

public class OBJ_SmallCrate : GStructure {

	static BOOL_YN interactive = BOOL_YN.YES;
	static BOOL_YN movable = BOOL_YN.YES;
	static BOOL_YN environmental = BOOL_YN.NO;

	public OBJ_SmallCrate() : base(OBJ_SmallCrate.interactive, OBJ_SmallCrate.movable, OBJ_SmallCrate.environmental) {
		sprite = SpriteDatabase.sprites.small_crate;
	}

}
