using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *  File: Types
 *  Contains auxiliary/utility types and classes for general use within the whole application.
 */

public enum CardinalDirections { None, N, NE, E, SE, S, SW, W, NW };

public enum ActorActions { Idle, Running };

//public enum GObjectType { Player, Character, Animal };

public enum BiomeTypes { TropicalForest, ConiferousForest, Desert, AbandonedSettlement };

public enum TileVertices { topLeft, topRight, bottomRight, bottomLeft };

public enum OperationMode { OUT_OF_CHARACTER_RANGE, WITHIN_CHARACTER_RANGE };

public enum BOOL_YN { YES, NO };

public struct Prog_Settings {
	public int zunits_per_level;
};

[System.Serializable]
public class Animator_Clip_Pair {
	public Animator animator;
	AnimationClip clip;
	public void setClip(AnimationClip newClip) {
		clip = newClip;
	}
	public AnimationClip getClip() {
		return clip;
	}
}

public class Pair<T, U> {
	public Pair() {
	}
	public Pair(T first, U second) {
		this.First = first;
		this.Second = second;
	}
	public T First { get; set; }
	public U Second { get; set; }
}