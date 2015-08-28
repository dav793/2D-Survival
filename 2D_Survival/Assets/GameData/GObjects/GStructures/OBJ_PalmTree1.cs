using UnityEngine;
using System.Collections;

public class OBJ_PalmTree1 : GStructure {
		
	static BOOL_YN interactive = BOOL_YN.YES;
	static BOOL_YN movable = BOOL_YN.NO;
	static BOOL_YN environmental = BOOL_YN.YES;
	
	public OBJ_PalmTree1() : base(OBJ_PalmTree1.interactive, OBJ_PalmTree1.movable, OBJ_PalmTree1.environmental) {
		sprite = SpriteDatabase.sprites.tropical_tree_2;
	}

}
