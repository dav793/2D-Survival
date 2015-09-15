using UnityEngine;
using System.Collections;

public class GActor : GObject {
	
	public WorldSector sector;
	public BOOL_YN environmental;
	public BOOL_YN npc;

	public float max_speed = 1f;

	public bool idle;
	public GBehaviour active_behaviour;

	public GActor_RefList_Index reflist_index;

	public GActor(BOOL_YN environmental, BOOL_YN npc) {
		base.type = GObjectType.Actor;	
		this.environmental = environmental;
		this.npc = npc;
		idle = true;
		reflist_index = new GActor_RefList_Index ();
	}

	public override void setPosition(Vector2 point) {
		if (placeAtPoint (point)) {
			GameData.GData.moveObjToSector (this, GameData.GData.getSectorFromWorldPoint (new Vector2 (pos_x, pos_y)));
		} 
	}
	
	public override void setPosition(Vector2 sector_index, Vector2 offset) {
		if (placeAtPoint (GameData.GData.getSector (sector_index).getCenter (offset))) {
			GameData.GData.moveObjToSector (this, GameData.GData.getSector (sector_index));
		} 
	}

	/*
	 * Attempts to place the GActor instance at <point>. If successful true is returned. False is returned otherwise.
	 * If placement is succesful and the new point is part of a different sector, moves the GActor reference to the new sector.
	 */
	public override bool placeAtPoint(Vector2 point) {
		if(base.placeAtPoint(point)) {
			WorldSector sec = GameData.GData.getSectorFromWorldPoint(point);
			if(sec != sector) {
				GameData.GData.moveObjToSector (this, sec);
			}
			return true;
		}
		return false;
	}

	public void setBehaviour(GBehaviour behaviour) {
		active_behaviour = behaviour;
		behaviour.owner = this;
		idle = false;
	}

	public void clearBehaviour() {
		idle = true;
		active_behaviour = null;
	}

	public void performBehaviour() {
		if (idle) {
			idle = false;
		}
		active_behaviour.performBehaviour ();
	}

	public void moveTowards(Vector2 point) {
		Vector2 mov_vector = new Vector2 (0, 0);
		if (point.x > pos_x) {
			mov_vector.x += Mathf.Abs(point.x-pos_x);
		} 
		else if (point.x < pos_x) {
			mov_vector.x -= Mathf.Abs(point.x-pos_x);
		}
		if (point.y > pos_y) {
			mov_vector.y += Mathf.Abs(point.y-pos_y);
		} 
		else if (point.y < pos_y) {
			mov_vector.y -= Mathf.Abs(point.y-pos_y);
		}
		moveBy (mov_vector);
	}

	public void moveBy(Vector2 mov_vector) {

		Vector2 corrected_mov = new Vector2 (Mathf.Min( Mathf.Abs(mov_vector.x), max_speed ), Mathf.Min( Mathf.Abs(mov_vector.y), max_speed ));
		if (mov_vector.x < 0) {
			corrected_mov.x *= -1;
		}
		if (mov_vector.y < 0) {
			corrected_mov.y *= -1;
		}

		if(corrected_mov.x != 0 || corrected_mov.y != 0) {		// if object needs to move
			placeAtPoint (new Vector2(pos_x + corrected_mov.x, pos_y + corrected_mov.y));
		}

	}

	public override bool isActor() {
		return true;
	}

}
