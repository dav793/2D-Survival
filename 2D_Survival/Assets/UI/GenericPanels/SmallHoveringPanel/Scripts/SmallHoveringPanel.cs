using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SmallHoveringPanel : MonoBehaviour {

	public Text textarea;
	public List<string> messages;
	GameObject anchor;
	UIManager superComponent;

	public void Init(GameObject obj, UIManager manager) {
		messages = new List<string> ();
		anchor = obj;
		superComponent = manager;
	}

	public void fillTextWithMessages() {
		textarea.text = "";
		for (int i = 0; i < messages.Count; ++i) {
			textarea.text = textarea.text + messages[i] + "\n";
		}
	}

	void FixedUpdate() {
		placeOverAnchor ();
		updateText ();
	}

	void placeOverAnchor() {
		if (anchor != null) {
			transform.position = superComponent.GetScreenPosition(anchor.transform, GameCameraController.GCamControl.cam);
			transform.position = new Vector3 (transform.position.x, transform.position.y+80, transform.position.z);
		}
	}

	void updateText() {
		GObject gob = anchor.GetComponent<GRObject> ().linked_gobject;
		messages.Clear ();
		messages.Add("Position: "+gob.pos_x+", "+gob.pos_y);
		messages.Add ("Tile: " + GameData.GData.getTileFromWorldPoint (new Vector2 (gob.pos_x, gob.pos_y)).indexToString ());
		if (gob.isActor ()) {
			messages.Add ("Sector: " + ((GActor)gob).sector.indexToString ());
		} else {
			messages.Add ("Sector: " +  GameData.GData.getSectorFromWorldPoint(new Vector2 (gob.pos_x, gob.pos_y)).indexToString ());
		}
		fillTextWithMessages();
	}

}
