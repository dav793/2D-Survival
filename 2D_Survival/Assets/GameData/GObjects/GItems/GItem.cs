using UnityEngine;
using System.Collections;

public class GItem : GObject {

	public GTile tile;
	public GItem_RefList_Index reflist_index;
	
	public float weight;

	public GItem() {
		base.type = GObjectType.Item;	
		reflist_index = new GItem_RefList_Index ();
	}

	public override void setPosition(Vector2 point) {
		removeFromTile ();
		if (placeAtPoint (point)) {
			addToTile (GameData.GData.getTileFromWorldPoint (new Vector2 (pos_x, pos_y)));
		} 
		else {
			Debug.LogError("Failed to place GItem at "+point);
		}
	}
	
	public override void setPosition(Vector2 tile_index, Vector2 offset) {
		removeFromTile ();
		if (placeAtPoint (GameData.GData.getTile (tile_index).getCenter (offset))) {
			addToTile (GameData.GData.getTile (tile_index));
		}
		else {
			Debug.LogError("Failed to place GItem at tile "+tile_index.x+", "+tile_index.y);
		}
	}

	public override void setPosition(GTile tile, Vector2 offset) {
		removeFromTile ();
		if (placeAtPoint (tile.getCenter (offset))) {
			addToTile (tile);
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

	public virtual bool isEquippable() {
		return false;
	}

	public virtual string getDebug() {
		return "";
	}

}



