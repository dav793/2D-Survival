using UnityEngine;
using System.Collections;

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

}
