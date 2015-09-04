using UnityEngine;
using System.Collections;

public class OBJ_TropicalBush3 : GStructure {
	
	static BOOL_YN interactive = BOOL_YN.YES;
	static BOOL_YN movable = BOOL_YN.NO;
	static BOOL_YN environmental = BOOL_YN.YES;
	
	public OBJ_TropicalBush3() : base(OBJ_TropicalBush3.interactive, OBJ_TropicalBush3.movable, OBJ_TropicalBush3.environmental) {
		sprite = SpriteDatabase.sprites.bush_3;
	}
	
}
