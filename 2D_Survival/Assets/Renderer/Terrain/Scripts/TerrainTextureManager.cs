using UnityEngine;
using System.Collections;

public class TerrainTextureManager : MonoBehaviour {

	public Material terrain_material;
	public int grid_cells_x;
	public int grid_cells_y;

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

}
