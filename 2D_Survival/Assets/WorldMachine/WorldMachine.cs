using UnityEngine;
using System.Collections;

public class WorldMachine : MonoBehaviour {

	public static WorldMachine WMachine;
	public WorldGenerator generator;

	void Awake() {
		if (WMachine == null) {
			WMachine = this;
			DontDestroyOnLoad(WMachine);
		}
		else if (WMachine != this) {
			Destroy (gameObject);
		}
	}
	
	public void Init() {
		generator = new WorldGenerator ();
	}

}
