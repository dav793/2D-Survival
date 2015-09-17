using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 	Class: GameData
 	Contains all data for world simulation, and operates the data every tick to reflect time progression.
 	It is our model of the game world, in which the world and all of its constituents are represented.
 	Its data reflects the state of the world as of the current tick. 
 	It is not concerned with rendering, it only works on a conceptual world simulation.
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

		//GTile tl = getTileFromWorldPoint (new Vector2 (crate.pos_x, crate.pos_y));
		//Debug.Log (tl.indexToString());
		//Debug.Log (tl.Contained_Objects.objectIsContained(crate));
		//Debug.Log (getTileFromWorldPoint(new Vector2(crate.pos_x, crate.pos_y)).indexToString());
		//Debug.Log (getTileFromWorldPoint(new Vector2(crate.pos_x, crate.pos_y)).Contained_Objects.objectIsContained(crate));
		//Debug.Log (tl.Contained_Objects.objectIsContained(crate));


		//rock.setPosition (new Vector2(24, 24));


		active_player = new OBJ_Player ("Player");
		active_player.setPosition (new Vector2(400,400));
		//active_player.setPosition (getTile(new Vector2(0,0)).getCenter());

		for (int i = 0; i < 3; ++i) {
			GCharacter charac = new GCharacter ();
			charac.setPosition (new Vector2(400+UnityEngine.Random.Range(-100, 100), 400+UnityEngine.Random.Range(-100, 100)));
			charac.setBehaviour (new Behaviour_PaceRandomly());
		}

		for (int i = 0; i < 100; ++i) {
			OBJ_Rabbit bunny = new OBJ_Rabbit ("Liro boni");
			bunny.setPosition (new Vector2(400+UnityEngine.Random.Range(-100, 100), 400+UnityEngine.Random.Range(-100, 100)));
			bunny.setBehaviour (new Behaviour_PaceRandomly());
		}

		/*OBJ_Rabbit bunny1 = new OBJ_Rabbit ("Bunny");
		bunny1.setPosition (new Vector2(200+UnityEngine.Random.Range(-100, 100), 200+UnityEngine.Random.Range(-100, 100)));
		bunny1.setBehaviour (new Behaviour_PaceRandomly());
		OBJ_Rabbit bunny2 = new OBJ_Rabbit ("Psychotic bunny");
		bunny2.setPosition (new Vector2(200+UnityEngine.Random.Range(-100, 100), 200+UnityEngine.Random.Range(-100, 100)));
		bunny2.setBehaviour (new Behaviour_PaceRandomly());*/
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
		return worldTiles[(int)indexes.x, (int)indexes.y];
	}

	public GTile getTileFromWorldPoint(Vector2 point) {
		return getTile(worldPointToTileIndexes (point));
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
	 *  -Please note: GObjects should only be moved by using this procedure, instead of directly changing the objects position properties. 
	 * 	 Otherwise, objects could be moved into unwanted positions.
	 */
	public bool requestObjectPositionChange(GObject obj, Vector2 new_position) {

		if (positionIsOutOfWorldBounds (new_position)) {
			return false;
		}

		obj.pos_x = new_position.x;
		obj.pos_y = new_position.y;

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
		if (active_player != null) {
			FocusPoint = new Vector2 (active_player.pos_x, active_player.pos_y);
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
