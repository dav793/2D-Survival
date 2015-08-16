using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	GameData class:
 *		Contains all data for world simulation, and operates the data every tick to reflect time progression.
 *		It is our model of the game world, in which the world and all of its constituents are represented.
 *		Its data reflects the state of the world as of the current tick. 
 *		It is not concerned with rendering, it only works on a conceptual world simulation.
 */

public class GameData : MonoBehaviour {

	public static GameData GData;
	public static Vector2 FocusPoint;

	private GTile[,] worldTiles;
	private WorldSector[,] worldSectors;
	private List<WorldSector> sectorsWithinRange;

	public GameData_Settings data_settings;
	
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
	}

	public void Init(GameData_Settings data_settings) {
		this.data_settings = data_settings;

		FocusPoint = Vector2.zero;

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

		// TESTS
		OBJ_SmallCrate crate = new OBJ_SmallCrate ();
		crate.placeAtPoint (new Vector2(24, 24));
		crate.addToTile (getTileFromWorldPoint(new Vector2(crate.pos_x, crate.pos_y)));
		//crate.removeFromTile ();

		OBJ_RockItem rock = new OBJ_RockItem ();
		rock.placeAtPoint (new Vector2(48, 48));
		rock.addToTile (getTileFromWorldPoint(new Vector2(rock.pos_x, rock.pos_y)));

		OBJ_Player player = new OBJ_Player ();
		player.placeAtPoint (new Vector2(72, 72));
		player.addToSector (getSectorFromWorldPoint(new Vector2(player.pos_x, player.pos_y)));
		// END TESTS

	}

	private void adjustSectorsWithinRange() {
		sectorsWithinRange = getSectorArea (getSectorIndexesWithinRange(data_settings.within_range_radius));
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

}
