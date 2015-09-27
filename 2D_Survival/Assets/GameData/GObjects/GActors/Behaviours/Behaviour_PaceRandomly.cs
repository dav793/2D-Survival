using UnityEngine;
using System.Collections;

public class Behaviour_PaceRandomly : GBehaviour {

	Vector2 destinationPosition;
	bool started = false;
	int reach_counter;

	public Behaviour_PaceRandomly() {

	}
	
	public override void performBehaviour() {
		if (!started) {
			destinationPosition = owner.getPosition ();
			reach_counter = 500;
			started = true;
		}
		if (destinationPosition.x != owner.getPosition ().x || destinationPosition.y != owner.getPosition ().y) {
			// advance towards destination
			if (!owner.moveTowards (destinationPosition) || reach_counter <= 0) {
				destinationPosition = owner.getPosition ();
				reach_counter = 500;
			}
			reach_counter--;
		} 
		else {
			// at destination
			if(UnityEngine.Random.Range(0, 100) < 1) {
				// set new random destination
				destinationPosition = new Vector2(
					destinationPosition.x + UnityEngine.Random.Range (-60, 60),
					destinationPosition.y + UnityEngine.Random.Range (-60, 60)
				);
			}
			reach_counter = 500;
		}
	}
	


}
