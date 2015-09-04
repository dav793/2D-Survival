using UnityEngine;
using System.Collections;

/*
 *	Class: GObject
 *		GObject is a base class for the following objects:
 *			-GObject_Structure
 *			-GObject_Actor
 *			-GObject_Item
 *
 *		This is the most basic definition of a gobject, and every gobject must extend from this class
 */

public class GObject {
	public int object_index;
	public float pos_x;
	public float pos_y;
	public GObjectType type;

	// variables for renderer use
	public bool is_rendered = false;
	public GameObject renderedGameObject;
	public Sprite sprite;

	public GObject() {
		pos_x = 0f;
		pos_y = 0f;					
	}

	public virtual void setPosition(Vector2 point) {

	}

	public virtual void setPosition(Vector2 tile_index, Vector2 offset) {
		
	}

	public virtual void setPosition(GTile tile, Vector2 offset) {
	
	}

	public Vector2 getPosition() {
		return new Vector2 (pos_x, pos_y);
	}

	public void placeAtPoint(Vector2 point) {
		pos_x = point.x;
		pos_y = point.y;
	}

	// procedures for renderer use
	public void linkGameObject(GameObject gobject) {
		renderedGameObject = gobject;
		is_rendered = true;
	}
	
	public void unlinkGameObject() {
		renderedGameObject = null;
		is_rendered = false;
	}

}
