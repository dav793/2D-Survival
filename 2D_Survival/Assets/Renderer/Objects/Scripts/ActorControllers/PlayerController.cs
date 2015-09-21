using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : CharacController {
		
	float mov_speed = 1f;

	public override void execTick() {
		base.execTick ();
	}

	void FixedUpdate() {
		checkInput ();
		execTick ();
	}

	void checkInput() {
		checkMovement ();
	}

	void checkMovement() {
		if(KeyboardInputController.input.UP || KeyboardInputController.input.RIGHT || KeyboardInputController.input.DOWN || KeyboardInputController.input.LEFT) {

			Vector2 mov_vector = new Vector2(0, 0);
			if(KeyboardInputController.input.UP) {
				mov_vector.y += getMaxSpeed();
			}
			if(KeyboardInputController.input.DOWN) {
				mov_vector.y -= getMaxSpeed();
			}
			if(KeyboardInputController.input.RIGHT) {
				mov_vector.x += getMaxSpeed();
			}
			if(KeyboardInputController.input.LEFT) {
				mov_vector.x -= getMaxSpeed();
			}

			tryToMove(mov_vector);

		}
	}

}
