using UnityEngine;
using System.Collections;

public class OBJ_SmallCrate : GStructure {

	public OBJ_SmallCrate() : base(
		GStructureProperties.GetDefaultProperties(GStructurePropertiesType.InteractiveProp)
	) {
		sprite = SpriteDatabase.sprites.small_crate;
	}

}
