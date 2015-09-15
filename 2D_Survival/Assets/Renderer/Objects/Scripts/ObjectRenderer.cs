using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *	GameRenderer class:
 *		Subcomponent of GameRenderer, which renders GObjects in the Unity worldspace.
 */
public class ObjectRenderer : MonoBehaviour {

	public GameObject gobject;
	public GameObject gobjectHolder;

	GameObjectPool objectPool;
	string unassigned_go_name = "Unassigned Object";

	public void Init() {
		initObjectPool ();
	}

	/*
	 *  Function: setupObject
	 *  
	 *  Parameters: GObject:<obj>
	 * 
	 * 	Returns: void
	 * 
	 * 	-Obtains a new GameObject from the <objectPool>, which will be used to render <obj> .
	 *  -Initializes the GameObjects attributes and links references between <obj> and the GameObject
	 *  -Sets the GameObjects position. 
	 */
	public void setupObject(GObject obj) {
		GameObject robj = getNewGameObject ();
		obj.linkGameObject (robj);
		robj.name = obj.gameObjectName;
		robj.GetComponent<GRObject> ().linked_gobject = obj;
		robj.GetComponent<SpriteRenderer> ().sprite = obj.sprite;
		robj.GetComponent<Animator> ().runtimeAnimatorController = getAnimationController (obj);
		robj.transform.position = new Vector3 (
			(int)obj.pos_x,
			(int)obj.pos_y,
			GameRenderer.GRenderer.getZUnitsObject(obj.getPosition())
		);

		//TESTS
		TestUtils.active_r_objs++;
	}

	/*
	 *  Function: discardObject
	 *  
	 *  Parameters: GObject:<obj>
	 * 
	 * 	Returns: void
	 * 
	 * 	-Returns <obj>s rendered GameObject to the <objectPool>.
	 *  -Unlinks references between <obj> and the GameObject
	 */
	public void discardObject(GObject obj) {
		discardGameObject (obj.renderedGameObject);
		obj.unlinkGameObject ();
	}

	/*
	 *  Function: updateObjectPosition
	 *  
	 *  Parameters: GObject:<obj>
	 * 
	 * 	Returns: void
	 * 
	 * 	-Sets <obj>s rendered GameObjects position. 
	 */
	public void updateObjectPosition(GObject obj) {
		obj.renderedGameObject.transform.position = new Vector3(
			(int)obj.pos_x,
			(int)obj.pos_y,
			GameRenderer.GRenderer.getZUnitsObject(obj.getPosition())
		);

		/* 	delete 
		OBJ_Player player = obj as OBJ_Player;
		if (player != null) {
			//gobject is a player
			//obj.renderedGameObject.GetComponent<PlayerController> ().updatePosition();
		} 
		*/
	}

	/*
	 *  Function: initObjectPool
	 *  
	 *  Parameters: none
	 * 
	 * 	Returns: void
	 * 
	 * 	Initializes the <objectPool> with new instances of <gobject>, which is the GameObject prefab used to visualize GObjects
	 *  in the worldspace.
	 */
	void initObjectPool() {
		// Destroy any previous objects
		List<GameObject> children = new List<GameObject> ();
		foreach(Transform child in gobjectHolder.transform) {
			children.Add(child.gameObject);
		}
		children.ForEach (child => Destroy(child));
		
		// Initialize GObject pool
		objectPool = new GameObjectPool (gobject, unassigned_go_name, GameController.RendererSettings.gobject_pool_size, gobjectHolder);
	}

	/*
	 *  Function: getNewGameObject
	 *  
	 *  Parameters: none
	 * 
	 * 	Returns: <GameObject>
	 * 
	 * 	Obtains a new GameObject from the <objectPool>
	 */
	GameObject getNewGameObject() {
		GameObject obj = objectPool.pop ();
		return obj;
	}

	/*
	 *  Function: discardGameObject
	 *  
	 *  Parameters: none
	 * 
	 * 	Returns: void
	 * 
	 * 	Deactivates and returns <obj> the <objectPool>
	 */
	void discardGameObject(GameObject obj) {
		obj.GetComponent<GRObject> ().linked_gobject = null;
		obj.GetComponent<SpriteRenderer> ().sprite = null;
		obj.GetComponent<Animator> ().runtimeAnimatorController = null;
		removeAnimationControllers (obj);
		obj.transform.position = new Vector3 (0,0,0);
		objectPool.push (obj);

		//TESTS
		TestUtils.active_r_objs--;
	}

	/*
	 *  Function: getAnimationController
	 *  
	 *  Parameters: GObject:<gobj>, GameObject:<robj>
	 * 
	 * 	Returns: <RuntimeAnimatorController>
	 * 
	 * 	-Finds and returns the AnimatorController corresponding to the type of GObject that <gobj> is.
	 * 	-Also adds the corresponding ActorController component to <gobj>s rendered GameObject
	 *  -Will return null if <gobj> is not animated.
	 */
	RuntimeAnimatorController getAnimationController(GObject gobj) {
		//Check if gobject is an actor
		GActor act = gobj as GActor;
		if (act != null) {
			ActorController controller = null;
			//Check if gobject is a player
			OBJ_Player player = act as OBJ_Player;
			if (player != null) {
				controller = gobj.renderedGameObject.AddComponent<PlayerController> ();
				controller.linkActor(act);
				return AnimationControllers.AnimControllers.player;
			}
			else {
				//Check if gobject is a character
				GCharacter character = act as GCharacter;
				if(character != null) {
					controller = gobj.renderedGameObject.AddComponent<CharacController> ();
					controller.linkActor(act);
					return AnimationControllers.AnimControllers.character;
				}
				else {
					//Check if gobject is an animal
					GAnimal animal = act as GAnimal;
					if(animal != null) {
						controller = gobj.renderedGameObject.AddComponent<AnimalController> ();
						controller.linkActor(act);
						return AnimationControllers.AnimControllers.animal;
					}
					else {
						//Its just a regular actor
						controller = gobj.renderedGameObject.AddComponent<ActorController> ();
						controller.linkActor(act);
						return null;
					}
				}
			}
		} 
		return null;
	}
	/*
	 *  Function: removeAnimationControllers
	 *  
	 *  Parameters: GameObject:<gobj>
	 * 
	 * 	Returns: void
	 * 
	 * 	-Removes all possible controller scripts that may have been added to <gobj>
	 */
	void removeAnimationControllers(GameObject gobj) {
		Destroy(gobj.GetComponent<PlayerController> ());
		Destroy(gobj.GetComponent<CharacController> ());
		Destroy(gobj.GetComponent<AnimalController> ());
		Destroy(gobj.GetComponent<ActorController> ());
	}

}
