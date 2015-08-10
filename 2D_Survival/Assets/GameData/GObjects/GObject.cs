using UnityEngine;
using System.Collections;

/*
 *	GObject class:
 *		A GObject can be one of the following elements:
 *			-GObject_Structure
 *			-GObject_Actor
 *			-GObject_Item
 */

public class GObject {
	public GTile tile;
	public float offset_x;
	public float offset_y;
	public GObject_RefLists_Index reflist_index;

	// variables for renderer use
	public bool is_rendered = false;

	public GObject() {
		offset_x = 0f;
		offset_y = 0f;
		reflist_index = new GObject_RefLists_Index ();
	}

}
