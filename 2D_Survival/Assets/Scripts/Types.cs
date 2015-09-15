using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *  File: Types
 *  Contains auxiliary/utility types and classes for general use within the whole application.
 */

public enum CardinalDirections { None, N, NE, E, SE, S, SW, W, NW };

public enum MovDirections { Sides, Up, Down };

public enum BiomeTypes { TropicalForest, ConiferousForest, Desert, AbandonedSettlement };

public enum TileVertices { topLeft, topRight, bottomRight, bottomLeft };

public enum OperationMode { OUT_OF_CHARACTER_RANGE, WITHIN_CHARACTER_RANGE };

public enum BOOL_YN { YES, NO };

public struct Prog_Settings {
	public int zunits_per_level;
};

