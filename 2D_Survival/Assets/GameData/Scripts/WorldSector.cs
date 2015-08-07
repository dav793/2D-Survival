using UnityEngine;
using System.Collections;

public class WorldSector {

	public int index_x;
	public int index_y;
	public int lower_boundary_x;
	public int upper_boundary_x;
	public int lower_boundary_y;
	public int upper_boundary_y;
	public OperationMode mode;
	
	// variables for renderer use
	public bool is_rendered = false;
	public GameObject terrainGameObject;
	
	public WorldSector(int index_x, int index_y, int lower_boundary_x, int upper_boundary_x, int lower_boundary_y, int upper_boundary_y) {
		this.index_x = index_x;
		this.index_y = index_y;
		this.lower_boundary_x = lower_boundary_x;
		this.upper_boundary_x = upper_boundary_x;
		this.lower_boundary_y = lower_boundary_y;
		this.upper_boundary_y = upper_boundary_y;
		mode = OperationMode.OUT_OF_CHARACTER_RANGE;			
	}

	// procedures for renderer use
	public void linkGameObject(GameObject gobject) {
		terrainGameObject = gobject;
	}

	public void unlinkGameObject() {
		terrainGameObject = null;
	}

}
