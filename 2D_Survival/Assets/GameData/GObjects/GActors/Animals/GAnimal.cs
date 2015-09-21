using UnityEngine;
using System.Collections;

public class GAnimal : GActor {
	
	public GAnimal() : base(BOOL_YN.YES, BOOL_YN.NO) {
		sprite = SpriteDatabase.sprites.default_object;
		type = GObjectType.Animal;
	}

}
