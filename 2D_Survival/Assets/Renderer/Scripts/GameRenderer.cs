using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	GameRenderer class:
 *		Visualizes game elements in the view, managing the construction, destruction and updating of Unity GameObjects on every render tick.
 *		It uses subsets of the world representation from the GameData object to visually reflect the state of the data using Unity GameObjects 
 *		and other visual elements.
 *		It works with a set of Update Queues, in which itself and other game components can enqueue or "schedule" rendering operations for game objects.
 *		All the jobs in the Update Queues are performed every render tick. 
 */

public class GameRenderer : MonoBehaviour {

	public static GameRenderer GRenderer;
	public TerrainRenderer rTerrain;
	public ObjectRenderer rObject;

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
		adjustVisibleSectors ();
		updateQueues ();
	}

	public void Init() {
		renderedSectors = new List<WorldSector> ();
		initUpdateQueues ();
		rTerrain.Init ();
		rObject.Init ();
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

	private void adjustVisibleSectors() {

		List<WorldSector> visible = GameData.GData.getSectorAreaRender ();

		for (int i = 0; i < visible.Count; ++i) {
			if(!sectorIsContained(visible[i], renderedSectors) && !visible[i].is_rendered) {
				//schedule creation of new sector
				ScheduleTerrainUpdate(RenderTerrainUpdateOperations.CREATE, visible[i]);
			}
		}

		for (int i = 0; i < renderedSectors.Count; ++i) {
			if(!sectorIsContained(renderedSectors[i], visible) && renderedSectors[i].is_rendered) {
				//schedule destruction of old sector
				ScheduleTerrainUpdate(RenderTerrainUpdateOperations.DESTROY, renderedSectors[i]);
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

	}

	private void updateObjectDestroy(GObject obj) {

	}

	private void updateObjectPosition(GObject obj) {

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
			//Debug.Log ("sector " + sector.index_x + ", " + sector.index_y + " is created.");
		}
	}
	
	private void updateTerrainDestroy(WorldSector sector) {
		if (sector.is_rendered) {
			rTerrain.discardSector (sector);
			//Debug.Log ("sector " + sector.index_x + ", " + sector.index_y + " is destroyed.");
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

}
