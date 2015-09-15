using UnityEngine;
using System.Collections;

public class OBJ_Rabbit : GAnimal {

	public OBJ_Rabbit() {
		max_speed = 1f;
	}

	public OBJ_Rabbit(string gameObject_name) {
		gameObjectName = gameObject_name;
		max_speed = 1f;
	}

}
