using UnityEngine;
using System.Collections;

public class GameData : MonoBehaviour {

	public static GameData GData;

	void Awake() {
		if (GData == null) {
			GData = this;
			DontDestroyOnLoad(GData);
		}
	}

	public void Tick() {

	}

	public void Init() {
		
	}

}
