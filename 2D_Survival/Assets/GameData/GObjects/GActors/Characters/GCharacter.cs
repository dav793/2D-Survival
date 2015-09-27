using UnityEngine;
using System.Collections;

public class GCharacter : GActor {

	public EquippedSlots equipped_items;

	public GCharacter() : base(
		GActorProperties.GetDefaultProperties(GActorPropertiesType.NPC)
	) {
		Init ();
	}

	public GCharacter(GActorProperties properties) : base(properties) {
		Init ();
	}

	void Init() {
		sprite = SpriteDatabase.sprites.default_object;
		type = GObjectType.Character;
		equipped_items = new EquippedSlots ();
		equipDefaultItems ();
	}

	void equipDefaultItems() {
		equipped_items.equipItem (new OBJ_Jeans1());
		equipped_items.equipItem (new OBJ_Shoes1());
		equipped_items.equipItem (new OBJ_Shirt1());

		//equipped_items.printEquippedItems ();
	}

}
