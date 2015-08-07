using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static GameController GController;
	public static GameData_Settings DataSettings;

	private bool game_started = false;

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

		GameData_Settings DataSettings = new GameData_Settings ();			// 	< look for type definition in Types.cs
		DataSettings.world_size = 100;
		DataSettings.sector_size = 10;
		DataSettings.sectors_x = DataSettings.world_size / DataSettings.sector_size;
		DataSettings.sectors_y = DataSettings.world_size / DataSettings.sector_size;
		DataSettings.tile_width = 32;
		DataSettings.within_range_radius = 20;
		DataSettings.render_radius = 10;

		GameData.GData.Init (DataSettings);
		GameRenderer.GRenderer.Init ();

		game_started = true;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (game_started) {
			Tick ();
		}
	}

	private void Tick() {
		GameData.GData.Tick ();
		GameRenderer.GRenderer.Tick ();
	}

}
