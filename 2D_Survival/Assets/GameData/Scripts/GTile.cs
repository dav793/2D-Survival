using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GTile {
	public int index_x;
	public int index_y;
	public Biome biome;
	public GObject_Tile_RefList Contained_Objects; 

	// variables for renderer use
	public bool is_rendered = false;
	public TerrainTextureIndex textures;

	public GTile(int index_x, int index_y) {
		this.index_x = index_x;
		this.index_y = index_y;
		biome = BiomeData.tropical_forest;
		Contained_Objects = new GObject_Tile_RefList (new Vector2(index_x, index_y));
	}

	public Vector2 getCenter() {
		return new Vector2 (GameController.DataSettings.tile_width * index_x + GameController.DataSettings.tile_width/2, 
		                    GameController.DataSettings.tile_width * index_y + GameController.DataSettings.tile_width/2);
	}

	public Vector2 getCenter(Vector2 offset) {
		if (Mathf.Abs (offset.x) <= GameController.DataSettings.tile_width / 2 && Mathf.Abs (offset.y) <= GameController.DataSettings.tile_width / 2) {
			Vector2 center = getCenter ();
			return new Vector2 (center.x + offset.x, center.y + offset.y);
		}
		return getCenter ();
	}

	public Vector2 getCenter(TileSubdivisions subdivision) {
		Vector2[] boundaries = GameData.GData.getTileSubdivisionBoundaries (this, subdivision);
		return new Vector2 (boundaries[0].x + Mathf.Abs(boundaries[1].x - boundaries[0].x)/2, boundaries[0].y + Mathf.Abs(boundaries[1].y - boundaries[0].y)/2);
	}

	public Vector2 getCenter(TileSubdivisions subdivision, Vector2 offset) {
		if (Mathf.Abs (offset.x) <= GameController.DataSettings.tile_width / 4 && Mathf.Abs (offset.y) <= GameController.DataSettings.tile_width / 4) {
			Vector2 center = getCenter (subdivision);
			return new Vector2 (center.x + offset.x, center.y + offset.y);
		}
		return getCenter (subdivision);
	}
	
	public Vector2 getIndexes() {
		return new Vector2 (index_x, index_y);
	}

	public Vector2 getOrigin() {
		return GameData.GData.tileIndexesToWorldPoint (getIndexes());
	}

	public bool isBlocked() {
		List<GStructure> contained_structures = Contained_Objects.structures.all.getDataList () as List<GStructure>;
		for (int i = 0; i < contained_structures.Count; ++i) {
			if(contained_structures[i].properties.is_blocking) {
				return true;
			}
		}
		return false;
	}

	public bool isBlocked(TileSubdivisions subdivision) {
		List<GStructure> contained_structures = Contained_Objects.structures.all.getDataList () as List<GStructure>;
		for (int i = 0; i < contained_structures.Count; ++i) {
			if(contained_structures[i].properties.is_blocking) {
				// check if blocking item is inside subdivision
				if(GameData.GData.getSubdivisionFromPoint(contained_structures[i].getPosition()) == subdivision) {
					return true;
				}
			}
		}
		return false;
	}

	public bool containsStructures() {
		List<GStructure> contained_structures = Contained_Objects.structures.all.getDataList () as List<GStructure>;
		if(contained_structures.Count > 0) {
			return true;
		}
		return false;
	}

	public bool containsStructures(TileSubdivisions subdivision) {
		List<GStructure> contained_structures = Contained_Objects.structures.all.getDataList () as List<GStructure>;
		for (int i = 0; i < contained_structures.Count; ++i) {
			if(GameData.GData.pointBelongsToTileSubdivision(contained_structures[i].getPosition(), this, subdivision)) {
				return true;
			}
			if((int)contained_structures[i].dimensions.size >= (int) StructureSizes.Medium) {
				return true;
			}
		}
		return false;	
	}

	public static TileSubdivisions GetRandomSubdivision() {
		return (TileSubdivisions) UnityEngine.Random.Range(0, System.Enum.GetNames (typeof(TileSubdivisions)).Length);
	}

	public string indexToString() {
		return index_x + ", " + index_y;
	}

}
