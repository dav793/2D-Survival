using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OBJ_TropicalBush1 : GStructure {
	
	public OBJ_TropicalBush1() : base(
		GStructureProperties.GetDefaultProperties(GStructurePropertiesType.NonBlockingVegetation),
		GStructureDimensions.GetDefaultDimensions(GStructureDimensionsType.Small)
	) {
		resource_identifiers = new Pair<string, Dictionary<CardinalDirections, List<string>>> (
			"Bush1",
			new Dictionary<CardinalDirections, List<string>> () {
				{CardinalDirections.S, new List<string> (new string[] {""})}
			}
		); 
	}

}
