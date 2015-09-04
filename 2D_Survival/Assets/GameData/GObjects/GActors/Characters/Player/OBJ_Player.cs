using UnityEngine;
using System.Collections;

public class OBJ_Player : GCharacter {
	
	static BOOL_YN npc = BOOL_YN.NO;
	static BOOL_YN environmental = BOOL_YN.NO;

	public OBJ_Player() : base(OBJ_Player.environmental, OBJ_Player.npc) {
		max_speed = 1f;
	}

}
