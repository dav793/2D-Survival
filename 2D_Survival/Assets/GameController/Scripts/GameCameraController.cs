using UnityEngine;
using System.Collections;

public class GameCameraController : MonoBehaviour {
	
	public GameObject target;
	public Camera cam;

	int zoom_level = 1;
	int camDepth = -5000;
	Vector3 last_position = Vector2.zero;

	float smoothTime = 0.3f;
	Vector3 velocity = Vector3.zero;

	public void Init() {
		camDepth = GameRenderer.GRenderer.getZUnitsCamera ();
		cam = GetComponent<Camera> ();
		cam.farClipPlane = Mathf.Abs(camDepth) + 100;
	}

	public Vector2 getDistanceFromCamCenter(Vector2 world_point) {
		return new Vector2 (
			Mathf.Abs(transform.position.x - world_point.x),
			Mathf.Abs(transform.position.y - world_point.y)
		);
	}

	public int getYOffsetFromCamCenter(int world_y) {
		return world_y - (int)transform.position.y;
	}

	public float getCamSize() {
		return cam.orthographicSize;
	}

	// Use this for initialization
	void Start () {
		adjustCamSize ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		/*float speed = 1f;
		if(Input.GetKey(KeyCode.LeftShift)) {
			speed = 2f;
		}
		if(Input.GetKey(KeyCode.UpArrow)) {
			moveTargetTemp(Vector2.up*speed);
		}
		if(Input.GetKey(KeyCode.LeftArrow)) {
			moveTargetTemp(Vector2.right*-1*speed);
		}
		if(Input.GetKey(KeyCode.DownArrow)) {
			moveTargetTemp(Vector2.up*-1*speed);
		}
		if(Input.GetKey(KeyCode.RightArrow)) {
			moveTargetTemp(Vector2.right*speed);
		}*/

		Vector3 targetPosition = new Vector3 (GameData.FocusPoint.x, GameData.FocusPoint.y, camDepth);
		transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
		//transform.position = new Vector3 (GameData.FocusPoint.x, GameData.FocusPoint.y, camDepth);

		if(last_position != transform.position) {		// camera position changed
			GRenderEventManager.TriggerEvent("CamPositionChange");
			last_position = transform.position;
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			adjustCamSize();
		}

		if(Input.GetKeyDown(KeyCode.LeftControl)) {
			if(zoom_level > 1) {
				zoom_level = 0;
			}
			else {
				zoom_level++;
			}
			adjustCamSize(zoom_level);
		}

	}

	private void adjustCamSize() {
		Debug.Log ("Adjusting camera size to screen height: " + Screen.height);
		gameObject.GetComponent<Camera> ().orthographicSize = Screen.height / 4;
		GRenderEventManager.TriggerEvent("CamPositionChange");
		//GameRenderer.GRenderer.ScheduleUpdateOnAllObjects(RenderObjectUpdateOperations.UPDATE_POSITION);
	}

	private void adjustCamSize(int zoom) {
		int zoom_coeficient = 0;
		switch (zoom) {
		case 0:
			zoom_coeficient = 2;
			break;
		case 1:
			zoom_coeficient = 4;
			break;
		case 2: 
			zoom_coeficient = 8;
			break;
		}
		gameObject.GetComponent<Camera> ().orthographicSize = Screen.height / zoom_coeficient;
		GRenderEventManager.TriggerEvent("CamPositionChange");
	}

}
