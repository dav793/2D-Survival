﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OBJ_Bush5 : GStructure {

	public OBJ_Bush5() : base(
		GStructureProperties.GetDefaultProperties(GStructurePropertiesType.NonBlockingVegetation),
		GStructureDimensions.GetDefaultDimensions(GStructureDimensionsType.Medium)
		) {
		resource_identifiers = new Pair<string, Dictionary<CardinalDirections, List<string>>> (
			"Bush5",
			new Dictionary<CardinalDirections, List<string>> () {
				{CardinalDirections.S, new List<string> (new string[] {""})}
			}
		); 
	}

}
