using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BiomeTypes { TropicalForest, ConiferousForest, Desert, AbandonedSettlement };

public enum TileVertices { topLeft, topRight, bottomRight, bottomLeft };

public enum RenderObjectUpdateOperations { CREATE, DESTROY, UPDATE_POSITION, UPDATE_BRIGHTNESS, UPDATE_TINT, UPDATE_TRANSPARENCY, UPDATE_ANIMATION };

public enum RenderTerrainUpdateOperations { CREATE, DESTROY, UPDATE_BRIGHTNESS, UPDATE_TINT };

public enum RenderLayers { TERRAIN, WORLD, HUD, UI };

public enum OperationMode { OUT_OF_CHARACTER_RANGE, WITHIN_CHARACTER_RANGE };

public enum GObjectType { Structure, Actor, Item };

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

public class RefList_DataStructure<T> {
	List<T> data;
	public RefList_DataStructure() {
		data = new List<T> ();
	}
	public int count() {
		return data.Count;
	}
	public int addObject(T obj) {
		data.Add (obj);
		return data.Count - 1;
	} 
	public void removeObjectAt(int position) {
		data.RemoveAt (position);
	}
	public T getObjectAt(int position) {
		return data [position];
	}
}

public class GObject_Tile_RefList {
	public Vector2 tile_index;
	public GObject_Structures_RefList structures;
	public GObject_Items_RefList items;
	public GObject_Tile_RefList(Vector2 tile_index) {
		this.tile_index = tile_index;
		structures = new GObject_Structures_RefList (tile_index);
		items = new GObject_Items_RefList (tile_index);
	}
	public void addObject(GObject obj) {
		switch (obj.type) {
			case GObjectType.Structure:
				structures.addObject((GStructure)obj);
				break;
			case GObjectType.Item:
				items.addObject((GItem)obj);
				break;
			default:
				Debug.LogError("Invalid GObject type");
				break;
		}
	}
	public void removeObject(GObject obj) {
		switch (obj.type) {
		case GObjectType.Structure:
			structures.removeObject((GStructure)obj);
			break;
		case GObjectType.Item:
			items.removeObject((GItem)obj);
			break;
		default:
			Debug.LogError("Invalid GObject type");
			break;
		}
	}
}

public class GObject_Sector_RefList {
	public Vector2 sector_index;
	public GObject_Actors_RefList actors;
	public GObject_Sector_RefList(Vector2 sector_index) {
		this.sector_index = sector_index;
		actors = new GObject_Actors_RefList (sector_index);
	}
	public void addObject(GObject obj) {
		if (obj.type == GObjectType.Actor) {
			actors.addObject ((GActor)obj);
		} 
		else {
			Debug.LogError ("Invalid GObject type");
		}
	}
	public void removeObject(GObject obj) {
		if (obj.type == GObjectType.Actor) {
			actors.removeObject ((GActor)obj);
		} 
		else {
			Debug.LogError ("Invalid GObject type");
		}
	}
}

public class GStructure_RefList_Index {
	public int all;
	public int interactive;
	public int movable;
	public int environmental;
}
public class GObject_Structures_RefList {
	public Vector2 container_index;
	public RefList_DataStructure<GStructure> all;
	public RefList_DataStructure<GStructure>[] interactive;
	public RefList_DataStructure<GStructure>[] movable;
	public RefList_DataStructure<GStructure>[] environmental;
	public GObject_Structures_RefList(Vector2 tile_index) {
		this.container_index = tile_index;

		all = new RefList_DataStructure<GStructure> ();
		
		interactive = new RefList_DataStructure<GStructure>[2];
		interactive[(int)BOOL_YN.NO] = new RefList_DataStructure<GStructure> ();
		interactive[(int)BOOL_YN.YES] = new RefList_DataStructure<GStructure> ();
		
		movable = new RefList_DataStructure<GStructure>[2];
		movable[(int)BOOL_YN.NO] = new RefList_DataStructure<GStructure> ();
		movable[(int)BOOL_YN.YES] = new RefList_DataStructure<GStructure> ();

		environmental = new RefList_DataStructure<GStructure>[2];
		environmental[(int)BOOL_YN.NO] = new RefList_DataStructure<GStructure> ();
		environmental[(int)BOOL_YN.YES] = new RefList_DataStructure<GStructure> ();
	}
	public void addObject(GStructure obj) {
		obj.reflist_index.all = all.count();
		all.addObject (obj);

		obj.reflist_index.interactive = interactive[(int)obj.interactive].count();
		interactive [(int)obj.interactive].addObject (obj);

		obj.reflist_index.movable = movable[(int)obj.movable].count();
		movable [(int)obj.movable].addObject (obj);

		obj.reflist_index.environmental = environmental[(int)obj.environmental].count();
		environmental [(int)obj.environmental].addObject (obj);
	}
	public void removeObject(GStructure obj) {
		all.removeObjectAt (obj.reflist_index.all);
		for (int i = obj.reflist_index.all; i < all.count(); ++i) {
			all.getObjectAt(i).reflist_index.all--;
		}

		interactive[(int)obj.interactive].removeObjectAt (obj.reflist_index.interactive);
		for (int i = obj.reflist_index.interactive; i < interactive[(int)obj.interactive].count(); ++i) {
			interactive[(int)obj.interactive].getObjectAt(i).reflist_index.interactive--;
		}

		movable[(int)obj.movable].removeObjectAt (obj.reflist_index.movable);
		for (int i = obj.reflist_index.movable; i < movable[(int)obj.movable].count(); ++i) {
			movable[(int)obj.movable].getObjectAt(i).reflist_index.movable--;
		}

		environmental[(int)obj.environmental].removeObjectAt (obj.reflist_index.environmental);
		for (int i = obj.reflist_index.environmental; i < environmental[(int)obj.environmental].count(); ++i) {
			environmental[(int)obj.environmental].getObjectAt(i).reflist_index.environmental--;
		}
	}
}

public class GActor_RefList_Index {
	public int all;
	public int environmental;
	public int NPCs;
}
public class GObject_Actors_RefList {
	public Vector2 container_index;
	public RefList_DataStructure<GActor> all;
	public RefList_DataStructure<GActor>[] environmental;
	public RefList_DataStructure<GActor>[] NPCs;
	public GObject_Actors_RefList(Vector2 sector_index) {
		this.container_index = sector_index;
		all = new RefList_DataStructure<GActor> ();

		environmental = new RefList_DataStructure<GActor>[2];
		environmental[(int)BOOL_YN.NO] = new RefList_DataStructure<GActor> ();
		environmental[(int)BOOL_YN.YES] = new RefList_DataStructure<GActor> ();

		NPCs = new RefList_DataStructure<GActor>[2];
		NPCs[(int)BOOL_YN.NO] = new RefList_DataStructure<GActor> ();
		NPCs[(int)BOOL_YN.YES] = new RefList_DataStructure<GActor> ();
	}
	public void addObject(GActor obj) {
		obj.reflist_index.all = all.count();
		all.addObject (obj);
		
		obj.reflist_index.environmental = environmental[(int)obj.environmental].count();
		environmental [(int)obj.environmental].addObject (obj);

		obj.reflist_index.NPCs = NPCs[(int)obj.npc].count();
		NPCs [(int)obj.npc].addObject (obj);
	}
	public void removeObject(GActor obj) {
		all.removeObjectAt (obj.reflist_index.all);
		for (int i = obj.reflist_index.all; i < all.count(); ++i) {
			all.getObjectAt(i).reflist_index.all--;
		}
		
		environmental[(int)obj.environmental].removeObjectAt (obj.reflist_index.environmental);
		for (int i = obj.reflist_index.environmental; i < environmental[(int)obj.environmental].count(); ++i) {
			environmental[(int)obj.environmental].getObjectAt(i).reflist_index.environmental--;
		}

		NPCs[(int)obj.npc].removeObjectAt (obj.reflist_index.NPCs);
		for (int i = obj.reflist_index.NPCs; i < NPCs[(int)obj.npc].count(); ++i) {
			NPCs[(int)obj.npc].getObjectAt(i).reflist_index.NPCs--;
		}
	}
}

public class GItem_RefList_Index {
	public int all;
}
public class GObject_Items_RefList {
	public Vector2 container_index;
	public RefList_DataStructure<GItem> all;
	public GObject_Items_RefList(Vector2 tile_index) {
		this.container_index = tile_index;
		all = new RefList_DataStructure<GItem> ();
	}
	public void addObject(GItem obj) {
		obj.reflist_index.all = all.count();
		all.addObject (obj);
	}
	public void removeObject(GItem obj) {
		all.removeObjectAt (obj.reflist_index.all);
		for (int i = obj.reflist_index.all; i < all.count(); ++i) {
			all.getObjectAt(i).reflist_index.all--;
		}
	}
}

/*
 * End of GObject_RefLists
 */

/*
 * End of game data types
 */

