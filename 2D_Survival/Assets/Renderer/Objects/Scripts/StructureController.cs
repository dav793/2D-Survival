using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StructureController : GeneralGObjectController {

	public string getSuffix(CardinalDirections orientation) {
		string clipSuffix = "";
		string separator = "";
		List<string> post_suffixes;
		
		if(getObj().resource_identifiers.Second.Count > 1) {
			clipSuffix = GetPreSuffixFromOrientation (orientation);
		}
		
		getObj().resource_identifiers.Second.TryGetValue (orientation, out post_suffixes);
		
		if(post_suffixes.Count > 1 && getObj().resource_identifiers.Second.Count > 1) {
			separator = "-";
		}
		
		clipSuffix = clipSuffix + separator + post_suffixes [UnityEngine.Random.Range (0, post_suffixes.Count)];
		
		return clipSuffix;
	}

	public override void updateSprite() {
		if (getObj().resource_identifiers != null) {
			spr_renderer.sprite = GetSpriteFromResources(getObj().resource_identifiers.First, getSuffix(getObj().properties.orientation));
		}
	}

	GStructure getObj() {
		return (GStructure)obj;
	}

}
