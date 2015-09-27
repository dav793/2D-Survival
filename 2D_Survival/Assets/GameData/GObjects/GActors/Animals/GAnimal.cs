using UnityEngine;
using System.Collections;

public class GAnimal : GActor {
	
	public GAnimal() : base(
		GActorProperties.GetDefaultProperties(GActorPropertiesType.Animal)
	) {
		sprite = SpriteDatabase.sprites.default_object;
		type = GObjectType.Animal;
	}

}
