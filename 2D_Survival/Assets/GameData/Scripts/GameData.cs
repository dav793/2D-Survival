using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 	Class: GameData
 	Contains all data for world simulation, and operates the data every tick to reflect time progression.
 	It is our model of the game world, in which the world and all of its constituents are represented.
 	Its data reflects the state of the world as of the current tick.
 	The renderer reads this data to generate Unity GameObjects, through which one can visualize the world.
	Nevertheless, the data contained here is not concerned with rendering; it represents a purely conceptual data world.
 */

public class GameData : MonoBehaviour {

	public static GameData GData;
	public static Vector2 FocusPoint;

	private GTile[,] worldTiles;
	private WorldSector[,] worldSectors;
	private List<WorldSector> sectorsWithinRange;
	
	public OBJ_Player active_player;
	
	void Awake() {
		if (GData == null) {
			GData = this;
			DontDestroyOnLoad(GData);
		}
		else if (GData != this) {
			Destroy(gameObject);
		}
	}

	public void Tick() {
		adjustSectorsWithinRange ();
		performActorBehaviours ();
		updateFocusPoint ();
	}

	public void Init() {

		FocusPoint = Vector2.zero;

		setupDataStructures ();
		WorldMachine.WMachine.Init ();
		WorldMachine.WMachine.generator.generateScenery ();

		// TESTS
		//OBJ_SmallCrate crate = new OBJ_SmallCrate ();
		//crate.setPosition (new Vector2(3, 1), new Vector2(0, 0));

		//OBJ_RockItem rock = new OBJ_RockItem ();
		//rock.setPosition (new Vector2(2, 1), new Vector2(0, 0));


		//crate.setPosition (new Vector2(24, 24));

		Vector2 position = Vector2.zero;

		position = new Vector2 (400, 400);
		if(!getTileFromWorldPoint(position).isBlocked()) {
			active_player = new OBJ_Player ("Player");
			active_player.setPosition (position);
		}

		for (int i = 0; i < 8; ++i) {
			position = new Vector2(400+UnityEngine.Random.Range(-400, 400), 400+UnityEngine.Random.Range(-400, 400));
				if(!getTileFromWorldPoint(position).isBlocked()) {
				GCharacter charac = new GCharacter ();
				charac.setPosition (position);
				charac.setBehaviour (new Behaviour_PaceRandomly());
			}
		}

		for (int i = 0; i < 30; ++i) {
			position = new Vector2(400+UnityEngine.Random.Range(-400, 400), 400+UnityEngine.Random.Range(-400, 400));
			if(!getTileFromWorldPoint(position).isBlocked()) {
				OBJ_Rabbit bunny = new OBJ_Rabbit ("a bunny");
				bunny.setPosition (position);
				bunny.setBehaviour (new Behaviour_PaceRandomly());
			}
		}
		// END TESTS

	}

	private void setupDataStructures() {
		// init tiles
		worldTiles = new GTile[GameController.DataSettings.world_size, GameController.DataSettings.world_size];
		for (int x = 0; x < GameController.DataSettings.world_size; ++x) {
			for (int y = 0; y < GameController.DataSettings.world_size; ++y) {
				worldTiles[x, y] = new GTile(x, y);
			}
		}
		
		// init sectors
		sectorsWithinRange = new List<WorldSector> ();
		if (GameController.DataSettings.world_size % GameController.DataSettings.sector_size != 0) {
			Debug.LogError ("Invalid sector size: Must be multiple of world size");
		} 
		else {
			worldSectors = new WorldSector[GameController.DataSettings.world_side_sectors, GameController.DataSettings.world_side_sectors];
			for (int x = 0; x < GameController.DataSettings.world_side_sectors; ++x) {
				for (int y = 0; y < GameController.DataSettings.world_side_sectors; ++y) {
					worldSectors[x, y] = new WorldSector(
						x,
						y,
						x * GameController.DataSettings.sector_size,
						x * GameController.DataSettings.sector_size + GameController.DataSettings.sector_size,
						y * GameController.DataSettings.sector_size,
						y * GameController.DataSettings.sector_size + GameController.DataSettings.sector_size
						);
				}
			}
		}
	}

	private void adjustSectorsWithinRange() {
		sectorsWithinRange = getSectorArea (getSectorIndexesWithinRange(GameController.DataSettings.within_range_radius));
	}

	private void performActorBehaviours() { // to be moved to ai section
		for (int i = 0; i < sectorsWithinRange.Count; ++i) {
			for(int j = 0; j < sectorsWithinRange[i].Contained_Objects.actors.all.count(); ++j) {
				GActor actor = sectorsWithinRange[i].Contained_Objects.actors.all.getObjectAt(j);
				if(!actor.idle) {
					actor.performBehaviour();
				}
			}
		}
	}

	public WorldSector getSector(Vector2 indexes) {
		return worldSectors[(int)indexes.x, (int)indexes.y];
	}

	public GTile getTile(Vector2 indexes) {
		if(tileExists(indexes)) {
			return worldTiles[(int)indexes.x, (int)indexes.y];
		}
		return null;
	}

	public GTile getTileFromWorldPoint(Vector2 point) {
		return getTile(worldPointToTileIndexes (point));
	}

	public bool pointBelongsToTile(Vector2 point, GTile tile) {
		if (getTile (worldPointToTileIndexes (point)) == tile) {
			return true;
		}
		return false;
	}

	public WorldSector getSectorFromWorldPoint(Vector2 point) {
		return getSector(worldPointToSectorIndexes(point));
	}

	public List<WorldSector> getSectorArea(Vector2[] boundary_sector_indexes) {
		List<WorldSector> area = new List<WorldSector> ();
		for (int x = (int)boundary_sector_indexes[0].x; x <= (int)boundary_sector_indexes[1].x; ++x) {
			for (int y = (int)boundary_sector_indexes[0].y; y <= (int)boundary_sector_indexes[1].y; ++y) {
				area.Add( worldSectors[x, y] );
			}
		}
		return area;
	}

	public List<WorldSector> getSectorAreaRender() {
		return getSectorArea (getSectorIndexesWithinRange(GameController.DataSettings.render_radius));
	}

	public Vector2[] getSectorIndexesWithinRange(float range) {
		Vector2 tile_indexes = worldPointToTileIndexes (FocusPoint);
		Vector2[] boundary_sectors = new Vector2[2];
		boundary_sectors [0] = tileIndexesToSectorIndexes( new Vector2( tile_indexes.x - range, tile_indexes.y - range ) );
		boundary_sectors [1] = tileIndexesToSectorIndexes( new Vector2( tile_indexes.x + range, tile_indexes.y + range ) );
		return boundary_sectors;
	}

	public Vector2 worldPointToSectorIndexes(Vector2 point) {
		return tileIndexesToSectorIndexes( worldPointToTileIndexes(point) );
	}

	public Vector2 tileIndexesToSectorIndexes(Vector2 tile_indexes) {
		Vector2 sector_indexes = new Vector2 (
			Mathf.Clamp ( Mathf.FloorToInt(tile_indexes.x / GameController.DataSettings.sector_size), 0, GameController.DataSettings.world_side_sectors-1 ),
			Mathf.Clamp ( Mathf.FloorToInt(tile_indexes.y / GameController.DataSettings.sector_size), 0, GameController.DataSettings.world_side_sectors-1 )
		);
		return sector_indexes;
	}

	public Vector2 worldPointToTileIndexes(Vector2 point) {
		Vector2 tile_indexes = new Vector2 (
			Mathf.Clamp ( Mathf.FloorToInt(point.x / GameController.DataSettings.tile_width), 0, GameController.DataSettings.world_size-1 ),
			Mathf.Clamp ( Mathf.FloorToInt(point.y / GameController.DataSettings.tile_width), 0, GameController.DataSettings.world_size-1 )
		);
		return tile_indexes;
	}

	public Vector2 tileIndexesToWorldPoint(Vector2 tile_indexes) {
		Vector2 world_point = new Vector2 (
			tile_indexes.x * GameController.DataSettings.tile_width,
			tile_indexes.y * GameController.DataSettings.tile_width
		);
		return world_point;
	}

	public Vector2 tileIndexesToWorldPoint(Vector2 tile_indexes, TileSubdivisions subdivision) {
		Vector2[] boundaries = getTileSubdivisionBoundaries (getTile(tile_indexes), subdivision);
		return boundaries [0];
	}

	public bool pointBelongsToTileSubdivision(Vector2 point, GTile tile, TileSubdivisions subdivision) {
		if (pointBelongsToTile (point, tile)) {
			Vector2[] boundaries = getTileSubdivisionBoundaries(tile, subdivision);
			if(point.x > boundaries[0].x && point.x < boundaries[1].x && point.y > boundaries[0].y && point.y < boundaries[1].y) {
				return true;
			}
		}
		return false;
	}

	/*
	 *  Function: getSubdivisionFromPoint
	 *  
	 *  Parameters: Vector2:<point>
	 * 
	 * 	Returns: TileSubdivisions
	 * 
	 * 	-Returns the TileSubdivision of the tile which contains <point> in which <point> is located.
	 */
	public TileSubdivisions getSubdivisionFromPoint(Vector2 point) {
		GTile tile = getTileFromWorldPoint (point);
		if (pointBelongsToTileSubdivision (point, tile, TileSubdivisions.TopLeft)) {
			return TileSubdivisions.TopLeft;
		}
		if (pointBelongsToTileSubdivision (point, tile, TileSubdivisions.TopRight)) {
			return TileSubdivisions.TopRight;
		}
		if (pointBelongsToTileSubdivision (point, tile, TileSubdivisions.BottomLeft)) {
			return TileSubdivisions.BottomLeft;
		}
		if (pointBelongsToTileSubdivision (point, tile, TileSubdivisions.BottomRight)) {
			return TileSubdivisions.BottomRight;
		}
		return TileSubdivisions.TopLeft;
	}

	/*
	 *  Function: getTileSubdivisionBoundaries
	 *  
	 *  Parameters: GTile:<tile>, TileSubdivisions:<subdivision>
	 * 
	 * 	Returns: Vector2[2]
	 * 
	 * 	-Returns a two-element array containing Vector2s to represent the position boundary points of <tile>s <subdivision>
	 *  
	 *  Tiles are subdivided as follows:
	 * 			+---+---+
	 * 			|[0]|[1]|
	 * 			+---+---+
	 * 			|[2]|[3]|
	 * 			+---+---+
	 * 	where: 
	 * 		subdivision [0] = TileSubdivisions.TopLeft
	 * 		subdivision [1] = TileSubdivisions.TopRight
	 * 		subdivision [2] = TileSubdivisions.BottomLeft
	 * 		subdivision [3] = TileSubdivisions.BottomRight
	 * 
	 * 	Then the returned array contains the position of the 2 points that delimitate <tile>s <subdivision>, as follows:
	 * 		<returned>[0] = Top-Left point
	 * 		<returned>[1] = Bottom-Right point
	 * 
	 */
	public Vector2[] getTileSubdivisionBoundaries(GTile tile, TileSubdivisions subdivision) {
	
		Vector2 tile_origin = tile.getOrigin();
		Vector2[] boundaries = new Vector2[2] { Vector2.zero, Vector2.zero };

		switch (subdivision) {
		case TileSubdivisions.TopLeft:
			boundaries[0] = new Vector2(tile_origin.x + 0, tile_origin.y + GameController.DataSettings.tile_width / 2);
			boundaries[1] = new Vector2(tile_origin.x + GameController.DataSettings.tile_width / 2, tile_origin.y + GameController.DataSettings.tile_width);
			break;
		case TileSubdivisions.TopRight:
			boundaries[0] = new Vector2(tile_origin.x + GameController.DataSettings.tile_width / 2, tile_origin.y + GameController.DataSettings.tile_width / 2);
			boundaries[1] = new Vector2(tile_origin.x + GameController.DataSettings.tile_width, tile_origin.y + GameController.DataSettings.tile_width);
			break;
		case TileSubdivisions.BottomLeft:
			boundaries[0] = new Vector2(tile_origin.x + 0, tile_origin.y + 0); 
			boundaries[1] = new Vector2(tile_origin.x + GameController.DataSettings.tile_width / 2, tile_origin.y + GameController.DataSettings.tile_width / 2); 
			break;
		case TileSubdivisions.BottomRight:
			boundaries[0] = new Vector2(tile_origin.x + GameController.DataSettings.tile_width / 2, tile_origin.y + 0);
			boundaries[1] = new Vector2(tile_origin.x + GameController.DataSettings.tile_width, tile_origin.y + GameController.DataSettings.tile_width / 2);
			break;
		}

		return boundaries;

	}

	/*
	 *  Function: tileExists
	 *  
	 *  Parameters: Vector2:<tile_index>
	 * 
	 * 	Returns: bool
	 * 
	 * 	-Returns false if tile indexes given by <tile_index> are out of bounds of <worldTiles> array.
	 *  -Returns true if they are within bounds.
	 */
	public bool tileExists(Vector2 tile_index) {
		if(tile_index.x >= 0 && tile_index.x < GameController.DataSettings.world_size && tile_index.x >= 0 && tile_index.x < GameController.DataSettings.world_size) {
			// tile exists
			return true;
		}
		return false;
	} 
		
	/*
	 *  Function: requestObjectPositionChange
	 *  
	 *  Parameters: GObject:<obj>, Vector2:<new_position>
	 * 
	 * 	Returns: bool
	 * 
	 * 	-Prompts GameData to change the position of <obj> to <new_position>.
	 *  -Returns true if position is changed successfully.
	 * 	-Returns false if position could not be changed.
	 *  -Please note: GObjects should only ever be moved using this procedure, instead of directly modifying the objects position properties. 
	 * 	 As long as this rule is followed, objects should never be moved into unwanted positions.
	 */
	public bool requestObjectPositionChange(GObject obj, Vector2 new_position) {

		Vector2 newX = new Vector2 (new_position.x, obj.pos_y);
		Vector2 newY = new Vector2 (obj.pos_x, new_position.y);
		Vector2 result = new_position;

		if (positionIsOutOfWorldBounds (result)) {
			if (!positionIsOutOfWorldBounds (newX)) {
				result.y = obj.pos_y;
			} else if (!positionIsOutOfWorldBounds (newY)) {
				result.x = obj.pos_x;
			} else {
				return false;	//new position is out of bounds
			}
		}

		if (getTileFromWorldPoint (result).isBlocked ()) {
			if (!getTileFromWorldPoint (newX).isBlocked ()) {
				result.y = obj.pos_y;
			} else if (!getTileFromWorldPoint (newY).isBlocked ()) {
				result.x = obj.pos_x;
			} else {
				//new position is blocked, move away from the center of blocked tile <obj> is running into

				return false;
			}
		}

		obj.pos_x = result.x;
		obj.pos_y = result.y;
		return true;

	}

	/*
	 *  Function: moveObjToSector
	 *  
	 *  Parameters: GActor:<obj>, WorldSector:<sector>
	 * 
	 * 	Returns: void
	 * 
	 * 	-Places the reference for <obj> into <sector>. 
	 *  -If <obj>s reference is already contained by another sector, it is first removed from the old sector.
	 *  -The reference to <obj> is stored in <sector>s <Contained_Objects> property.
	 *  -Since only GActors are stored in sectors, <obj> must be of type GActor. All other GObjects are stored in tiles.
	 */
	public void moveObjToSector(GActor obj, WorldSector sector) {
		if (obj.sector != null) {
			// object belongs to a sector
			removeObjFromSector(obj);
		}
		addObjToSector (obj, sector);
	}

	void addObjToSector(GActor obj, WorldSector sector) {
		if (obj.sector == null) {
			// object does not belong to a sector
			obj.sector = sector;
			sector.Contained_Objects.addObject(obj);
		}
	}
	
	void removeObjFromSector(GActor obj) {
		if (obj.sector != null) {
			// object belongs to a sector
			obj.sector.Contained_Objects.removeObject(obj);
			obj.sector = null;
		}
		
	}

	void updateFocusPoint() {
		if (active_player != null && active_player.renderedGameObject != null) {
			/*FocusPoint = new Vector2 (
				active_player.renderedGameObject.transform.position.x, 
				active_player.renderedGameObject.transform.position.y
			);*/
			FocusPoint = new Vector2(active_player.pos_x, active_player.pos_y);
			GameCameraController.GCamControl.updateCamPosition();
		}
	}

	bool positionIsOutOfWorldBounds(Vector2 position) {
		if(
			position.x < 0 || 
		   	position.y < 0 || 
		   	position.x > GameController.DataSettings.tile_width * GameController.DataSettings.world_size || 
		   	position.y > GameController.DataSettings.tile_width * GameController.DataSettings.world_size
		) {
			return true;
		}

		return false;
	}

	/******** FOR TESTING ********/
	public string sectorsWithinRangeToString() {
		string str = "";
		for (int i = 0; i < sectorsWithinRange.Count; ++i) {
			str = str + "(" + sectorsWithinRange[i].indexToString() + ") ";
		}
		return str;
	}

}
