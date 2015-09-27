using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldGenerator {

	public void generateScenery() {
		Vector2 selected_tile_index;
		GTile selected_tile;
		TileSubdivisions subdivision;
		GStructure veg;
		int objs = 1000;

		while (objs > 0) {

			selected_tile_index = new Vector2(UnityEngine.Random.Range(0, 50), UnityEngine.Random.Range(0, 50));
			selected_tile = GameData.GData.getTile(selected_tile_index);
			veg = null;

			int vegType = UnityEngine.Random.Range(0, 100);
			if(vegType >= 0 && vegType < 20) {
				veg = getRandomTree();
			} 
			if(vegType >= 20 && vegType < 30) {
				veg = getRandomSceneryProp();
			}
			if(vegType >= 30 && vegType < 100) {
				veg = getRandomBush();
			}


			if(veg != null) {
				if (veg.dimensions.size == StructureSizes.Large) {
					if(!selected_tile.containsStructures()) {
						// set position
						objs--;
					}
				} else if(veg.dimensions.size == StructureSizes.Medium) {
					if(!selected_tile.containsStructures()) {
						veg.setPosition (selected_tile, Vector2.zero);
						objs--;
					}
				} else if (veg.dimensions.size == StructureSizes.Small) {
					subdivision = GTile.GetRandomSubdivision();
					if(!selected_tile.containsStructures(subdivision)) {
						veg.setPosition (selected_tile, subdivision);
						objs--;
					}
				}
			}

		}

	}

	GStructure getRandomBush() {
		GStructure veg = null;
		int vegType = UnityEngine.Random.Range(0, 100);
		if(vegType >= 0 && vegType < 15) {
			veg = new OBJ_Bush1();
		} 
		if(vegType >= 15 && vegType < 20) {
			veg = new OBJ_Bush2();
		}
		if(vegType >= 20 && vegType < 51) {
			veg = new OBJ_Bush3();
		}
		if(vegType >= 51 && vegType < 53) {
			veg = new OBJ_Bush4();
		}
		if(vegType >= 53 && vegType < 54) {
			veg = new OBJ_Bush5();
		}
		if(vegType >= 54 && vegType < 60) {
			veg = new OBJ_Bush6();
		}
		if(vegType >= 60 && vegType < 80) {
			veg = new OBJ_Bush7();
		}
		if(vegType >= 80 && vegType < 100) {
			veg = new OBJ_Bush8();
		}
		return veg;
	}

	GStructure getRandomBush(StructureSizes size) {
		GStructure veg = null;
		int vegType = UnityEngine.Random.Range(0, 100);

		return veg;
	}
	
	GStructure getRandomTree() {
		GStructure veg = null;
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

	GStructure getRandomTree(StructureSizes size) {
		GStructure veg = null;
		int vegType = UnityEngine.Random.Range(0, 100);

		return veg;
	}

	GStructure getRandomSceneryProp() {
		GStructure prop = null;
		int propType = UnityEngine.Random.Range(0, 100);
		if(propType >= 0 && propType < 10) {
			prop = new OBJ_Rock1();
		} 
		if(propType >= 10 && propType < 50) {
			prop = new OBJ_Rock2();
		} 
		if(propType >= 50 && propType < 60) {
			prop = new OBJ_Rock3();
		}
		if(propType >= 60 && propType < 100) {
			prop = new OBJ_Rock4();
		}
		return prop;
	}

}
