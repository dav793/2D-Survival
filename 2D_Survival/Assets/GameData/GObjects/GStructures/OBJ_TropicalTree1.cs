using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OBJ_TropicalTree1 : GStructure {
	
	public OBJ_TropicalTree1() : base(
		GStructureProperties.GetDefaultProperties(GStructurePropertiesType.BlockingVegetation),
		GStructureDimensions.GetDefaultDimensions(GStructureDimensionsType.Medium)
	) {
		resource_identifiers = new Pair<string, Dictionary<CardinalDirections, List<string>>> (
			"Tree1",
			new Dictionary<CardinalDirections, List<string>> () {
				{CardinalDirections.S, new List<string> (new string[] {""})}
			}
		); 
	}

}
