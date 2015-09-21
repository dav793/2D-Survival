using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/*
 * Short for Game Renderer Object
 */

public class GRObject : MonoBehaviour {

	public GObject linked_gobject;
	public SmallHoveringPanel assoc_shp;		//DELETE LATER

	List<Component> addedComponents;			// holds all components added by the ObjectRenderer to this gameObject
	List<GameObject> addedGameObjects;			// holds all GameObjects added by the ObjectRenderer as children of this gameObject

	UnityAction CamPositionChange_Listener;
	
	void Awake() {
		CamPositionChange_Listener = new UnityAction (updateZPos);
	}
	
	void updateZPos() {
		if (linked_gobject != null) {
			GameRenderer.GRenderer.rObject.updateObjectPosition (linked_gobject);
		}
	}

	void Init(GObject obj) {
		linked_gobject = obj;
		addedComponents = new List<Component> ();
		addedGameObjects = new List<GameObject> ();
		if (obj.type != null) {
			Setup (obj.type);
		} 
		else {
			Debug.LogError("GObject type is undefined.");
		}
	}

	void StartListeners() {
		GRenderEventManager.StartListening ("CamPositionChange", CamPositionChange_Listener);
	}

	void StopListeners() {
		GRenderEventManager.StopListening ("CamPositionChange", CamPositionChange_Listener);
	}

	public void Activate(GObject obj) {
		Init (obj);
		StartListeners ();
	}

	public void Deactivate() {
		StopListeners ();
		Reset ();
		UIManager.UI.destroySHP (gameObject);
		linked_gobject = null;
		assoc_shp = null;
	}

	public ActorController getActorControllerIfExists() {
		for (int i = 0; i < addedComponents.Count; ++i) {
			ActorController act = addedComponents[i] as ActorController;
			if(act != null) {
				return act;
			}
		}
		return null;
	}

	void Setup(GObjectType obj_type) {

		ActorController controller;
		GameObject parts;

		switch(obj_type) {

		case GObjectType.Player:

			controller = linked_gobject.renderedGameObject.AddComponent<PlayerController> ();
			addedComponents.Add(controller);
			parts = Instantiate(GameRenderer.GRenderer.rObject.characterAnimStructure) as GameObject;
			addedGameObjects.Add(parts);

			parts.transform.parent = linked_gobject.renderedGameObject.transform;
			parts.GetComponent<CharBodyController> ().Init(linked_gobject);
			controller.linkActor((GActor)linked_gobject, parts.GetComponent<CharBodyController> ());
			break;

		case GObjectType.Character:

			controller = linked_gobject.renderedGameObject.AddComponent<CharacController> ();
			addedComponents.Add(controller);
			parts = Instantiate(GameRenderer.GRenderer.rObject.characterAnimStructure) as GameObject;
			addedGameObjects.Add(parts);
			
			parts.transform.parent = linked_gobject.renderedGameObject.transform;
			parts.GetComponent<CharBodyController> ().Init(linked_gobject);
			controller.linkActor((GActor)linked_gobject, parts.GetComponent<CharBodyController> ());
			break;

		case GObjectType.Animal:

			controller = linked_gobject.renderedGameObject.AddComponent<AnimalController> ();
			addedComponents.Add(controller);
			parts = Instantiate(GameRenderer.GRenderer.rObject.animalAnimStructure) as GameObject;
			addedGameObjects.Add(parts);

			parts.transform.parent = linked_gobject.renderedGameObject.transform;
			parts.GetComponent<AnimalBodyController> ().Init(linked_gobject);
			controller.linkActor((GActor)linked_gobject, parts.GetComponent<AnimalBodyController> ());
			break;

		case GObjectType.Structure:

			GeneralGObjectController ctrl = linked_gobject.renderedGameObject.AddComponent<GeneralGObjectController> ();
			addedComponents.Add(ctrl);

			Component sprite_renderer = linked_gobject.renderedGameObject.AddComponent<SpriteRenderer> ();
			addedComponents.Add(sprite_renderer);

			ctrl.linkGObject(linked_gobject);
			ctrl.Init ();
			break;

		}

	}

	void Reset() {

		for (int i = 0; i < addedComponents.Count; ++i) {
			Destroy(addedComponents[i]);
		}
		for (int i = 0; i < addedGameObjects.Count; ++i) {
			Destroy(addedGameObjects[i]);
		}
		
	}

}
