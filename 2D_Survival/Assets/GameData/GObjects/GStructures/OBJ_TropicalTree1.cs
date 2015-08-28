using UnityEngine;
using System.Collections;

public class OBJ_TropicalTree1 : GStructure {

	static BOOL_YN interactive = BOOL_YN.YES;
	static BOOL_YN movable = BOOL_YN.NO;
	static BOOL_YN environmental = BOOL_YN.YES;
	
	public OBJ_TropicalTree1() : base(OBJ_TropicalTree1.interactive, OBJ_TropicalTree1.movable, OBJ_TropicalTree1.environmental) {
		sprite = SpriteDatabase.sprites.tropical_tree_1;
	}

}
