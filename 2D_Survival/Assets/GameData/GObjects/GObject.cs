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
}
