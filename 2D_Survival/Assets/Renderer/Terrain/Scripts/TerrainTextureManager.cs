using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainTextureGroups {
	public static List<Vector2> tropical_forest = new List<Vector2> ();
	public static List<Vector2> coniferous_forest = new List<Vector2> ();
	public static void SetGroups() {
		// tropical forest
		tropical_forest.Add (new Vector2(0, 0));
		tropical_forest.Add (new Vector2(1, 0));
		tropical_forest.Add (new Vector2(2, 0));
		tropical_forest.Add (new Vector2(0, 1));
		tropical_forest.Add (new Vector2(1, 1));
		tropical_forest.Add (new Vector2(2, 1));
	}
}

public class TerrainTextureIndex {
	public Vector2 ground_texture_index = Vector2.zero;
	public Vector2 floor_texture_index =  Vector2.zero;
}

public class TerrainTextureManager : MonoBehaviour {
	
	public int grid_cells_x;
	public int grid_cells_y;
	public Texture2D terrain_texture;
	public Material terrain_material;

	public void Init() {
		TerrainTextureGroups.SetGroups ();
		terrain_material.mainTexture = terrain_texture;
	}

	public Vector2[] getUvCoords(int cell_index_x, int cell_index_y) {
		if (cell_index_x < grid_cells_x && cell_index_y < grid_cells_y && cell_index_x >= 0 && cell_index_y >= 0) {
			float cell_offset_x = 1f/grid_cells_x;
			float cell_offset_y = 1f/grid_cells_y;
			Vector2[] coords = new Vector2[2];
			coords[0] = new Vector2(cell_offset_x*cell_index_x, cell_offset_y*cell_index_y);
			coords[1] = new Vector2(cell_offset_x*(cell_index_x+1), cell_offset_y*(cell_index_y+1));
			return coords;
		} else {
			Debug.LogError("cell indexes out of bounds");
		}
		return null;
	} 

	public Vector2[] getUvCoords(Vector2 cell_indexes) {
		if (cell_indexes.x < grid_cells_x && cell_indexes.y < grid_cells_y && cell_indexes.x >= 0 && cell_indexes.y >= 0) {
			float cell_offset_x = 1.000000f/grid_cells_x;
			float cell_offset_y = 1.000000f/grid_cells_y;
			Vector2[] coords = new Vector2[2];
			//coords[0] = new Vector2(cell_offset_x*cell_indexes.x, cell_offset_y*cell_indexes.y);
			//coords[1] = new Vector2(cell_offset_x*(cell_indexes.x+1), cell_offset_y*(cell_indexes.y+1));
			coords[0] = new Vector2(cell_indexes.x/grid_cells_x, cell_indexes.y/grid_cells_y);
			coords[1] = new Vector2((cell_indexes.x+1)/grid_cells_x, (cell_indexes.y+1)/grid_cells_y);
			return coords;
		} else {
			Debug.LogError("cell indexes out of bounds");
		}
		return null;
	} 

	public Vector2 getRandomTexture(BiomeTypes biome) {
		switch (biome) {
			case BiomeTypes.TropicalForest:
				return TerrainTextureGroups.tropical_forest[(int)(UnityEngine.Random.Range(0, TerrainTextureGroups.tropical_forest.Count))];
				break;
			case BiomeTypes.ConiferousForest:
				return TerrainTextureGroups.coniferous_forest[(int)(UnityEngine.Random.Range(0, TerrainTextureGroups.coniferous_forest.Count))];
				break;
		}
		return Vector2.zero;
	}

}
