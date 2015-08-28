using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Behaviour_PatrolPoints : GBehaviour {

	List<Vector2> patrol_points;
	int current_point_objective;

	int lim = 5000;

	public Behaviour_PatrolPoints(List<Vector2> points) {
		patrol_points = points;
		current_point_objective = 0;
	}

	public override void performBehaviour() {
		if (lim > 0) {
			advanceTowardsObjective();
			lim--;
		} 
		else {
			Debug.Log ("I have completed my behaviour");
			owner.clearBehaviour();
		}
	}

	void advanceTowardsObjective() {
		checkPointReached ();
		owner.moveTowards (patrol_points [current_point_objective]);
	}

	void checkPointReached() {
		Vector2 pos = owner.getPosition();
		if (pos.x == patrol_points [current_point_objective].x && pos.y == patrol_points [current_point_objective].y) {
			// point reached
			if(current_point_objective < patrol_points.Count-1) {
				current_point_objective++;
			}
			else {
				current_point_objective = 0;
			}
		}
	}

}
