using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController GController;
	
	void Awake() {
		if (GController == null) {
			GController = this;
			DontDestroyOnLoad (GController);
		} 
		else if (GController != this) {
			Destroy(gameObject);
		}
	}

	// Use this for initialization
	void Start () {

		GameData_Settings data_conf = new GameData_Settings ();
		data_conf.world_size_x = 10;
		data_conf.world_size_y = 10;

		GameData.GData.Init (data_conf);
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
