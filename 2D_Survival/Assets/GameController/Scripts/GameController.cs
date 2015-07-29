using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController GController;
	
	void Awake() {
		if (GController == null) {
			GController = this;
			DontDestroyOnLoad(GController);
		}
	}

	// Use this for initialization
	void Start () {
		GameData.GData.Init ();
		GameRenderer.GRenderer.Init ();
	}
	
	// Update is called once per frame
	void Update () {
		Tick ();
	}

	private void Tick() {
		GameData.GData.Tick ();
		GameRenderer.GRenderer.Tick ();
	}

}
