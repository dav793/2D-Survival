using UnityEngine;
using System.Collections;

public class GTile {
	public int index_x;
	public int index_y;
	public TileOperationMode operation_mode;
	public GObject_RefLists Contained_Objects; 

	// variables for renderer use
	public bool is_rendered = false;

	public GTile(int index_x, int index_y) {
		this.index_x = index_x;
		this.index_y = index_y;
		operation_mode = TileOperationMode.WITHIN_CHARACTER_RANGE;			// test, testing: change to out of range by default
		Contained_Objects = new GObject_RefLists ();
		//Debug.Log (Contained_Objects.structures.movable[(int)BOOL_YN.YES].Count);
	}

}
