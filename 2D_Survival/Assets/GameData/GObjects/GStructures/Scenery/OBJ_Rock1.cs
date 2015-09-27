using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OBJ_Rock1 : GStructure {

	public OBJ_Rock1() : base(
		GStructureProperties.GetDefaultProperties(GStructurePropertiesType.BlockingSceneryProp),
		GStructureDimensions.GetDefaultDimensions(GStructureDimensionsType.Medium)
	) {
		resource_identifiers = new Pair<string, Dictionary<CardinalDirections, List<string>>> (
			"Rock1",
			new Dictionary<CardinalDirections, List<string>> () {
				{CardinalDirections.S, new List<string> (new string[] {""})}
			}
		); 
	}

}
