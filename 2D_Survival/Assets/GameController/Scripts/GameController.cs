using UnityEngine;
using System.Collections;


/*
 * 	Hierarchic structure of major game components:
 * 
 * 								 GameController
 * 									   |
 * 					+------------------+------------------+
 * 					|			 		 			 	  |					   	   		 
 * 			 	GameData	 			   	   		GameRenderer		 		  
 * 					 								 	  |					  			 
 * 											 	  +-------+-------+			
 * 											 	  |				  |
 * 	    								   TerrainRenderer	 ObjectRenderer
 * 
 * 
 * 	GameController class:
 * 		Directs progression of game program. Sets up itself and all other parts of the game structure, and directs game tick progression.
 * 		It is intended to be the single most high-level entry point to the application, all other parts branch down from here.
 * 
 */

public class GameController : MonoBehaviour {

	public static GameController GController;
	public static GameData_Settings DataSettings;

	private bool game_started = false;				// this will be set to true once initialization is finished

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
		DataSettings.world_size = 1000;																// in square tiles
		DataSettings.sector_size = 10;																// in square tiles
		DataSettings.world_side_sectors = DataSettings.world_size / DataSettings.sector_size;		// in square sectors
		DataSettings.tile_width = 24;																// in pixels
		DataSettings.within_range_radius = 20;														// in tiles
		DataSettings.render_radius = 20;															// in tiles

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
