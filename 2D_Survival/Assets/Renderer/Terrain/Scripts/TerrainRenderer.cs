using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	TerrainRenderer class:
 *		Subcomponent of GameRenderer, which renders the terrain elements (tiles) that comprise any given WorldSector. 
 */

public class TerrainRenderer : MonoBehaviour {

	public TerrainTextureManager texMan;
	public GameObject terrainObject;
	public GameObject terrainObjectHolder;

	GameObjectPool terrainObjectPool;
	string unassigned_go_name = "Unassigned Tile";

	public void Init() {
		initObjectPool ();
		texMan.Init ();
	}

	/*
	 *  Function: setupSector
	 *  
	 *  Parameters: WorldSector:<sector>
	 * 
	 * 	Returns: void
	 * 
	 * 	-Obtains a new GameObject from the <terrainObjectPool>, which will be used to render <sector>s terrain.
	 *  -Initializes the GameObjects attributes and links references between <sector> and the GameObject
	 *  -Sets the GameObjects position. 
	 */
	public void setupSector(WorldSector sector) {
		float sector_width = GameController.DataSettings.sector_size * GameController.DataSettings.tile_width;
		GameObject obj = getNewTerrainObject();
		obj.name = sector.gameObjectName;
		setupObject(obj, sector);
		obj.transform.position = new Vector3 (
			sector.index_x * sector_width + sector_width,
			sector.index_y * sector_width,
			GameRenderer.GRenderer.getZUnitsTerrain()
		);
		sector.linkGameObject (obj);

		//TESTS
		TestUtils.active_r_trns++;
	}

	/*
	 *  Function: discardSector
	 *  
	 *  Parameters: WorldSector:<sector>
	 * 
	 * 	Returns: void
	 * 
	 * 	-Returns <sector>s rendered GameObject to the <terrainObjectPool>.
	 *  -Unlinks references between <sector> and the GameObject
	 */
	public void discardSector(WorldSector sector) {
		terrainObjectPool.push (sector.terrainGameObject);
		sector.unlinkGameObject ();

		//TESTS
		TestUtils.active_r_trns--;
	}

	/*
	 *  Function: getNewTerrainObject
	 *  
	 *  Parameters: none
	 * 
	 * 	Returns: <GameObject>
	 * 
	 * 	Obtains a new GameObject from the <terrainObjectPool>
	 */
	private GameObject getNewTerrainObject() {
		GameObject obj = terrainObjectPool.pop ();
		return obj;
	}

	/*
	 *  Function: setupObject
	 *  
	 *  Parameters: GameObject:<obj>, WorldSector:<sector>
	 * 
	 * 	Returns: void
	 * 
	 * 	Assigns a material and mesh to <obj> in relation to the data contained in <sector>
	 */
	private void setupObject(GameObject obj, WorldSector sector) {
		obj.GetComponent<MeshRenderer> ().material = texMan.terrain_material;
		//obj.GetComponent<MeshRenderer> ().material.mainTexture = texMan.terrain_sprite.texture;
		obj.GetComponent<MeshFilter> ().mesh = generateMesh (GameController.DataSettings.sector_size * GameController.DataSettings.sector_size, GameController.DataSettings.tile_width, sector);
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

	/*
	 *  Function: initObjectPool
	 *  
	 *  Parameters: none
	 * 
	 * 	Returns: void
	 * 
	 * 	Initializes the <terrainObjectPool> with new instances of <terrainObject>, which is the GameObject prefab used to visualize terrain objects
	 *  in the worldspace.
	 */
	private void initObjectPool() {
		// Destroy any previous objects
		List<GameObject> children = new List<GameObject> ();
		foreach(Transform child in terrainObjectHolder.transform) {
			children.Add(child.gameObject);
		}
		children.ForEach (child => Destroy(child));
		
		// Initialize terrain mesh object pool
		terrainObjectPool = new GameObjectPool (terrainObject, unassigned_go_name, GameController.RendererSettings.terrain_pool_size, terrainObjectHolder);
	}

}
