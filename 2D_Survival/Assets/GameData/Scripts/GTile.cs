using UnityEngine;
using System.Collections;

public class GTile {
	public int index_x;
	public int index_y;
	public GObject_RefLists Contained_Objects; 

	// variables for renderer use
	public bool is_rendered = false;

	public GTile(int index_x, int index_y) {
		this.index_x = index_x;
		this.index_y = index_y;
		Contained_Objects = new GObject_RefLists ();
	}

}
