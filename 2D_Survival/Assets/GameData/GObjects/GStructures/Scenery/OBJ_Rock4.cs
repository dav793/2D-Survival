using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OBJ_Rock4 : GStructure {

	public OBJ_Rock4() : base(
		GStructureProperties.GetDefaultProperties(GStructurePropertiesType.NonBlockingSceneryProp),
		GStructureDimensions.GetDefaultDimensions(GStructureDimensionsType.Small)
	) {
		resource_identifiers = new Pair<string, Dictionary<CardinalDirections, List<string>>> (
			"Rock4",
			new Dictionary<CardinalDirections, List<string>> () {
				{CardinalDirections.S, new List<string> (new string[] {""})}
			}
		); 
	}

}
