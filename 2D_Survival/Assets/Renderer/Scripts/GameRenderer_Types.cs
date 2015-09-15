using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *  File: GameRenderer_Types
 *  Contains auxiliary/utility types and classes for use within the GameRenderer component.
 */

public enum DepthLevel { TERRAIN, OBJECTS, OVERLAY, CAMERA };
public enum RenderLayers { TERRAIN, WORLD, HUD, UI };
public enum RenderObjectUpdateOperations { CREATE, DESTROY, UPDATE_POSITION, UPDATE_BRIGHTNESS, UPDATE_TINT, UPDATE_TRANSPARENCY, UPDATE_ANIMATION };
public enum RenderTerrainUpdateOperations { CREATE, DESTROY, UPDATE_BRIGHTNESS, UPDATE_TINT };

public struct Renderer_Settings {
	public int gobject_pool_size;
	public int terrain_pool_size;
};

/*
 *  Class: RendererObjectUpdateQueues
 * 
 *  Data structure which contains a Queue of GObjects for each rendering operation. Whenever a rendering operation needs to be performed
 *  on a GObject, such GObject must be added to the queue for the corresponding operation. The GameRenderer component will sistematically 
 *  dequeue every GObject and perform the corresponding rendering operation on it.
 * 
 *  The following are the available rendering operations for a GObject:
 * 		-create
 * 		-destroy
 * 		-update position
 * 		-update brightness
 * 		-update tint
 * 		-update transparency
 */
public class RendererObjectUpdateQueues {
	
	public Queue<GObject> create;
	public Queue<GObject> destroy;
	public Queue<GObject> update_position;
	public Queue<GObject> update_brightness;
	public Queue<GObject> update_tint;
	public Queue<GObject> update_transparency;
	public Queue<GObject> update_animation;
	
	public RendererObjectUpdateQueues() {
		create = new Queue<GObject> ();
		destroy = new Queue<GObject> ();
		update_position = new Queue<GObject> ();
		update_brightness = new Queue<GObject> ();
		update_tint = new Queue<GObject> ();
		update_transparency = new Queue<GObject> ();
		update_animation = new Queue<GObject> ();
	}
	
	public Queue<GObject> getOperationQueue(RenderObjectUpdateOperations operation) {
		switch(operation) {
		case RenderObjectUpdateOperations.CREATE:
			return create;
		case RenderObjectUpdateOperations.DESTROY:
			return destroy;
		case RenderObjectUpdateOperations.UPDATE_POSITION:
			return update_position;
		case RenderObjectUpdateOperations.UPDATE_BRIGHTNESS:
			return update_brightness;
		case RenderObjectUpdateOperations.UPDATE_TINT:
			return update_tint;
		case RenderObjectUpdateOperations.UPDATE_TRANSPARENCY:
			return update_transparency;
		case RenderObjectUpdateOperations.UPDATE_ANIMATION:
			return update_animation;
		}
		return null;
	}
	
}

/*
 *  Class: RendererTerrainUpdateQueues
 * 
 *  Data structure which contains a Queue of WorldSectors for each rendering operation. Whenever a rendering operation needs to be performed
 *  on a WorldSector, such sector must be added to the queue for the corresponding operation. The GameRenderer component will sistematically 
 *  dequeue every WorldSector and perform the corresponding rendering operation on it.
 *  
 *  The following are the available rendering operations for a WorldSector:
 * 		-create
 * 		-destroy
 * 		-update brightness
 * 		-update tint
 */
public class RendererTerrainUpdateQueues {
	
	public Queue<WorldSector> create;
	public Queue<WorldSector> destroy;
	public Queue<WorldSector> update_brightness;
	public Queue<WorldSector> update_tint;
	
	public RendererTerrainUpdateQueues() {
		create = new Queue<WorldSector> ();
		destroy = new Queue<WorldSector> ();
		update_brightness = new Queue<WorldSector> ();
		update_tint = new Queue<WorldSector> ();
	}
	
	public Queue<WorldSector> getOperationQueue(RenderTerrainUpdateOperations operation) {
		switch(operation) {
		case RenderTerrainUpdateOperations.CREATE:
			return create;
		case RenderTerrainUpdateOperations.DESTROY:
			return destroy;
		case RenderTerrainUpdateOperations.UPDATE_BRIGHTNESS:
			return update_brightness;
		case RenderTerrainUpdateOperations.UPDATE_TINT:
			return update_tint;
		}
		return null;
	}
	
}
