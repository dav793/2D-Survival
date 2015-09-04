using UnityEngine;
using System.Collections;

public class AnimalController : ActorController {

	public override void execTick() {
		base.execTick ();
	}
	
	void FixedUpdate() {
		execTick ();
	}

}
