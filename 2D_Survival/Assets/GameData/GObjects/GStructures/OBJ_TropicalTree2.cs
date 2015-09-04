using UnityEngine;
using System.Collections;

public class OBJ_TropicalTree2 : GStructure {
	
	static BOOL_YN interactive = BOOL_YN.YES;
	static BOOL_YN movable = BOOL_YN.NO;
	static BOOL_YN environmental = BOOL_YN.YES;
	
	public OBJ_TropicalTree2() : base(OBJ_TropicalTree2.interactive, OBJ_TropicalTree2.movable, OBJ_TropicalTree2.environmental) {
		sprite = SpriteDatabase.sprites.large_tree_1;
	}
	
}
