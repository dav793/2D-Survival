using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BiomeTypes { TropicalForest, ConiferousForest, Desert, AbandonedSettlement };

public enum TileVertices { topLeft, topRight, bottomRight, bottomLeft };

public enum RenderObjectUpdateOperations { CREATE, DESTROY, UPDATE_POSITION, UPDATE_BRIGHTNESS, UPDATE_TINT, UPDATE_TRANSPARENCY, UPDATE_ANIMATION };

public enum RenderTerrainUpdateOperations { CREATE, DESTROY, UPDATE_BRIGHTNESS, UPDATE_TINT };

public enum RenderLayers { TERRAIN, WORLD, HUD, UI };

public enum OperationMode { OUT_OF_CHARACTER_RANGE, WITHIN_CHARACTER_RANGE };

public enum BOOL_YN { YES, NO };

public struct GameData_Settings {
	public int world_size;					// world side length in tiles
	public int sector_size;					// sector side length in tiles
	public int world_side_sectors;			// world side length in sectors
	public float tile_width;				// tile side length in units (pixels)
	public int within_range_radius;			// tile radius around focus point to be within range
	public int render_radius;				// tile radius around focus point to be within render range
};

public class Types {
	
}

/*
 *	Renderer data types
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
 * 			
 * 		Structures:
 * 			Interactable - object with which some actors may interact
 * 				interactables: table, barrel, coffee maker, door, sofa...
 * 				non-interactables: wall, statue, column...
 * 
 * 			Movable - can be moved after their initial placement
 * 				movables: coffee maker, barrel, sofa...
 * 				non-movables: door, wall, window...
 * 
 * 		Actors:
 * 			Mobile - may move and/or act through its own accord
 * 				mobiles: player, NPCs, animals, critters
 * 				non-mobiles: trees, vegetation, rocks			
 * 
 * 			Environmental - has an effect in the environmental balance of the sector. typically any member of a species is environmental.
 * 				environmentals: animals, vegetation, salty rock
 * 				non-environmentals: player, NPCs, other inanimate actors like aestethical rocks (maybe?)
 * 
 * 		Items: items which may be picked up and carried inside an inventory, which are in their dropped form in the world
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

public class GObject_RefLists_Index {
	public GObject_Structures_RefList_Index structures;
	public GObject_Actors_RefList_Index actors;
	public GObject_Items_RefList_Index items;
	public GObject_RefLists_Index() {
		structures = new GObject_Structures_RefList_Index ();
		actors = new GObject_Actors_RefList_Index ();
		items = new GObject_Items_RefList_Index ();
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

public class GObject_Structures_RefList_Index {
	public int all;
	public int[] interactable;
	public int[] movable;
	public GObject_Structures_RefList_Index() {
		all = 0;
		interactable = new int[2] {0, 0};
		movable = new int[2] {0, 0};
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

public class GObject_Actors_RefList_Index {
	public int all;
	public int[] mobile;
	public int[] environmental;
	public GObject_Actors_RefList_Index(){
		all = 0;
		mobile = new int[2] {0, 0};
		environmental = new int[2] {0, 0};
	}
}

public class GObject_Items_RefList {
	public List<GObject> all;
	public GObject_Items_RefList() {
		all = new List<GObject> ();
	}
}

public class GObject_Items_RefList_Index {
	public int all;
	public GObject_Items_RefList_Index() {
		all = 0;
	}
}

/*
 * End of GObject_RefLists
 */

/*
 * End of game data types
 */

