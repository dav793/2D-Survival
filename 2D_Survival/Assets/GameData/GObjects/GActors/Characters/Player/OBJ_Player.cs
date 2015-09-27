using UnityEngine;
using System.Collections;

public class OBJ_Player : GCharacter {

	public OBJ_Player() : base(
		GActorProperties.GetDefaultProperties(GActorPropertiesType.nonNPC)
	) {
		Init ();
	}

	public OBJ_Player(string gameObject_name) : base(
		GActorProperties.GetDefaultProperties(GActorPropertiesType.nonNPC)
	) {
		gameObjectName = gameObject_name;
		Init ();
	}

	void Init() {
		max_speed = 2f;
		type = GObjectType.Player;
	}

}
