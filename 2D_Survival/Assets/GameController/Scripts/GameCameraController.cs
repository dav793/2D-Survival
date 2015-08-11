using UnityEngine;
using System.Collections;

public class GameCameraController : MonoBehaviour {

	public GameObject target;

	// Use this for initialization
	void Start () {
		adjustCamSize ();
	}
	
	// Update is called once per frame
	void Update () {

		float speed = 1;
		if(Input.GetKey(KeyCode.LeftShift)) {
			speed = 4;
		}
		if(Input.GetKey(KeyCode.W)) {
			moveTargetTemp(Vector2.up*speed);
		}
		if(Input.GetKey(KeyCode.W)) {
			moveTargetTemp(Vector2.up*speed);
		}
		if(Input.GetKey(KeyCode.A)) {
			moveTargetTemp(Vector2.right*-1*speed);
		}
		if(Input.GetKey(KeyCode.S)) {
			moveTargetTemp(Vector2.up*-1*speed);
		}
		if(Input.GetKey(KeyCode.D)) {
			moveTargetTemp(Vector2.right*speed);
		}

		transform.position = new Vector3 (GameData.FocusPoint.x, GameData.FocusPoint.y, transform.position.z);

		if (Input.GetKeyDown (KeyCode.P)) {
			adjustCamSize();
		}

	}

	private void adjustCamSize() {
		Debug.Log ("Adjusting camera size to screen height: " + Screen.height);
		gameObject.GetComponent<Camera> ().orthographicSize = Screen.height / 4;
	}

	private void moveTargetTemp(Vector3 direction) {
		target.transform.position += direction;
		GameData.FocusPoint = new Vector2(target.transform.position.x, target.transform.position.y);
	}

}
