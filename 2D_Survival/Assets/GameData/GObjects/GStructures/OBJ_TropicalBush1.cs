using UnityEngine;
using System.Collections;

public class OBJ_TropicalBush1 : GStructure {

	static BOOL_YN interactive = BOOL_YN.YES;
	static BOOL_YN movable = BOOL_YN.NO;
	static BOOL_YN environmental = BOOL_YN.YES;
	
	public OBJ_TropicalBush1() : base(OBJ_TropicalBush1.interactive, OBJ_TropicalBush1.movable, OBJ_TropicalBush1.environmental) {
		sprite = SpriteDatabase.sprites.bush_1;
	}

}
