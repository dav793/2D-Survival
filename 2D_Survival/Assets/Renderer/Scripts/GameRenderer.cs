using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameRenderer : MonoBehaviour {

	public static GameRenderer GRenderer;
	public TerrainRenderer rTerrain;

	private RendererObjectUpdateQueues ObjectUpdateQueues;
	private RendererTerrainUpdateQueues TerrainUpdateQueues;
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
		updateQueues ();
	}

	public void Init() {
		initUpdateQueues ();
		rTerrain.Init ();
	}

	/*
	 * Add operation on terrain sector to a render update queue
	 */
	public void ScheduleTerrainUpdate(RenderTerrainUpdateOperations operation, GameObject obj) {
		TerrainUpdateQueues.getOperationQueue (operation).Enqueue(obj);
	}

	/*
	 * Add operation on object to a render update queue
	 */
	public void ScheduleObjectUpdate(RenderObjectUpdateOperations operation, GameObject obj) {
		ObjectUpdateQueues.getOperationQueue (operation).Enqueue(obj);
	}

	private void initUpdateQueues() {
		ObjectUpdateQueues = new RendererObjectUpdateQueues ();
		TerrainUpdateQueues = new RendererTerrainUpdateQueues ();
	}

	private void updateQueues() {
		updateTerrainQueues ();
		updateObjectQueues ();
	}

	private void updateTerrainQueues() {
		Queue<GameObject> queue;

		queue = TerrainUpdateQueues.create;
		while (queue.Count > 0) {
			GameObject obj = queue.Dequeue ();
			updateTerrainCreate (obj);
		}

		queue = TerrainUpdateQueues.destroy;
		while (queue.Count > 0) {
			GameObject obj = queue.Dequeue ();
			updateTerrainDestroy (obj);
		}

		queue = TerrainUpdateQueues.update_brightness;
		while (queue.Count > 0) {
			GameObject obj = queue.Dequeue ();
			updateTerrainBrightness (obj);
		}

		queue = TerrainUpdateQueues.update_tint;
		while (queue.Count > 0) {
			GameObject obj = queue.Dequeue ();
			updateTerrainTint (obj);
		}

	}

	private void updateObjectQueues() {
		Queue<GameObject> queue;

		queue = ObjectUpdateQueues.create;
		while (queue.Count > 0) {
			GameObject obj = queue.Dequeue ();
			updateObjectCreate (obj);
		}
		
		queue = ObjectUpdateQueues.destroy;
		while (queue.Count > 0) {
			GameObject obj = queue.Dequeue ();
			updateObjectDestroy (obj);
		}

		queue = ObjectUpdateQueues.update_position;
		while (queue.Count > 0) {
			GameObject obj = queue.Dequeue ();
			updateObjectPosition (obj);
		}

		queue = ObjectUpdateQueues.update_brightness;
		while (queue.Count > 0) {
			GameObject obj = queue.Dequeue ();
			updateObjectBrightness (obj);
		}
		
		queue = ObjectUpdateQueues.update_tint;
		while (queue.Count > 0) {
			GameObject obj = queue.Dequeue ();
			updateObjectTint (obj);
		}

		queue = ObjectUpdateQueues.update_transparency;
		while (queue.Count > 0) {
			GameObject obj = queue.Dequeue ();
			updateObjectTransparency (obj);
		}

		queue = ObjectUpdateQueues.update_animation;
		while (queue.Count > 0) {
			GameObject obj = queue.Dequeue ();
			updateObjectAnimation (obj);
		}

	}


	/*
	 *	RENDER UPDATE OPERATIONS 
	 */

	private void updateObjectCreate(GameObject obj) {

	}

	private void updateObjectDestroy(GameObject obj) {

	}

	private void updateObjectPosition(GameObject obj) {

	}

	private void updateObjectBrightness(GameObject obj) {

	}

	private void updateObjectTint(GameObject obj) {

	}

	private void updateObjectTransparency(GameObject obj) {

	}

	private void updateObjectAnimation(GameObject obj) {

	}


	private void updateTerrainCreate(GameObject obj) {
		
	}
	
	private void updateTerrainDestroy(GameObject obj) {
		
	}

	private void updateTerrainBrightness(GameObject obj) {
		
	}
	
	private void updateTerrainTint(GameObject obj) {
		
	}

	/*
	 *	END OF RENDER UPDATE OPERATIONS 
	 */

}
