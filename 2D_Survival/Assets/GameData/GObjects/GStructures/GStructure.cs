using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GStructure : GObject {

	public GTile tile;
	public GStructureProperties properties;
	public GStructureDimensions dimensions;

	public Pair<string, Dictionary<CardinalDirections, List<string>>> resource_identifiers;
	public GStructure_RefList_Index reflist_index;

	public GStructure(GStructureProperties properties) {	
		this.properties = properties;
		dimensions = GStructureDimensions.GetDefaultDimensions (GStructureDimensionsType.Medium);
		type = GObjectType.Structure;
		reflist_index = new GStructure_RefList_Index ();
	}

	public GStructure(GStructureProperties properties, GStructureDimensions dimensions) {
		this.properties = properties;
		this.dimensions = dimensions;
		type = GObjectType.Structure;
		reflist_index = new GStructure_RefList_Index ();
	}

	/*
	 *	Sets the position of this GStructure to <point> 
	 */
	public override void setPosition(Vector2 point) {
		removeFromTile ();
		if (placeAtPoint (point)) {
			addToTile (GameData.GData.getTileFromWorldPoint (getPosition()));
			if(dimensions.size == StructureSizes.Small) {
				reflist_index.subdivision = GameData.GData.getSubdivisionFromPoint(getPosition());
			}
		} 
		else {
			Debug.LogError("Failed to place GStructure at "+point);
		}
	}

	/*
	 *	Sets the position of this GStructure to the center of tile with indexes <tile_index>, offset by <offset> 
	 */
	public override void setPosition(Vector2 tile_index, Vector2 offset) {
		removeFromTile ();
		if (placeAtPoint (GameData.GData.getTile (tile_index).getCenter (offset))) {
			addToTile (GameData.GData.getTile (tile_index));
			if(dimensions.size == StructureSizes.Small) {
				reflist_index.subdivision = GameData.GData.getSubdivisionFromPoint(getPosition());
			}
		} 
		else {
			Debug.LogError("Failed to place GStructure at "+GameData.GData.getTile (tile_index).getCenter (offset));
		}
	}

	/*
	 *	Sets the position of this GStructure to tile with indexes <tile_index>, at the specified <subdivision>
	 *  The GStructure must be of size small.
	 */
	public void setPosition(Vector2 tile_index, TileSubdivisions subdivision) {
		if (dimensions.size == StructureSizes.Small) {
			removeFromTile ();
			if (placeAtPoint (GameData.GData.getTile (tile_index).getCenter (subdivision))) {
				addToTile (GameData.GData.getTile (tile_index));
				reflist_index.subdivision = subdivision;
			} else {
				Debug.LogError ("Failed to place GStructure at " + GameData.GData.getTile (tile_index).getCenter ());
			}
		} else {
			Debug.LogError ("GStructure must be of size small.");
		}
	}

	/*
	 *	Sets the position of this GStructure to tile with indexes <tile_index>, at the specified <subdivision>, offset by <offset>
	 *  The GStructure must be of size small.
	 */
	public void setPosition(Vector2 tile_index, TileSubdivisions subdivision, Vector2 offset) {
		if (dimensions.size == StructureSizes.Small) {
			removeFromTile ();
			if (placeAtPoint (GameData.GData.getTile (tile_index).getCenter (subdivision, offset))) {
				addToTile (GameData.GData.getTile (tile_index));
				reflist_index.subdivision = subdivision;
			} else {
				Debug.LogError ("Failed to place GStructure at " + GameData.GData.getTile (tile_index).getCenter (offset));
			}
		} else {
			Debug.LogError ("GStructure must be of size small.");
		}
	}

	/*
	 *	Sets the position of this GStructure to the center of <tile>
	 */
	public override void setPosition(GTile tile) {
		removeFromTile ();
		if (placeAtPoint (tile.getCenter ())) {
			addToTile (tile);
			if(dimensions.size == StructureSizes.Small) {
				reflist_index.subdivision = GameData.GData.getSubdivisionFromPoint(getPosition());
			}
		} 
		else {
			Debug.LogError("Failed to place GStructure at "+tile.getCenter ());
		}
	}

	/*
	 *	Sets the position of this GStructure to the center of <tile>, offset by <offset>
	 */
	public override void setPosition(GTile tile, Vector2 offset) {
		removeFromTile ();
		if (placeAtPoint (tile.getCenter (offset))) {
			addToTile (tile);
			if(dimensions.size == StructureSizes.Small) {
				reflist_index.subdivision = GameData.GData.getSubdivisionFromPoint(getPosition());
			}
		} 
		else {
			Debug.LogError("Failed to place GStructure at "+tile.getCenter (offset));
		}
	}

	/*
	 *	Sets the position of this GStructure to the center of <tile>, at the specified <subdivision>
	 */
	public void setPosition(GTile tile, TileSubdivisions subdivision) {
		if (dimensions.size == StructureSizes.Small) {
			removeFromTile ();
			if (placeAtPoint (tile.getCenter (subdivision))) {
				addToTile (tile);
				reflist_index.subdivision = subdivision;
			} else {
				Debug.LogError ("Failed to place GStructure at " + tile.getCenter ());
			}
		} else {
			Debug.LogError ("GStructure must be of size small.");
		}
	}

	/*
	 *	Adds GObject to <tile> contained objects.
	 */
	public void addToTile(GTile tile) {
		if (this.tile == null) {
			// object does not belong to a tile
			this.tile = tile;
			tile.Contained_Objects.addObject(this);
		} 
	}

	/*
	 *	Removes GObject from its current tiles contained objects.
	 */
	public void removeFromTile() {
		if (tile != null) {
			// object belongs to a tile
			tile.Contained_Objects.removeObject(this);
			tile = null;
		}

	}

	/*
	 *	Removes GObject from its current tiles contained objects, and adds GObject to <tile> contained objects.
	 */
	public void transferToTile(GTile tile) {
		if (this.tile != null) {
			// object belongs to a tile
			removeFromTile();
		}
		addToTile (tile);
	}

}
