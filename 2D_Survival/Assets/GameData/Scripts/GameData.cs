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

	public GameData_Settings data_settings;
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
		updateCamFocusPoint ();
	}

	public void Init(GameData_Settings data_settings) {
		this.data_settings = data_settings;

		FocusPoint = Vector2.zero;

		setupDataStructures ();
		WorldMachine.WMachine.Init ();
		WorldMachine.WMachine.generator.generateScenery ();

		// TESTS
		OBJ_SmallCrate crate = new OBJ_SmallCrate ();
		crate.setPosition (new Vector2(3, 1), new Vector2(0, 0));
		//crate.setPosition (new Vector2(24, 24));

		//GTile tl = getTileFromWorldPoint (new Vector2 (crate.pos_x, crate.pos_y));
		//Debug.Log (tl.indexToString());
		//Debug.Log (tl.Contained_Objects.objectIsContained(crate));
		//Debug.Log (getTileFromWorldPoint(new Vector2(crate.pos_x, crate.pos_y)).indexToString());
		//Debug.Log (getTileFromWorldPoint(new Vector2(crate.pos_x, crate.pos_y)).Contained_Objects.objectIsContained(crate));
		//Debug.Log (tl.Contained_Objects.objectIsContained(crate));

		OBJ_RockItem rock = new OBJ_RockItem ();
		rock.setPosition (new Vector2(2, 1), new Vector2(0, 0));
		//rock.setPosition (new Vector2(24, 24));


		active_player = new OBJ_Player ();
		active_player.setPosition (new Vector2(50,50));
		//active_player.setPosition (getTile(new Vector2(0,0)).getCenter());

		for (int i = 0; i < 6; ++i) {
			GCharacter charac = new GCharacter ();
			charac.setPosition (new Vector2(200+UnityEngine.Random.Range(-100, 100), 200+UnityEngine.Random.Range(-100, 100)));
			charac.setBehaviour (new Behaviour_PaceRandomly());
		}
		// END TESTS

	}

	private void setupDataStructures() {
		// init tiles
		worldTiles = new GTile[data_settings.world_size, data_settings.world_size];
		for (int x = 0; x < data_settings.world_size; ++x) {
			for (int y = 0; y < data_settings.world_size; ++y) {
				worldTiles[x, y] = new GTile(x, y);
				/* test */
				if(UnityEngine.Random.Range(0,100) < 2) {
					worldTiles[x, y].biome = BiomeData.coniferous_forest;
				}
				/* end test */
			}
		}
		
		// init sectors
		sectorsWithinRange = new List<WorldSector> ();
		if (data_settings.world_size % data_settings.sector_size != 0) {
			Debug.LogError ("Invalid sector size: Must be multiple of world size");
		} 
		else {
			worldSectors = new WorldSector[data_settings.world_side_sectors, data_settings.world_side_sectors];
			for (int x = 0; x < data_settings.world_side_sectors; ++x) {
				for (int y = 0; y < data_settings.world_side_sectors; ++y) {
					worldSectors[x, y] = new WorldSector(
						x,
						y,
						x * data_settings.sector_size,
						x * data_settings.sector_size + data_settings.sector_size,
						y * data_settings.sector_size,
						y * data_settings.sector_size + data_settings.sector_size
						);
				}
			}
		}
	}

	private void adjustSectorsWithinRange() {
		sectorsWithinRange = getSectorArea (getSectorIndexesWithinRange(data_settings.within_range_radius));
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
		return getSectorArea (getSectorIndexesWithinRange(data_settings.render_radius));
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
			Mathf.Clamp ( Mathf.FloorToInt(tile_indexes.x / data_settings.sector_size), 0, data_settings.world_side_sectors-1 ),
			Mathf.Clamp ( Mathf.FloorToInt(tile_indexes.y / data_settings.sector_size), 0, data_settings.world_side_sectors-1 )
		);
		return sector_indexes;
	}

	public Vector2 worldPointToTileIndexes(Vector2 point) {
		Vector2 tile_indexes = new Vector2 (
			Mathf.Clamp ( Mathf.FloorToInt(point.x / data_settings.tile_width), 0, data_settings.world_size-1 ),
			Mathf.Clamp ( Mathf.FloorToInt(point.y / data_settings.tile_width), 0, data_settings.world_size-1 )
		);
		return tile_indexes;
	}

	public Vector2 tileIndexesToWorldPoint(Vector2 tile_indexes) {
		Vector2 world_point = new Vector2 (
			tile_indexes.x * data_settings.tile_width,
			tile_indexes.y * data_settings.tile_width
		);
		return world_point;
	}

	void updateCamFocusPoint() {
		if (active_player != null) {
			FocusPoint = new Vector2 (active_player.pos_x, active_player.pos_y);
		}
	}

}
