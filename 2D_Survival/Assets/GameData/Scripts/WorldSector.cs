using UnityEngine;
using System.Collections;

public class WorldSector {

	public int index_x;
	public int index_y;
	public int lower_boundary_x;
	public int upper_boundary_x;
	public int lower_boundary_y;
	public int upper_boundary_y;
	public OperationMode mode;
	public GObject_Sector_RefList Contained_Objects; 
	
	// variables for renderer use
	public bool is_rendered = false;
	public GameObject terrainGameObject;
	
	public WorldSector(int index_x, int index_y, int lower_boundary_x, int upper_boundary_x, int lower_boundary_y, int upper_boundary_y) {
		this.index_x = index_x;
		this.index_y = index_y;
		this.lower_boundary_x = lower_boundary_x;
		this.upper_boundary_x = upper_boundary_x;
		this.lower_boundary_y = lower_boundary_y;
		this.upper_boundary_y = upper_boundary_y;
		mode = OperationMode.OUT_OF_CHARACTER_RANGE;
		Contained_Objects = new GObject_Sector_RefList (new Vector2(index_x, index_y));
	}

	public bool tileBelongsToSector(GTile tile) {
		Vector2 sectorIndexes = GameData.GData.tileIndexesToSectorIndexes (new Vector2 (tile.index_x, tile.index_y));
		if (sectorIndexes.x == index_x && sectorIndexes.y == index_y) {
			return true;
		}
		return false;
	}

	// procedures for renderer use
	public void linkGameObject(GameObject gobject) {
		terrainGameObject = gobject;
		is_rendered = true;
	}

	public void unlinkGameObject() {
		terrainGameObject = null;
		is_rendered = false;
	}

}
