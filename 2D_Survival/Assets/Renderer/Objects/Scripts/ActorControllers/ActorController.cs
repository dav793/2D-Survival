using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ActorController : MonoBehaviour {

	MovDirections movement_direction;
	bool is_moving;
	int stall = 0;
	Vector2 last_position;
	
	bool facingRight = true;
	Animator anim;
	GActor actor;

	public virtual void execTick() {
		updatePosition ();
	}

	void FixedUpdate () {
		execTick ();
	}
	
	void OnEnable() {
		anim = GetComponent<Animator> ();
		last_position = new Vector2(transform.position.x, transform.position.y);
		ResetScale ();
	}

	void updatePosition() {

		Vector2 vec = new Vector2(transform.position.x, transform.position.y) - last_position;
		if (vec != Vector2.zero) {		// if it has moved
			stall = 4;
			// set movement direction
			if(vec.y != 0) {
				if(vec.y < 0) {
					movement_direction = MovDirections.Down; // (1)
				}
				else if(vec.y > 0) {
					movement_direction = MovDirections.Up; // (2)
				}
			}
			if(vec.x != 0) {
				movement_direction = MovDirections.Sides; // (0)
				if(vec.x > 0) {
					if(!facingRight) {
						FlipHScale ();
					}
					facingRight = true;
				}
				else if(vec.x < 0) {
					if(facingRight) {
						FlipHScale ();
					}
					facingRight = false;
				}
			}
			anim.SetFloat ("MovDirection", (int)movement_direction);
			is_moving = true;
		} 
		else {
			stall--;
			if(stall <= 0) {
				is_moving = false;
			}
		}
		
		last_position = transform.position;
		anim.SetBool ("IsMoving", is_moving);
		
	}
	
	void FlipHScale() {
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void ResetScale() {
		transform.localScale = new Vector3 (1, 1, 1);
	}

	public void tryToMove(Vector2 movement_vector) {
		if (actor != null) {
			actor.moveBy(movement_vector);	
		}
	}

	public float getMaxSpeed() {
		if (actor != null) {
			return actor.max_speed;
		}
		return 0f;
	}

	public void linkActor(GActor act) {
		actor = act;
	}

}
