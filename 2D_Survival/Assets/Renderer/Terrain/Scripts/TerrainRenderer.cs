using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	GameRenderer class:
 *		Subcomponent of GameRenderer, which renders the terrain elements associated to a WorldSector. 
 */

public class TerrainRenderer : MonoBehaviour {

	public TerrainTextureManager texMan;
	public GameObject terrainObject;
	public GameObject terrainObjectHolder;

	private GameObjectPool terrainObjectPool;

	public void Init() {
		initObjectPool ();
		texMan.Init ();
	}

	public void setupSector(WorldSector sector) {
		float sector_width = GameData.GData.data_settings.sector_size * GameData.GData.data_settings.tile_width;
		GameObject obj = getNewTerrainObject();
		setupObject(obj, sector);
		obj.transform.position = new Vector3 (
			sector.index_x * sector_width + sector_width,
			sector.index_y * sector_width,
			0
		);
		sector.linkGameObject (obj);
	}

	public void discardSector(WorldSector sector) {
		discardTerrainObject (sector.terrainGameObject);
		sector.unlinkGameObject ();
	}

	private GameObject getNewTerrainObject() {
		GameObject obj = terrainObjectPool.pop ();
		return obj;
	}

	private void discardTerrainObject(GameObject obj) {
		terrainObjectPool.push (obj);
	}

	private void setupObject(GameObject obj, WorldSector sector) {
		obj.GetComponent<MeshRenderer> ().material = texMan.terrain_material;
		//obj.GetComponent<MeshRenderer> ().material.mainTexture = texMan.terrain_sprite.texture;
		obj.GetComponent<MeshFilter> ().mesh = generateMesh (GameData.GData.data_settings.sector_size * GameData.GData.data_settings.sector_size, GameData.GData.data_settings.tile_width, sector);
	}

	private Mesh generateMesh(int tile_count, float tile_width, WorldSector sector) {
	
		Mesh mesh = new Mesh ();
		mesh.Clear ();
		mesh.name = "TerrainMesh";
		
		Vector3[] vertices = new Vector3[4 * tile_count];
		int[] triangles = new int[6 * tile_count];
		Vector2[] uvs = new Vector2[4 * tile_count];
		int side_length = (int)Mathf.Sqrt(tile_count);

		for(int i = 0; i < tile_count; ++i) {

			int index_y = i / side_length;
			int index_x = i % side_length;			
			float origin_x = index_x * tile_width;
			float origin_y = index_y * tile_width;

			setTileVertices(i, 
                new Vector3( origin_x, 				origin_y, 				0), 
                new Vector3( origin_x + tile_width, origin_y, 				0), 
                new Vector3( origin_x + tile_width, origin_y + tile_width, 	0), 
                new Vector3( origin_x, 				origin_y + tile_width, 	0),
                vertices
			);

			setTileTriangles(i, triangles);

			setTileUvs(i, uvs, sector, index_x, index_y);

		}

		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.Optimize ();

		return mesh;

	}

	private void setTileVertices(int tile_index, Vector3 vert0, Vector3 vert1, Vector3 vert2, Vector3 vert3, Vector3[] vertices) {
		vertices [4*tile_index + (int)TileVertices.topLeft] 	  	= vert0;
		vertices [4*tile_index + (int)TileVertices.topRight] 	  	= vert1;
		vertices [4*tile_index + (int)TileVertices.bottomRight] 	= vert2;
		vertices [4*tile_index + (int)TileVertices.bottomLeft]  	= vert3;
	}

	private void setTileTriangles(int tile_index, int[] triangles) {
		// triangle 0
		triangles [6*tile_index + 0] = 4*tile_index + (int)TileVertices.topLeft;
		triangles [6*tile_index + 1] = 4*tile_index + (int)TileVertices.bottomRight;
		triangles [6*tile_index + 2] = 4*tile_index + (int)TileVertices.bottomLeft;
		
		// triangle 1
		triangles [6*tile_index + 3] = 4*tile_index + (int)TileVertices.topLeft;
		triangles [6*tile_index + 4] = 4*tile_index + (int)TileVertices.topRight;
		triangles [6*tile_index + 5] = 4*tile_index + (int)TileVertices.bottomRight;
	}

	private void setTileUvs(int tile_index, Vector2[] uvs, WorldSector sector, int tile_index_x, int tile_index_y) {
		Vector2 abs_indexes = new Vector2 (sector.lower_boundary_x + tile_index_x, sector.lower_boundary_y + tile_index_y);
		GTile tile = GameData.GData.getTile (abs_indexes);
		Vector2[] uvCoords = texMan.getUvCoords (texMan.getRandomTexture(tile.biome.type));
		//Vector2[] uvCoords = texMan.getUvCoords(UnityEngine.Random.Range(0, texMan.grid_cells_x), UnityEngine.Random.Range (0, texMan.grid_cells_y));
		uvs [4 * tile_index + (int)TileVertices.topLeft] 		= new Vector2 (uvCoords[0].x, uvCoords[0].y);
		uvs [4 * tile_index + (int)TileVertices.topRight] 		= new Vector2 (uvCoords[0].x, uvCoords[1].y);
		uvs [4 * tile_index + (int)TileVertices.bottomRight] 	= new Vector2 (uvCoords[1].x, uvCoords[1].y);
		uvs [4 * tile_index + (int)TileVertices.bottomLeft] 	= new Vector2 (uvCoords[1].x, uvCoords[0].y);
	}

	private void initObjectPool() {
		// Destroy any previous objects
		List<GameObject> children = new List<GameObject> ();
		foreach(Transform child in terrainObjectHolder.transform) {
			children.Add(child.gameObject);
		}
		children.ForEach (child => Destroy(child));
		
		// Initialize terrain mesh object pool
		terrainObjectPool = new GameObjectPool (terrainObject, 256, terrainObjectHolder);
	}

}
