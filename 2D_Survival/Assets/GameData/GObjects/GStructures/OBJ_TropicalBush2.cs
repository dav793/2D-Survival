using UnityEngine;
using System.Collections;

public class OBJ_TropicalBush2 : GStructure {

	static BOOL_YN interactive = BOOL_YN.YES;
	static BOOL_YN movable = BOOL_YN.NO;
	static BOOL_YN environmental = BOOL_YN.YES;
	
	public OBJ_TropicalBush2() : base(OBJ_TropicalBush2.interactive, OBJ_TropicalBush2.movable, OBJ_TropicalBush2.environmental) {
		sprite = SpriteDatabase.sprites.bush_2;
	}

}
