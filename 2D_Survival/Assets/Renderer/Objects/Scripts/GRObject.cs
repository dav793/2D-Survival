using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class GRObject : MonoBehaviour {

	public GObject linked_gobject;
	public SmallHoveringPanel assoc_shp;
	private UnityAction CamPositionChange_Listener;
	
	void Awake() {
		CamPositionChange_Listener = new UnityAction (updateZPos);
	}
	
	void updateZPos() {
		if (linked_gobject != null) {
			GameRenderer.GRenderer.rObject.updateObjectPosition (linked_gobject);
		}
	}

	public void Activate() {
		GRenderEventManager.StartListening ("CamPositionChange", CamPositionChange_Listener);
	}

	public void Deactivate() {
		GRenderEventManager.StopListening ("CamPositionChange", CamPositionChange_Listener);
		UIManager.UI.destroySHP (gameObject);
		linked_gobject = null;
		assoc_shp = null;
	}

}
