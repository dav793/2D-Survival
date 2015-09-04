using UnityEngine;
using System.Collections;

public class CharacController : ActorController {

	public override void execTick() {
		base.execTick ();
	}

	void FixedUpdate() {
		execTick ();
	}

}
