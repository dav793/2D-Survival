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

}
