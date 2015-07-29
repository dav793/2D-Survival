using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameRenderer : MonoBehaviour {

	public static GameRenderer GRenderer;
	public TerrainRenderer rTerrain;

	private Queue<GameObject>[] UpdateQueues;
	private readonly int queue_count = 7;

	void Awake() {
		if (GRenderer == null) {
			GRenderer = this;
			DontDestroyOnLoad(GRenderer);
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
	 * Add operation on obj to a render update queue
	 */
	public void ScheduleUpdate(RenderUpdateOperations operation, GameObject obj) {
		UpdateQueues [(int)operation].Enqueue (obj);
	}

	private void initUpdateQueues() {
		UpdateQueues = new Queue<GameObject>[queue_count];
		UpdateQueues [(int)RenderUpdateOperations.CREATE] 				= new Queue<GameObject> ();
		UpdateQueues [(int)RenderUpdateOperations.DESTROY] 				= new Queue<GameObject> ();
		UpdateQueues [(int)RenderUpdateOperations.UPDATE_POSITION] 		= new Queue<GameObject> ();
		UpdateQueues [(int)RenderUpdateOperations.UPDATE_BRIGHTNESS] 	= new Queue<GameObject> ();
		UpdateQueues [(int)RenderUpdateOperations.UPDATE_TINT] 			= new Queue<GameObject> ();
		UpdateQueues [(int)RenderUpdateOperations.UPDATE_TRANSPARENCY] 	= new Queue<GameObject> ();
		UpdateQueues [(int)RenderUpdateOperations.UPDATE_ANIMATION] 	= new Queue<GameObject> ();
	}

	private void updateQueues() {
		for (int i = 0; i < queue_count; ++i) {
			while(UpdateQueues[i].Count > 0) {
				GameObject obj = UpdateQueues[i].Dequeue ();
				switch(i) {
					case (int)RenderUpdateOperations.CREATE:
						updateCreate (obj);
						break;
					case (int)RenderUpdateOperations.DESTROY:
						updateDestroy (obj);
						break;
					case (int)RenderUpdateOperations.UPDATE_POSITION:
						updatePosition (obj); 
						break;
					case (int)RenderUpdateOperations.UPDATE_BRIGHTNESS:
						updateBrightness (obj);
						break;
					case (int)RenderUpdateOperations.UPDATE_TINT:
						updateTint (obj); 
						break;
					case (int)RenderUpdateOperations.UPDATE_TRANSPARENCY:
						updateTransparency (obj); 
						break;
					case (int)RenderUpdateOperations.UPDATE_ANIMATION:
						updateAnimation (obj);
						break;
				}
			}
		} 
	}

	private void updateCreate(GameObject obj) {

	}

	private void updateDestroy(GameObject obj) {

	}

	private void updatePosition(GameObject obj) {

	}

	private void updateBrightness(GameObject obj) {

	}

	private void updateTint(GameObject obj) {

	}

	private void updateTransparency(GameObject obj) {

	}

	private void updateAnimation(GameObject obj) {

	}



}
