using UnityEngine;
using System.Collections;

public class WorldGenerator {

	public void generateScenery() {

		int objs = 700;
		while (objs > 0) {

			Vector2 selected_tile_index = new Vector2(UnityEngine.Random.Range(0, 50), UnityEngine.Random.Range(0, 50));
			GTile selected_tile = GameData.GData.getTile(selected_tile_index);
			if(selected_tile.Contained_Objects.structures.isEmpty()) {

				GObject veg = null;
				int veg_index = UnityEngine.Random.Range(0, 4);
				switch(veg_index) {
					case 0:
						veg = new OBJ_TropicalTree1();
						break;
					case 1:
						veg = new OBJ_PalmTree1();
						break;
					case 2:
						veg = new OBJ_TropicalBush1();
						break;
					case 3:
						veg = new OBJ_TropicalBush2();
						break;
				}
				if(veg != null) {
					veg.setPosition (selected_tile, new Vector2(UnityEngine.Random.Range(-10,10), UnityEngine.Random.Range(-10,10)));
					objs--;
				}

			}

		}

	}

}
