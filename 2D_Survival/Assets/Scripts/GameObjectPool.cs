using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameObjectPool {

	private Stack<GameObject> pool;
	private GameObject reference_object;
	private int size;

	public GameObjectPool(GameObject reference_object, int pool_size, GameObject parent_obj) {
		size = pool_size;
		this.reference_object = reference_object;
		initialize (parent_obj);
	}

	public GameObject pop() {
		if (pool.Count > 0) {
			GameObject obj = pool.Pop();
			obj.SetActive(true);
			return obj;
		}
		return null;
	}
	
	public void push(GameObject obj) {
		obj.SetActive(false);
		pool.Push (obj);
	}

	public int getCount() {
		return pool.Count;
	}

	private void initialize(GameObject parent_obj) {
		pool = new Stack<GameObject> ();
		for (int i = 0; i < size; ++i) {
			GameObject obj = GameObject.Instantiate(reference_object) as GameObject;
			obj.transform.parent = parent_obj.transform;
			obj.SetActive(false);
			pool.Push(obj);
		}
	}

}
