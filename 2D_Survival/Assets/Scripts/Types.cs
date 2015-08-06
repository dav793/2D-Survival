using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TileVertices { topLeft, topRight, bottomRight, bottomLeft };

public enum RenderObjectUpdateOperations { CREATE, DESTROY, UPDATE_POSITION, UPDATE_BRIGHTNESS, UPDATE_TINT, UPDATE_TRANSPARENCY, UPDATE_ANIMATION };

public enum RenderTerrainUpdateOperations { CREATE, DESTROY, UPDATE_BRIGHTNESS, UPDATE_TINT };

public enum RenderLayers { TERRAIN, WORLD, HUD, UI };

public enum TileOperationMode { OUT_OF_CHARACTER_RANGE, WITHIN_CHARACTER_RANGE };

public enum BOOL_YN { YES, NO };

public struct GameData_Settings {
	public int world_size_x;
	public int world_size_y;
};

public class Types {
	
}

/*
 *	Renderer data types
 */

public class RendererObjectUpdateQueues {

	public Queue<GameObject> create;
	public Queue<GameObject> destroy;
	public Queue<GameObject> update_position;
	public Queue<GameObject> update_brightness;
	public Queue<GameObject> update_tint;
	public Queue<GameObject> update_transparency;
	public Queue<GameObject> update_animation;

	public RendererObjectUpdateQueues() {
		create = new Queue<GameObject> ();
		destroy = new Queue<GameObject> ();
		update_position = new Queue<GameObject> ();
		update_brightness = new Queue<GameObject> ();
		update_tint = new Queue<GameObject> ();
		update_transparency = new Queue<GameObject> ();
		update_animation = new Queue<GameObject> ();
	}

	public Queue<GameObject> getOperationQueue(RenderObjectUpdateOperations operation) {
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

public class RendererTerrainUpdateQueues {

	public Queue<GameObject> create;
	public Queue<GameObject> destroy;
	public Queue<GameObject> update_brightness;
	public Queue<GameObject> update_tint;

	public RendererTerrainUpdateQueues() {
		create = new Queue<GameObject> ();
		destroy = new Queue<GameObject> ();
		update_brightness = new Queue<GameObject> ();
		update_tint = new Queue<GameObject> ();
	}

	public Queue<GameObject> getOperationQueue(RenderTerrainUpdateOperations operation) {
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

/*
 *	End of renderer data types 
 */

/*
 *	Game data types 
 */

/*
 * GObject_RefLists data type, used for holding organized references of GObjects, arranged in Lists for classification:
 * 
 * 		In the following tree, which is used for classification of GObject references within the GObject_RefLists type, 
 * 		every leaf represents a list of GObjects, whose containing GObjects meet some criteria.
 * 
 * 										 	  	   GObject_RefLists
 * 													 	  |
 * 					+-------------------------------------+------------------------------+
 * 					|			 		 			 	  |					   	   		 |
 * 			 GObject_Structures	 			   	   GObject_Actors		 		  GObject_Items
 * 					|								 	  |					  			 |
 * 			+-------+-------+---------+		 	  +-------+-------+-----------+			All
 * 			|			  	|		  |		 	  |				  |			  |
 * 	  interactable		 movable	 All  		mobile	    environmental    All
 * 			|				|				 	  |   			  |
 * 		+-------+ 		+-------+		 	  +-------+ 	  +-------+
 * 		|		| 		| 		|		 	  |	 	  | 	  |    	  |
 * 	   Yes	    No	   Yes		No			 Yes	  No	 Yes	  No
 * 
 */
public class GObject_RefLists {
	public GObject_Structures_RefList structures;
	public GObject_Actors_RefList actors;
	public GObject_Items_RefList items;
	public GObject_RefLists() {
		structures = new GObject_Structures_RefList ();
		actors = new GObject_Actors_RefList ();
		items = new GObject_Items_RefList ();
	}
}

public class GObject_Structures_RefList {
	public List<GObject> all;
	public List<GObject>[] interactable;
	public List<GObject>[] movable;
	public GObject_Structures_RefList() {
		all = new List<GObject> ();
		
		interactable = new List<GObject>[2];
		interactable[(int)BOOL_YN.NO] = new List<GObject> ();
		interactable[(int)BOOL_YN.YES] = new List<GObject> ();
		
		movable = new List<GObject>[2];
		movable[(int)BOOL_YN.NO] = new List<GObject> ();
		movable[(int)BOOL_YN.YES] = new List<GObject> ();
	}
}

public class GObject_Actors_RefList {
	public List<GObject> all;
	public List<GObject>[] mobile;
	public List<GObject>[] environmental;
	public GObject_Actors_RefList() {
		all = new List<GObject> ();

		mobile = new List<GObject>[2];
		mobile[(int)BOOL_YN.NO] = new List<GObject> ();
		mobile[(int)BOOL_YN.YES] = new List<GObject> ();

		environmental = new List<GObject>[2];
		environmental[(int)BOOL_YN.NO] = new List<GObject> ();
		environmental[(int)BOOL_YN.YES] = new List<GObject> ();
	}
}

public class GObject_Items_RefList {
	public List<GObject> all;
	public GObject_Items_RefList() {
		all = new List<GObject> ();
	}
}

/*
 * End of GObject_RefLists
 */

/*
 * End of game data types
 */

