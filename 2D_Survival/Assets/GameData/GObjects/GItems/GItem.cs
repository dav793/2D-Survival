using UnityEngine;
using System.Collections;

public class GItem : GObject {

	public GTile tile;
	public GItem_RefList_Index reflist_index;

	public GItem() {
		base.type = GObjectType.Item;	
		reflist_index = new GItem_RefList_Index ();
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

