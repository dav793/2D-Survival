using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {

	public static GameData GData;

	private GTile[,] world;

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

	public void Init(int x, int y) {
		world = new GTile[x, y];
	}

}
