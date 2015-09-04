using UnityEngine;
using System.Collections;

public class GBehaviour {

	public GActor owner;

	public virtual void performBehaviour() {

	}

	public virtual void stopBehaviour() {
		Debug.Log ("I have completed my behaviour");
		owner.clearBehaviour();
	}

}
