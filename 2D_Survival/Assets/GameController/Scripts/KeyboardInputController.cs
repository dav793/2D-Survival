using UnityEngine;
using System.Collections;

public class KeyboardInputController : MonoBehaviour {

	public static KeyboardInputController input;

	public bool UP = false;
	public bool RIGHT = false;
	public bool DOWN = false;
	public bool LEFT = false;

	void Awake() {
		if (input == null) {
			input = this;
		}
		else if (input != this) {
			Destroy(gameObject);
		}
	}

	void Update() {

		if (Input.GetKey (KeyCode.W)) {
			UP = true;
		} 
		else {
			UP = false;
		}

		if (Input.GetKey (KeyCode.A)) {
			LEFT = true;
		} 
		else {
			LEFT = false;
		}

		if (Input.GetKey (KeyCode.S)) {
			DOWN = true;
		} 
		else {
			DOWN = false;
		}

		if (Input.GetKey (KeyCode.D)) {
			RIGHT = true;
		} 
		else {
			RIGHT = false;
		}

	}

}
