using UnityEngine;
using System.Collections;


/*
  	Class: GameController
  	Directs progression of game program. Sets up itself and all other parts of the game structure, and directs game tick progression.
  	It is intended to be the single highest-level entry point to the application, all other parts branch down from here.

*/

public class GameController : MonoBehaviour {

	public static GameController GController;
	public static Prog_Settings PSettings;
	public static GameData_Settings DataSettings;
	public static Renderer_Settings RendererSettings;

	private bool game_started = false;				// this will be set to true once initialization is finished
	SmallHoveringPanel temp_shp;

	int final_frameskip = 0;
	int render_frameskip;

	void Awake() {
		if (GController == null) {
			GController = this;
			DontDestroyOnLoad (GController);
		} 
		else if (GController != this) {
			Destroy(gameObject);
		}
	}
	
	void Start () {

		DataSettings = new GameData_Settings ();			// 	< look for type definition in Types.cs
		DataSettings.world_size = 1000;																// in square tiles
		DataSettings.sector_size = 10;																// in square tiles
		DataSettings.world_side_sectors = DataSettings.world_size / DataSettings.sector_size;		// in square sectors
		DataSettings.tile_width = 32;																// in pixels
		DataSettings.within_range_radius = 20;														// in tiles
		DataSettings.render_radius = 20;															// in tiles

		RendererSettings = new Renderer_Settings ();
		RendererSettings.terrain_pool_size = 4084;
		RendererSettings.gobject_pool_size = 4084;

		PSettings = new Prog_Settings ();
		PSettings.zunits_per_level = -3000;

		render_frameskip = final_frameskip;

		GameData.GData.Init ();
		GameRenderer.GRenderer.Init ();
		UIManager.UI.Init ();

		game_started = true;
	
	}

	void FixedUpdate () {
		if (game_started) {
			Tick ();

			//TESTS
			if(Input.GetKeyDown(KeyCode.O)) {
				string output = "";
				output += "Active renderer objs: \t" + TestUtils.active_r_objs + "\n";
				output += "Active renderer sectors: \t" + TestUtils.active_r_trns + "\n";
				output += "Sectors within range: \t" + GameData.GData.sectorsWithinRangeToString() + "\n";
				output += "Rendered sectors: \t\t" + GameRenderer.GRenderer.renderedSectorsToString() + "\n";
				UIManager.UI.setInfo(output);
			}

			if(Input.GetKeyDown(KeyCode.P)) {
				UIManager.UI.hideInfo();
			}

			if(Input.GetKeyDown(KeyCode.K)) {
				GameRenderer.GRenderer.showSmallHoveringPanelOnAllObjects();
			}

			if(Input.GetKeyDown(KeyCode.L)) {
				GameRenderer.GRenderer.destroyAllHoveringPanels();
			}

			if(Input.GetKeyDown(KeyCode.M)) {
				TestUtils.debugging = !TestUtils.debugging;
			}
		}
	}

	private void Tick() {
		GameData.GData.Tick ();
		if (render_frameskip > 0) {
			render_frameskip--;
		} else {
			GameRenderer.GRenderer.Tick ();
			render_frameskip = final_frameskip;
		}
	}

}
