﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
			}
		}

		// init sectors
		if (data_settings.world_size % data_settings.sector_size != 0) {
			Debug.LogError ("Invalid sector size: Must be multiple of world size");
		} 
		else {
			sectorsWithinRange = new List<WorldSector> ();
			worldSectors = new WorldSector[data_settings.sectors_x, data_settings.sectors_y];
			for (int x = 0; x < data_settings.sectors_x; ++x) {
				for (int y = 0; y < data_settings.sectors_y; ++y) {
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

	private Vector2 worldPointToSectorIndexes(Vector2 point) {
		return tileIndexesToSectorIndexes( worldPointToTileIndexes(point) );
	}

	private Vector2 tileIndexesToSectorIndexes(Vector2 tile_indexes) {
		Vector2 sector_indexes = new Vector2 (
			Mathf.Clamp ( Mathf.FloorToInt(tile_indexes.x / data_settings.sector_size), 0, data_settings.sectors_x-1 ),
		    Mathf.Clamp ( Mathf.FloorToInt(tile_indexes.y / data_settings.sector_size), 0, data_settings.sectors_y-1 )
		);
		return sector_indexes;
	}

	private Vector2 worldPointToTileIndexes(Vector2 point) {
		Vector2 tile_indexes = new Vector2 (
			Mathf.Clamp ( Mathf.FloorToInt(point.x / data_settings.tile_width), 0, data_settings.world_size-1 ),
			Mathf.Clamp ( Mathf.FloorToInt(point.y / data_settings.tile_width), 0, data_settings.world_size-1 )
		);
		return tile_indexes;
	}

}
