using UnityEngine;
using System.Collections;

public class OBJ_PalmTree2 : GStructure {
	
	static BOOL_YN interactive = BOOL_YN.YES;
	static BOOL_YN movable = BOOL_YN.NO;
	static BOOL_YN environmental = BOOL_YN.YES;
	
	public OBJ_PalmTree2() : base(OBJ_PalmTree2.interactive, OBJ_PalmTree2.movable, OBJ_PalmTree2.environmental) {
		sprite = SpriteDatabase.sprites.tropical_tree_3;
	}
	
}
