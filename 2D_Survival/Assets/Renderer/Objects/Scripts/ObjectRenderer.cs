using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	GameRenderer class:
 *		Subcomponent of GameRenderer, which renders GObjects 
 */
public class ObjectRenderer : MonoBehaviour {

	public GameObject gobject;
	public GameObject gobjectHolder;

	private GameObjectPool objectPool;
	
	public void Init() {
		initObjectPool ();
	}

	public void setupObject(GObject obj) {
		GameObject robj = getNewGameObject ();
		robj.GetComponent<GRObject> ().linked_gobject = obj;
		robj.GetComponent<SpriteRenderer> ().sprite = obj.sprite;
		robj.transform.position = new Vector3 (
			(int)obj.pos_x,
			(int)obj.pos_y,
			GameRenderer.GRenderer.getZUnitsObject(obj.getPosition())
		);
		obj.linkGameObject (robj);
	}
	
	public void discardObject(GObject obj) {
		discardGameObject (obj.renderedGameObject);
		obj.unlinkGameObject ();
	}

	public void updateObjectPosition(GObject obj) {
		obj.renderedGameObject.transform.position = new Vector3(
			(int)obj.pos_x,
			(int)obj.pos_y,
			GameRenderer.GRenderer.getZUnitsObject(obj.getPosition())
		);
	}

	private void initObjectPool() {
		// Destroy any previous objects
		List<GameObject> children = new List<GameObject> ();
		foreach(Transform child in gobjectHolder.transform) {
			children.Add(child.gameObject);
		}
		children.ForEach (child => Destroy(child));
		
		// Initialize GObject pool
		objectPool = new GameObjectPool (gobject, 2048, gobjectHolder);
	}

	private GameObject getNewGameObject() {
		GameObject obj = objectPool.pop ();
		return obj;
	}
	
	private void discardGameObject(GameObject obj) {
		obj.GetComponent<GRObject> ().linked_gobject = null;
		objectPool.push (obj);
	}

}
