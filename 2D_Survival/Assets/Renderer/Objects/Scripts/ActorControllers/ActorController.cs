using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ActorController : MonoBehaviour {

	public CardinalDirections face_direction;
	public List <string> active_actions;
	public Dictionary <string, Dictionary <string, int>> bodypart_anim_priorities;
	public Animator anim;

	Vector2 last_position;
	int stall = 0;
	GActor actor;
	ObjectAnimController animController;

	public virtual void execTick() {
		checkPosUpdate ();
		updateAnimClips ();
	}

	void FixedUpdate () {
		execTick ();
	}

	void OnEnable() {
		Init ();
	}

	public void Init() {
		anim = GetComponent<Animator> ();
		ResetScale ();
		last_position = new Vector2(transform.position.x, transform.position.y);
		if (active_actions == null) {
			active_actions = new List<string> ();
		}
		if (bodypart_anim_priorities == null) {
			bodypart_anim_priorities = new Dictionary<string, Dictionary <string, int>> ();
		}
		startAction ("idle");
	}

	public virtual Dictionary <string, Dictionary <string, int>> getBodypartAnimPriorities() {
		return bodypart_anim_priorities;
	}

	public List<string> getActiveActionList() {
		List<string> result = new List<string> ();
		foreach (string entry in active_actions) {
			result.Add(entry);
		}
		return result;
	}

	public int? getBodypartAnimPriority(string bodypart_name, string animation_name) {
		Dictionary <string, Dictionary <string, int>> search_dictionary = getBodypartAnimPriorities ();
		if(search_dictionary.ContainsKey(animation_name)) {
			Dictionary<string, int> bodyparts = null;
			search_dictionary.TryGetValue(animation_name, out bodyparts);
			if(bodyparts != null) {
				int priority;
				bodyparts.TryGetValue(bodypart_name, out priority);
				return priority;
			}
		}
		return null;
	}

	public string getHighestPriorityActionOnBodypart(string bodypart_name) {
		int? highest_priority = -1;
		string selected_action = null;
		List<string> actions = getActiveActionList ();
		for (int i = 0; i < actions.Count; ++i) {
			int? priority = getBodypartAnimPriority(bodypart_name, actions[i]);
			if(priority != null && priority > highest_priority) {
				highest_priority = priority;
				selected_action = actions[i];
			}
		}
		return selected_action;
	}

	/*
	 * Tell this actor to attempt to move by movement_vector.
	 * Used by PlayerController to move player in respond to key presses.
	 */
	public void tryToMove(Vector2 movement_vector) {
		if (actor != null) {
			actor.moveBy(movement_vector);	
		}
	}
	
	public float getMaxSpeed() {
		if (actor != null) {
			return actor.max_speed;
		}
		return 0f;
	}
	
	public void linkActor(GActor act) {
		actor = act;
	}

	public void linkActor(GActor act, ObjectAnimController anim_controller) {
		actor = act;
		animController = anim_controller;
	}

	void FlipHScale() {
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void FlipHScale(CardinalDirections direction) {
		if(direction == CardinalDirections.E) {
			transform.localScale = new Vector3 (1, 1, 1);
		}
		else if (direction == CardinalDirections.W) {
			transform.localScale = new Vector3 (-1, 1, 1);
		}
	}
	
	void ResetScale() {
		transform.localScale = new Vector3 (1, 1, 1);
	}
	
	void checkPosUpdate() {
		Vector2 vec = new Vector2(transform.position.x, transform.position.y) - last_position;
		if (vec != Vector2.zero) {		// if it has moved
			stall = 4;
			// set movement direction
			if(vec.y != 0) {
				if(vec.y < 0) {
					face_direction = CardinalDirections.S;
				}
				else if(vec.y > 0) {
					face_direction = CardinalDirections.N;
				}
			}
			if(vec.x != 0) {
				if(vec.x > 0) {
					if(face_direction != CardinalDirections.E) {
						FlipHScale (CardinalDirections.E);
					}
					face_direction = CardinalDirections.E;
				}
				else if(vec.x < 0) {
					if(face_direction != CardinalDirections.W) {
						FlipHScale (CardinalDirections.W);
					}
					face_direction = CardinalDirections.W;
				}
			}
			startAction("running");
			last_position = transform.position;
		} 
		else {
			stall--;
			if(stall <= 0) {
				stopAction("running");
			}
		}
	}

	void startAction(string action) {
		if(!StringIsInList(action, active_actions)) {
			active_actions.Add(action);
			//updateAnimClips();
		}
	}

	void stopAction(string action) {
		if(StringIsInList(action, active_actions)) {
			active_actions.Remove(action);
			//updateAnimClips();
		}
	}

	void changeFaceDirection(CardinalDirections new_direction) {
		if (face_direction != new_direction) {
			face_direction = new_direction;
			//updateAnimClips();
		}
	}

	bool StringIsInList(string str, List<string> list) {
		for(int i = 0; i < list.Count; ++i) {
			if(list[i] == str) {
				return true;
			}
		}
		return false;
	}

	void updateAnimClips() {
		if(animController != null) {
			animController.updateClips();
		}
	}

	// FOR TESTS
	public virtual void printDebug() {
		string output = "Direction: ";
		output = output + face_direction + "\n" + "Active actions: " + activeActionsToString() + "\n";
		UIManager.UI.DebugConsole1.transform.Find("Text").GetComponent<Text> ().text = output;
	}

	public string activeActionsToString() {
		string res = "";
		foreach (string entry in active_actions) {
			res = res + entry + ", ";
		}  
		return res;
	}
	// TESTS END

}
