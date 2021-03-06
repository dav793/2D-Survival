﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *  File: GameData_Types
 *  Contains auxiliary/utility types and classes for use within the GameData component.
 */

public enum GObjectType { Structure, Actor, Item, Player, Character, Animal };

public enum CharBodyPart { Hair, Head, Torso, Arms, Legs, Feet };

public enum CharacterSlots { Hat, Hair, Eyewear, Mask, RHand, LHand, Gloves, JSleeves, Sleeves, JacketTorso, Bodysuit, Pants, Shoes };

public enum TileSubdivisions { TopLeft, TopRight, BottomLeft, BottomRight};

public enum StructureSizes { Small, Medium, Large };

public enum GStructurePropertiesType { 
	BlockingVegetation, 
	NonBlockingVegetation, 
	InteractiveProp, 
	BlockingSceneryProp, 
	NonBlockingSceneryProp, 
	BlockingSceneryPropMovable, 
	NonBlockingSceneryPropMovable 
};

public enum GStructureDimensionsType { Small, Medium, Large_2Tiles };

public enum GActorPropertiesType { NPC, nonNPC, Animal };

public struct GameData_Settings {
	public int world_size;					// world side length in tiles
	public int sector_size;					// sector side length in tiles
	public int world_side_sectors;			// world side length in sectors
	public float tile_width;				// tile side length in units (pixels)
	public int within_range_radius;			// tile radius around focus point to be within range
	public int render_radius;				// tile radius around focus point to be within render range
};

public class GStructureDimensions {
	
	public StructureSizes size;
	int dim_matrix_size;
	int[,] dim_matrix;
	Vector2 dim_matrix_origin;
	
	public GStructureDimensions(StructureSizes size) {
		this.size = size;
		if ((int)size > (int)StructureSizes.Medium) {
			Debug.LogError("You must provide a dimension matrix and its size for structures with a size greater than Medium. Use the other constructor.");
		}
	}
	
	public GStructureDimensions(StructureSizes size, int dim_matrix_size, int[,] dim_matrix) {
		this.size = size;
		this.dim_matrix_size = dim_matrix_size;
		this.dim_matrix = dim_matrix;
		dim_matrix_origin = Vector2.zero;
	}

	public GStructureDimensions(StructureSizes size, int dim_matrix_size, int[,] dim_matrix, Vector2 dim_matrix_origin) {
		this.size = size;
		this.dim_matrix_size = dim_matrix_size;
		this.dim_matrix = dim_matrix;
		this.dim_matrix_origin = dim_matrix_origin;
	}

	public static GStructureDimensions GetDefaultDimensions(GStructureDimensionsType type) {
		switch (type) {
		case GStructureDimensionsType.Small:
			return new GStructureDimensions(StructureSizes.Small);
			break;
		case GStructureDimensionsType.Medium:
			return new GStructureDimensions(StructureSizes.Medium);
			break;
		case GStructureDimensionsType.Large_2Tiles:
			return new GStructureDimensions(StructureSizes.Large, 2, 
				new int[2,2] {	
					{1,0},
					{1,0}
				}
			);
			break;
		}
		return null;
	}

	public static StructureSizes getRandomSize() {
		int index = UnityEngine.Random.Range (0, System.Enum.GetNames (typeof(StructureSizes)).Length);
		return (StructureSizes) index;
	}

}

public class GStructureProperties {
	public BOOL_YN interactive;
	public BOOL_YN movable;
	public BOOL_YN environmental;
	public bool is_blocking;
	public CardinalDirections orientation;
	public GStructureProperties(BOOL_YN interactive, BOOL_YN movable, BOOL_YN environmental) {
		this.interactive = interactive;
		this.movable = movable;
		this.environmental = environmental;
		is_blocking = false;
		orientation = CardinalDirections.S;
	}
	public GStructureProperties(BOOL_YN interactive, BOOL_YN movable, BOOL_YN environmental, bool is_blocking) {
		this.interactive = interactive;
		this.movable = movable;
		this.environmental = environmental;
		this.is_blocking = is_blocking;
		orientation = CardinalDirections.S;
	}
	public static GStructureProperties GetDefaultProperties(GStructurePropertiesType type) {
		switch (type) {
		case GStructurePropertiesType.BlockingVegetation:
			return new GStructureProperties(BOOL_YN.YES, BOOL_YN.NO, BOOL_YN.YES, true);
			break;
		case GStructurePropertiesType.NonBlockingVegetation:
			return new GStructureProperties(BOOL_YN.YES, BOOL_YN.NO, BOOL_YN.YES);
			break;
		case GStructurePropertiesType.InteractiveProp:
			return new GStructureProperties(BOOL_YN.YES, BOOL_YN.YES, BOOL_YN.NO);
			break;
		case GStructurePropertiesType.NonBlockingSceneryProp:
			return new GStructureProperties(BOOL_YN.NO, BOOL_YN.NO, BOOL_YN.NO);
			break;
		case GStructurePropertiesType.BlockingSceneryProp:
			return new GStructureProperties(BOOL_YN.NO, BOOL_YN.NO, BOOL_YN.NO, true);
			break;
		case GStructurePropertiesType.NonBlockingSceneryPropMovable:
			return new GStructureProperties(BOOL_YN.NO, BOOL_YN.YES, BOOL_YN.NO);
			break;
		case GStructurePropertiesType.BlockingSceneryPropMovable:
			return new GStructureProperties(BOOL_YN.NO, BOOL_YN.NO, BOOL_YN.NO, true);
			break;
		}
		return null;
	}
}

public class GActorProperties {
	public BOOL_YN environmental;
	public BOOL_YN npc;
	public GActorProperties(BOOL_YN environmental, BOOL_YN npc) {
		this.environmental = environmental;
		this.npc = npc;
	}
	public static GActorProperties GetDefaultProperties(GActorPropertiesType type) {
		switch (type) {
		case GActorPropertiesType.NPC:
			return new GActorProperties(BOOL_YN.NO, BOOL_YN.YES);
			break;
		case GActorPropertiesType.nonNPC:
			return new GActorProperties(BOOL_YN.NO, BOOL_YN.NO);
			break;
		case GActorPropertiesType.Animal:
			return new GActorProperties(BOOL_YN.YES, BOOL_YN.NO);
			break;
		}
		return null;
	}
}

/*
 * RefLists are used for holding organized references of GObjects, classified as follows:
 * 
 * 		Structures:
 * 			Interactable - object with which some actors may interact
 * 				interactables: table, barrel, coffee maker, door, sofa...
 * 				non-interactables: wall, statue, column...
 * 
 * 			Movable - can be moved after their initial placement, without having to be destroyed
 * 				movables: coffee maker, barrel, sofa...
 * 				non-movables: door, wall, window, tree...
 * 
 * 			Environmental - has an effect in the biological balance of the sector.
 * 				environmentals: vegetation, salty rock
 * 				non-environmentals: wall, table, aestethical rocks and scenery
 * 
 * 		Actors:
 * 			Environmental - has an effect in the biological balance of the sector.
 * 				environmentals: rabbit, wolf, bear
 * 				non-environmentals: player, NPCs, bandit
 * 			
 * 			NPC - Non-Player-Character, typically a humanoid, with which other actors such as the player can 
 * 			do high-level interaction (speaking, bartering...)
 * 				NPCs: shopkeeper, villager, hunter, craftsman
 * 				non-NPCs: Bandit, zombie, animals
 * 
 * 		Items: items which may be picked up and carried inside an inventory, which are in their dropped form in the world
 */

/*
 *  Class: RefList_DataStructure
 * 
 *  -The most basic data structure used to hold references for GObjects. Every GObject RefList has access to one or more instances of this class. 
 *  -It contains basic operations for simple storing and removing of GObjects.
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
	public List<T> getDataList() {
		return data;
	}
	/*public bool findObject(T obj) {
		return true;
	}*/
}
	
/*
 *  Class: GObject_Tile_RefList
 * 
 *  -Data structure used to hold references for GObjects in a per-tile level. Every GTile has access to a unique instance of this class, 
 *  and every tile-specific or tile-referenced GObject (such as a GStructure) that is contained by a GTile is stored in its instance of
 *  this class.
 */
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
	public bool objectIsContained(GObject obj) {
		switch (obj.type) {
		case GObjectType.Structure:
			return structures.objectIsContained((GStructure)obj);
		case GObjectType.Item:
			return items.objectIsContained((GItem)obj);
		default:
			Debug.LogError("Invalid GObject type");
			break;
		}
		return false;
	}
}
	
/*
 *  Class: GObject_Sector_RefList
 * 
 *  -Data structure used to hold references for GObjects in a per-sector level. Every WorldSector has access to a unique instance of this class, 
 *  and every sector-specific or sector-referenced GObject (such as a GActor) that is contained by a WorldSector is stored in its instance of
 *  this class.
 */
public class GObject_Sector_RefList {
	public Vector2 sector_index;
	public GObject_Actors_RefList actors;
	public GObject_Sector_RefList(Vector2 sector_index) {
		this.sector_index = sector_index;
		actors = new GObject_Actors_RefList (sector_index);
	}
	public void addObject(GObject obj) {
		GActor act = obj as GActor;
		if (act != null) {
			actors.addObject (act);
		} 
		else {
			Debug.LogError ("Invalid GObject type");
		}
	}
	public void removeObject(GObject obj) {
		GActor act = obj as GActor;
		if (act != null) {
			actors.removeObject (act);
		} 
		else {
			Debug.LogError ("Invalid GObject type");
		}
	}
	public bool objectIsContained(GObject obj) {
		GActor act = obj as GActor;
		if (act != null) {
			return actors.objectIsContained (act);
		} 
		else {
			Debug.LogError ("Invalid GObject type");
		}
		return false;
	}
}
	
/*
 *  Class: GStructure_RefList_Index
 * 
 *  -Data structure used to hold the indexes a specific GStructure possesses in the GObject_Tile_RefList which contains it.
 *  -It is a key for finding an object (the GStructure) within its container (the tiles GObject_Tile_RefList).
 */
public class GStructure_RefList_Index {
	public int all;
	public int interactive;
	public int movable;
	public int environmental;
	public TileSubdivisions subdivision;
}
	
/*
 *  Class: GObject_Structures_RefList
 * 
 *  Data structure which holds the references for all GStructures contained by a tiles GObject_Tile_RefList.
 *  Also classifies the contained GStructures by storing them in different pairs of RefList_DataStructures, each
 *  pair representing a certain condition (interactive, movable...) and each RefList_DataStructure inside the pair 
 *  representing the fulfillment (or not) of that condition.
 * 
 * 	For example, interactive[0] contains references for interactive GStructures, while interactive[1] contains
 *  references for non-interactive GStructures.
 *  
 *  Note that BOOL_YN.YES = 0 and BOOL_YN.NO = 1. This way the RefList_DataStructure containing interactive or
 *  non-interactive GStructures within this GObject_Structures_RefList can be accessed with interactive[BOOL_YN.YES]
 *  and interactive[BOOL_YN.NO]. This way we have a more intuitive way of accessing the appropriate RefList_DataStructure 
 *  inside a pair. 
 */
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
		
		obj.reflist_index.interactive = interactive[(int)obj.properties.interactive].count();
		interactive [(int)obj.properties.interactive].addObject (obj);
		
		obj.reflist_index.movable = movable[(int)obj.properties.movable].count();
		movable [(int)obj.properties.movable].addObject (obj);
		
		obj.reflist_index.environmental = environmental[(int)obj.properties.environmental].count();
		environmental [(int)obj.properties.environmental].addObject (obj);
	}
	public void removeObject(GStructure obj) {
		all.removeObjectAt (obj.reflist_index.all);
		for (int i = obj.reflist_index.all; i < all.count(); ++i) {
			// every entry after this one had their index decreased by 1, so correct their reflist indexes
			all.getObjectAt(i).reflist_index.all--;
		}
		
		interactive[(int)obj.properties.interactive].removeObjectAt (obj.reflist_index.interactive);
		for (int i = obj.reflist_index.interactive; i < interactive[(int)obj.properties.interactive].count(); ++i) {
			// every entry after this one had their index decreased by 1, so correct their reflist indexes
			interactive[(int)obj.properties.interactive].getObjectAt(i).reflist_index.interactive--;
		}
		
		movable[(int)obj.properties.movable].removeObjectAt (obj.reflist_index.movable);
		for (int i = obj.reflist_index.movable; i < movable[(int)obj.properties.movable].count(); ++i) {
			// every entry after this one had their index decreased by 1, so correct their reflist indexes
			movable[(int)obj.properties.movable].getObjectAt(i).reflist_index.movable--;
		}
		
		environmental[(int)obj.properties.environmental].removeObjectAt (obj.reflist_index.environmental);
		for (int i = obj.reflist_index.environmental; i < environmental[(int)obj.properties.environmental].count(); ++i) {
			// every entry after this one had their index decreased by 1, so correct their reflist indexes
			environmental[(int)obj.properties.environmental].getObjectAt(i).reflist_index.environmental--;
		}
	}
	public bool objectIsContained(GStructure obj) {
		for (int i = 0; i < all.count(); ++i) {
			if(all.getObjectAt(i) == obj) {
				return true;
			}
		}
		return false;
	}
	public bool isEmpty() {
		if (all.count () > 0) {
			return false;
		}
		return true;
	}
}

/*
 *  Class: GActor_RefList_Index
 * 
 *  -Data structure used to hold the indexes a specific GActor possesses in the GObject_Sector_RefList which contains it.
 *  -It is a key for finding an object (the GActor) within its container (the sectors GObject_Sector_RefList).
 */
public class GActor_RefList_Index {
	public int all;
	public int environmental;
	public int NPCs;
}
	
/*
 *  Class: GObject_Actors_RefList
 * 
 *  Data structure which holds the references for all GActors contained by a sectors GObject_Sector_RefList.
 *  Also classifies the contained GActors by storing them in different pairs of RefList_DataStructures, each
 *  pair representing a certain condition (environmental, NPC...) and each RefList_DataStructure inside the pair 
 *  representing the fulfillment (or not) of that condition.
 * 
 * 	For example, environmental[0] contains references for environmental GActors, while environmental[1] contains
 *  references for non-interactive GActors.
 *  
 *  Note that BOOL_YN.YES = 0 and BOOL_YN.NO = 1. This way the RefList_DataStructure containing evironmental or
 *  non-environmental GActors within this GObject_Actors_RefList can be accessed with environmental[BOOL_YN.YES]
 *  and environmental[BOOL_YN.NO]. This way we have a more intuitive way of accessing the appropriate RefList_DataStructure 
 *  inside a pair. 
 */
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
		
		obj.reflist_index.environmental = environmental[(int)obj.properties.environmental].count();
		environmental [(int)obj.properties.environmental].addObject (obj);
		
		obj.reflist_index.NPCs = NPCs[(int)obj.properties.npc].count();
		NPCs [(int)obj.properties.npc].addObject (obj);
	}
	public void removeObject(GActor obj) {
		all.removeObjectAt (obj.reflist_index.all);
		for (int i = obj.reflist_index.all; i < all.count(); ++i) {
			// every entry after this one had their index decreased by 1, so correct their reflist indexes
			all.getObjectAt(i).reflist_index.all--;
		}
		
		environmental[(int)obj.properties.environmental].removeObjectAt (obj.reflist_index.environmental);
		for (int i = obj.reflist_index.environmental; i < environmental[(int)obj.properties.environmental].count(); ++i) {
			// every entry after this one had their index decreased by 1, so correct their reflist indexes
			environmental[(int)obj.properties.environmental].getObjectAt(i).reflist_index.environmental--;
		}
		
		NPCs[(int)obj.properties.npc].removeObjectAt (obj.reflist_index.NPCs);
		for (int i = obj.reflist_index.NPCs; i < NPCs[(int)obj.properties.npc].count(); ++i) {
			// every entry after this one had their index decreased by 1, so correct their reflist indexes
			NPCs[(int)obj.properties.npc].getObjectAt(i).reflist_index.NPCs--;
		}
	}
	public bool objectIsContained(GActor obj) {
		for (int i = 0; i < all.count(); ++i) {
			if(all.getObjectAt(i) == obj) {
				return true;
			}
		}
		return false;
	}
	public bool isEmpty() {
		if (all.count () > 0) {
			return false;
		}
		return true;
	}
}

	/*
 *  Class: GItem_RefList_Index
 * 
 *  -Data structure used to hold the indexes a specific GItem possesses in the GObject_Tile_RefList which contains it.
 *  -It is a key for finding an object (the GItem) within its container (the tiles GObject_Tile_RefList).
 */
public class GItem_RefList_Index {
	public int all;
}
	
	/*
 *  Class: GObject_Items_RefList
 * 
 *  Data structure which holds the references for all GItems contained by a tiles GObject_Tile_RefList.
 */
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
			// every entry after this one had their index decreased by 1, so correct their reflist indexes
			all.getObjectAt(i).reflist_index.all--;
		}
	}
	public bool objectIsContained(GItem obj) {
		for (int i = 0; i < all.count(); ++i) {
			if(all.getObjectAt(i) == obj) {
				return true;
			}
		}
		return false;
	}
	public bool isEmpty() {
		if (all.count () > 0) {
			return false;
		}
		return true;
	}
}
	
/*
 * End of GObject_RefLists
 */
