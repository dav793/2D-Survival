using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 	Class: GameRenderer
 	Visualizes game elements in the view, managing the construction, destruction and updating of Unity GameObjects on every render tick.
 	It uses subsets of the world representation from the GameData object to visually reflect the state of the data using Unity GameObjects and other visual elements.
 	It works with a set of Update Queues, in which itself and other game components can enqueue or "schedule" rendering operations for game objects.
	All scheduled jobs are performed every render tick until the Update Queues are empty. 
 */

public class GameRenderer : MonoBehaviour {

	public static GameRenderer GRenderer;
	public TerrainRenderer rTerrain;
	public ObjectRenderer rObject;
	public GameCameraController gameCamera;

	private RendererObjectUpdateQueues ObjectUpdateQueues;
	private RendererTerrainUpdateQueues TerrainUpdateQueues;

	private List<WorldSector> renderedSectors;
	//private readonly int queue_count = 7;

	void Awake() {
		if (GRenderer == null) {
			GRenderer = this;
			DontDestroyOnLoad(GRenderer);
		}
		else if (GRenderer != this) {
			Destroy (gameObject);
		}
	}

	public void Tick() {
		renderVisibleSectors ();
		updateQueues ();
	}

	public void Init() {
		renderedSectors = new List<WorldSector> ();
		initUpdateQueues ();
		rTerrain.Init ();
		rObject.Init ();
		gameCamera.Init ();
	}

	private void updateQueues() {
		updateTerrainQueues ();
		updateObjectQueues ();
	}

	private void initUpdateQueues() {
		ObjectUpdateQueues = new RendererObjectUpdateQueues ();
		TerrainUpdateQueues = new RendererTerrainUpdateQueues ();
	}

	/*
	 * Add operation on terrain sector to a render update queue
	 */
	public void ScheduleTerrainUpdate(RenderTerrainUpdateOperations operation, WorldSector sector) {
		TerrainUpdateQueues.getOperationQueue (operation).Enqueue(sector);
	}

	/*
	 * Add operation on object to a render update queue
	 */
	public void ScheduleObjectUpdate(RenderObjectUpdateOperations operation, GObject obj) {
		ObjectUpdateQueues.getOperationQueue (operation).Enqueue(obj);
	}

	/*
	 * Add operation on all objects in sector to a render update queue
	 */
	public void ScheduleUpdateOnSectorObjects(RenderObjectUpdateOperations operation, WorldSector sector) {

		for (int x = sector.lower_boundary_x; x < sector.upper_boundary_x; ++x) {
			for (int y = sector.lower_boundary_y; y < sector.upper_boundary_y; ++y) {
				GTile tile = GameData.GData.getTile(new Vector2(x, y));
				// schedule structures
				for(int i = 0; i < tile.Contained_Objects.structures.all.count(); ++i) {
					ScheduleObjectUpdate(operation, tile.Contained_Objects.structures.all.getObjectAt(i));
				}
				// schedule items
				for(int i = 0; i < tile.Contained_Objects.items.all.count(); ++i) {
					ScheduleObjectUpdate(operation, tile.Contained_Objects.items.all.getObjectAt(i));
				}
			}
		}

		// schedule actors
		for (int i = 0; i < sector.Contained_Objects.actors.all.count(); ++i) {
			ScheduleObjectUpdate(operation, sector.Contained_Objects.actors.all.getObjectAt(i));
		}

	}

	public void ScheduleUpdateOnAllObjects(RenderObjectUpdateOperations operation) {
		for (int i = 0; i < renderedSectors.Count; ++i) {
			ScheduleUpdateOnSectorObjects(operation, renderedSectors[i]);
		}
	}

	public void showSmallHoveringPanelOnAllObjects() {
		for (int i = 0; i < renderedSectors.Count; ++i) {
			for (int x = renderedSectors[i].lower_boundary_x; x < renderedSectors[i].upper_boundary_x; ++x) {
				for (int y = renderedSectors[i].lower_boundary_y; y < renderedSectors[i].upper_boundary_y; ++y) {
					GTile tile = GameData.GData.getTile(new Vector2(x, y));
					for(int k = 0; k < tile.Contained_Objects.structures.all.count(); ++k) {
						UIManager.UI.destroySHP(tile.Contained_Objects.structures.all.getObjectAt(k).renderedGameObject);
						UIManager.UI.createSHP(tile.Contained_Objects.structures.all.getObjectAt(k).renderedGameObject);
					}
					for(int k = 0; k < tile.Contained_Objects.items.all.count(); ++k) {
						UIManager.UI.destroySHP(tile.Contained_Objects.items.all.getObjectAt(k).renderedGameObject);
						UIManager.UI.createSHP(tile.Contained_Objects.items.all.getObjectAt(k).renderedGameObject);
					}
				}
			}

			for (int j = 0; j < renderedSectors[i].Contained_Objects.actors.all.count(); ++j) {
				UIManager.UI.destroySHP(renderedSectors[i].Contained_Objects.actors.all.getObjectAt(j).renderedGameObject);
				UIManager.UI.createSHP(renderedSectors[i].Contained_Objects.actors.all.getObjectAt(j).renderedGameObject);
			}
		}
	}

	public void destroyAllHoveringPanels() {
		for (int i = 0; i < renderedSectors.Count; ++i) {
			for (int x = renderedSectors[i].lower_boundary_x; x < renderedSectors[i].upper_boundary_x; ++x) {
				for (int y = renderedSectors[i].lower_boundary_y; y < renderedSectors[i].upper_boundary_y; ++y) {
					GTile tile = GameData.GData.getTile(new Vector2(x, y));
					for(int k = 0; k < tile.Contained_Objects.structures.all.count(); ++k) {
						UIManager.UI.destroySHP(tile.Contained_Objects.structures.all.getObjectAt(k).renderedGameObject);
					}
					for(int k = 0; k < tile.Contained_Objects.items.all.count(); ++k) {
						UIManager.UI.destroySHP(tile.Contained_Objects.items.all.getObjectAt(k).renderedGameObject);
					}
				}
			}
			
			for (int j = 0; j < renderedSectors[i].Contained_Objects.actors.all.count(); ++j) {
				UIManager.UI.destroySHP(renderedSectors[i].Contained_Objects.actors.all.getObjectAt(j).renderedGameObject);
			}
		}
	}

	public int getZUnitsTerrain() {
		return GameController.PSettings.zunits_per_level * (int)DepthLevel.TERRAIN;
	}

	public int getZUnitsObject(Vector2 position) {
		int base_zunits = GameController.PSettings.zunits_per_level * (int)DepthLevel.OBJECTS;
		Vector2 cam_offset = gameCamera.getDistanceFromCamCenter (position);
		if(gameCamera.getCamSize()+100 >= cam_offset.x && gameCamera.getCamSize()+100 >= cam_offset.y) {		// object is within cam range
			return base_zunits + (int)(gameCamera.getCamSize()+100) + gameCamera.getYOffsetFromCamCenter(Mathf.FloorToInt(position.y));
		}
		return base_zunits;
	}

	public int getZUnitsOverlay() {
		return GameController.PSettings.zunits_per_level * (int)DepthLevel.OVERLAY;
	}

	public int getZUnitsCamera() {
		return GameController.PSettings.zunits_per_level * (int)DepthLevel.CAMERA;
	}

	private void renderVisibleSectors() {

		List<WorldSector> visible = GameData.GData.getSectorAreaRender ();

		for (int i = 0; i < visible.Count; ++i) {
			if(!sectorIsContained(visible[i], renderedSectors) && !visible[i].is_rendered) {
				//schedule creation of new sector
				ScheduleTerrainUpdate(RenderTerrainUpdateOperations.CREATE, visible[i]);
				ScheduleUpdateOnSectorObjects(RenderObjectUpdateOperations.CREATE, visible[i]);
			}
			else {
				//schedule update of sectors

			}
		}

		for (int i = 0; i < renderedSectors.Count; ++i) {
			if(!sectorIsContained(renderedSectors[i], visible) && renderedSectors[i].is_rendered) {
				//schedule destruction of old sector
				ScheduleTerrainUpdate(RenderTerrainUpdateOperations.DESTROY, renderedSectors[i]);
				ScheduleUpdateOnSectorObjects(RenderObjectUpdateOperations.DESTROY, renderedSectors[i]);
			}
		}

		renderedSectors = visible;

	}

	private void updateTerrainQueues() {
		Queue<WorldSector> queue;

		queue = TerrainUpdateQueues.create;
		while (queue.Count > 0) {
			updateTerrainCreate (queue.Dequeue ());
		}

		queue = TerrainUpdateQueues.destroy;
		while (queue.Count > 0) {
			updateTerrainDestroy (queue.Dequeue ());
		}

		queue = TerrainUpdateQueues.update_brightness;
		while (queue.Count > 0) {
			updateTerrainBrightness (queue.Dequeue ());
		}

		queue = TerrainUpdateQueues.update_tint;
		while (queue.Count > 0) {
			updateTerrainTint (queue.Dequeue ());
		}

	}

	private void updateObjectQueues() {
		Queue<GObject> queue;

		queue = ObjectUpdateQueues.create;
		while (queue.Count > 0) {
			updateObjectCreate (queue.Dequeue ());
		}
		
		queue = ObjectUpdateQueues.destroy;
		while (queue.Count > 0) {
			updateObjectDestroy (queue.Dequeue ());
		}

		queue = ObjectUpdateQueues.update_position;
		while (queue.Count > 0) {
			updateObjectPosition (queue.Dequeue ());
		}

		queue = ObjectUpdateQueues.update_brightness;
		while (queue.Count > 0) {
			updateObjectBrightness (queue.Dequeue ());
		}
		
		queue = ObjectUpdateQueues.update_tint;
		while (queue.Count > 0) {
			updateObjectTint (queue.Dequeue ());
		}

		queue = ObjectUpdateQueues.update_transparency;
		while (queue.Count > 0) {
			updateObjectTransparency (queue.Dequeue ());
		}

		queue = ObjectUpdateQueues.update_animation;
		while (queue.Count > 0) {
			updateObjectAnimation (queue.Dequeue ());
		}

	}


	/*
	 *	RENDER UPDATE OPERATIONS 
	 */

	private void updateObjectCreate(GObject obj) {
		if (!obj.is_rendered) {
			rObject.setupObject (obj);
		}
	}

	private void updateObjectDestroy(GObject obj) {
		if (obj.is_rendered) {
			rObject.discardObject (obj);
		}
	}

	private void updateObjectPosition(GObject obj) {
		if (obj.is_rendered) {
			rObject.updateObjectPosition (obj);
		}
	}

	private void updateObjectBrightness(GObject obj) {

	}

	private void updateObjectTint(GObject obj) {

	}

	private void updateObjectTransparency(GObject obj) {

	}

	private void updateObjectAnimation(GObject obj) {

	}


	private void updateTerrainCreate(WorldSector sector) {
		if (!sector.is_rendered) {
			rTerrain.setupSector (sector);
		}
	}
	
	private void updateTerrainDestroy(WorldSector sector) {
		if (sector.is_rendered) {
			rTerrain.discardSector (sector);
		}
	}

	private void updateTerrainBrightness(WorldSector sector) {
		
	}
	
	private void updateTerrainTint(WorldSector sector) {
		
	}
	/*
	 *	END OF RENDER UPDATE OPERATIONS 
	 */

	public bool sectorIsContained(WorldSector needle, List<WorldSector> haystack) {
		for (int i = 0; i < haystack.Count; ++i) {
			if(needle == haystack[i]) {
				return true;
			}
		}
		return false;
	}

	/******** FOR TESTING ********/
	public string renderedSectorsToString() {
		string str = "";
		for (int i = 0; i < renderedSectors.Count; ++i) {
			str = str + "(" + renderedSectors[i].indexToString() + ") ";
		}
		return str;
	}
}
