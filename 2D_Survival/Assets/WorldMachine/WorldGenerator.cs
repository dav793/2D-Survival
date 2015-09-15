using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator {

	public void generateScenery() {

		int objs = 1000;
		while (objs > 0) {

			Vector2 selected_tile_index = new Vector2(UnityEngine.Random.Range(0, 50), UnityEngine.Random.Range(0, 50));
			GTile selected_tile = GameData.GData.getTile(selected_tile_index);
			if(selected_tile.Contained_Objects.structures.isEmpty()) {

				GObject veg = null;
				int vegType = UnityEngine.Random.Range(0, 100);

				if(vegType >= 0 && vegType < 20) {
					veg = getRandomTree();
				} 

				if(vegType >= 20 && vegType < 100) {
					veg = getRandomBush();
				}

				if(veg != null) {
					veg.setPosition (selected_tile, new Vector2(UnityEngine.Random.Range(-7,7), UnityEngine.Random.Range(-7,7)));
					objs--;
				}

			}

		}

	}

	GObject getRandomBush() {
		GObject veg = null;
		int vegType = UnityEngine.Random.Range(0, 100);
		if(vegType >= 0 && vegType < 20) {
			veg = new OBJ_TropicalBush1();
		} 
		if(vegType >= 20 && vegType < 55) {
			veg = new OBJ_TropicalBush2();
		}
		if(vegType >= 55 && vegType < 100) {
			veg = new OBJ_TropicalBush3();
		}
		return veg;
	}

	GObject getRandomTree() {
		GObject veg = null;
		int vegType = UnityEngine.Random.Range(0, 100);
		if(vegType >= 0 && vegType < 20) {
			veg = new OBJ_TropicalTree2();
		} 
		if(vegType >= 20 && vegType < 70) {
			veg = new OBJ_PalmTree1();
		} 
		if(vegType >= 70 && vegType < 100) {
			veg = new OBJ_TropicalTree1();
		}
		return veg;
	}

}
