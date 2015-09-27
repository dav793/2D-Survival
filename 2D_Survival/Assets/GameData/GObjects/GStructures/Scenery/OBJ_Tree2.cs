using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OBJ_Tree2 : GStructure {

	public OBJ_Tree2() : base(
		GStructureProperties.GetDefaultProperties(GStructurePropertiesType.BlockingVegetation),
		GStructureDimensions.GetDefaultDimensions(GStructureDimensionsType.Medium)
	) {
		resource_identifiers = new Pair<string, Dictionary<CardinalDirections, List<string>>> (
			"Tree2",
			new Dictionary<CardinalDirections, List<string>> () {
				{CardinalDirections.S, new List<string> (new string[] {""})}
			}
		); 
	}

}
