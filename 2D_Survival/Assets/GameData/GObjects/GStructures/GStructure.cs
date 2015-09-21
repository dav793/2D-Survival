using UnityEngine;
using System.Collections;

public class GStructure : GObject {

	public GTile tile;
	public BOOL_YN interactive;
	public BOOL_YN movable;
	public BOOL_YN environmental;

	public GStructure_RefList_Index reflist_index;

	public GStructure(BOOL_YN interactive, BOOL_YN movable, BOOL_YN environmental) {
		base.type = GObjectType.Structure;	
		this.interactive = interactive;
		this.movable = movable;
		this.environmental = environmental;
		reflist_index = new GStructure_RefList_Index ();
		type = GObjectType.Structure;
	}

	public override void setPosition(Vector2 point) {
		removeFromTile ();
		if (placeAtPoint (point)) {
			addToTile (GameData.GData.getTileFromWorldPoint (new Vector2 (pos_x, pos_y)));
		} 
		else {
			Debug.LogError("Failed to place GStructure at "+point);
		}

	}

	public override void setPosition(Vector2 tile_index, Vector2 offset) {
		removeFromTile ();
		if (placeAtPoint (GameData.GData.getTile (tile_index).getCenter (offset))) {
			addToTile (GameData.GData.getTile (tile_index));
		} 
		else {
			Debug.LogError("Failed to place GStructure at "+GameData.GData.getTile (tile_index).getCenter (offset));
		}
	}

	public override void setPosition(GTile tile, Vector2 offset) {
		removeFromTile ();
		if (placeAtPoint (tile.getCenter (offset))) {
			addToTile (tile);
		} 
		else {
			Debug.LogError("Failed to place GStructure at "+tile.getCenter (offset));
		}
	}

	public void addToTile(GTile tile) {
		if (this.tile == null) {
			// object does not belong to a tile
			this.tile = tile;
			tile.Contained_Objects.addObject(this);
		} 
	}

	public void removeFromTile() {
		if (tile != null) {
			// object belongs to a tile
			tile.Contained_Objects.removeObject(this);
			tile = null;
		}

	}

	public void transferToTile(GTile tile) {
		if (this.tile != null) {
			// object belongs to a tile
			removeFromTile();
		}
		addToTile (tile);
	}

}
