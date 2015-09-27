using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/*
 *  Class: GRObject
 * 
 *  -Short for Game Renderer Object
 *  -Its a script which is attached to every GObjects <renderedGameObject> in the scene.
 *  -Handles the adding and removing of other components or child GameObjects specific to each GObject type.
 * 
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
			//GameRenderer.GRenderer.rObject.updateObjectPosition (linked_gobject);
			GameRenderer.GRenderer.rObject.updateObjectZ(linked_gobject);
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

	/*
	 * Function: Setup
	 * 
	 * -Sets up the GameObject this script is attached to. That is:
	 * 		-Adds controllers, components or child GameObjects to this GameObject, which are specific to each GObjectType
	 * 		-Initializes added controllers/components.
	 */
	void Setup(GObjectType obj_type) {

		ActorController controller;
		GameObject parts;

		switch(obj_type) {

		case GObjectType.Player:

			// add PlayerController
			controller = linked_gobject.renderedGameObject.AddComponent<PlayerController> ();
			addedComponents.Add(controller);

			// add bodypart structure hierarchy
			parts = Instantiate(GameRenderer.GRenderer.rObject.characterAnimStructure) as GameObject;
			addedGameObjects.Add(parts);

			parts.transform.parent = linked_gobject.renderedGameObject.transform;
			parts.GetComponent<CharBodyController> ().Init(linked_gobject);
			controller.linkActor((GActor)linked_gobject, parts.GetComponent<CharBodyController> ());
			break;

		case GObjectType.Character:

			// add CharacController
			controller = linked_gobject.renderedGameObject.AddComponent<CharacController> ();
			addedComponents.Add(controller);

			// add bodypart structure hierarchy
			parts = Instantiate(GameRenderer.GRenderer.rObject.characterAnimStructure) as GameObject;
			addedGameObjects.Add(parts);
			
			parts.transform.parent = linked_gobject.renderedGameObject.transform;
			parts.GetComponent<CharBodyController> ().Init(linked_gobject);
			controller.linkActor((GActor)linked_gobject, parts.GetComponent<CharBodyController> ());
			break;

		case GObjectType.Animal:

			// add AnimalController
			controller = linked_gobject.renderedGameObject.AddComponent<AnimalController> ();
			addedComponents.Add(controller);

			// add bodypart structure hierarchy
			parts = Instantiate(GameRenderer.GRenderer.rObject.animalAnimStructure) as GameObject;
			addedGameObjects.Add(parts);

			parts.transform.parent = linked_gobject.renderedGameObject.transform;
			parts.GetComponent<AnimalBodyController> ().Init(linked_gobject);
			controller.linkActor((GActor)linked_gobject, parts.GetComponent<AnimalBodyController> ());
			break;

		case GObjectType.Structure:

			// add GeneralObjectController
			StructureController ctrl = linked_gobject.renderedGameObject.AddComponent<StructureController> ();
			addedComponents.Add(ctrl);

			// add sprite renderer
			Component sprite_renderer = linked_gobject.renderedGameObject.AddComponent<SpriteRenderer> ();
			addedComponents.Add(sprite_renderer);

			ctrl.linkGObject(linked_gobject);
			ctrl.Init ();		// sets sprite
			break;

		}

	}

	/*
	 * Function: Reset
	 * 
	 * -Removes all components/gameObjects added to this GameObject
	 */
	void Reset() {

		for (int i = 0; i < addedComponents.Count; ++i) {
			Destroy(addedComponents[i]);
		}
		for (int i = 0; i < addedGameObjects.Count; ++i) {
			Destroy(addedGameObjects[i]);
		}
		
	}

}
