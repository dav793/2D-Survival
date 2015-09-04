using UnityEngine;
using System.Collections;

public class GCharacter : GActor {

	public GCharacter() : base(BOOL_YN.NO, BOOL_YN.YES) {
		sprite = SpriteDatabase.sprites.default_object;
	}

	public GCharacter(BOOL_YN environmental, BOOL_YN npc) : base(environmental, npc) {
		sprite = SpriteDatabase.sprites.default_object;
	}

}
