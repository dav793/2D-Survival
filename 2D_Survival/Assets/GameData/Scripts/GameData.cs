using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {

	public static GameData GData;

	private GTile[,] worldTiles;

	void Awake() {
		if (GData == null) {
			GData = this;
			DontDestroyOnLoad(GData);
		}
		else if (GData != this) {
			Destroy(gameObject);
		}
	}

	public void Tick() {

	}

	public void Init(GameData_Settings conf) {
		worldTiles = new GTile[conf.world_size_x, conf.world_size_y];
		for (int x = 0; x < conf.world_size_x; ++x) {
			for (int y = 0; y < conf.world_size_y; ++y) {
				worldTiles[x, y] = new GTile(x, y);
			}
		}
	}

}
