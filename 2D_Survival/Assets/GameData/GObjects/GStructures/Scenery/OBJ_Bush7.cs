﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OBJ_Bush7 : GStructure {

	public OBJ_Bush7() : base(
		GStructureProperties.GetDefaultProperties(GStructurePropertiesType.NonBlockingVegetation),
		GStructureDimensions.GetDefaultDimensions(GStructureDimensionsType.Small)
		) {
		resource_identifiers = new Pair<string, Dictionary<CardinalDirections, List<string>>> (
			"Bush7",
			new Dictionary<CardinalDirections, List<string>> () {
				{CardinalDirections.S, new List<string> (new string[] {"A", "B"})}
			}
		); 
	}

}
