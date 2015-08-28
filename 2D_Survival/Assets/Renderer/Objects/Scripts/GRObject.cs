using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GRObject : MonoBehaviour {

	public GObject linked_gobject;
	private UnityAction CamPositionChange_Listener;
	
	void Awake() {
		CamPositionChange_Listener = new UnityAction (updateZPos);
	}
	
	void OnEnable() {
		//CamPositionChange_Listener = new UnityAction (updateZPos);
		GRenderEventManager.StartListening ("CamPositionChange", CamPositionChange_Listener);
	}
	
	void OnDisable() {
		GRenderEventManager.StopListening ("CamPositionChange", CamPositionChange_Listener);
	}
	
	void updateZPos() {
		GameRenderer.GRenderer.rObject.updateObjectPosition(linked_gobject);
	}

}
