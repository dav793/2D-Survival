using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UI_InfoPanel : MonoBehaviour {

	public Text text_area;

	public void setText(string text) {
		text_area.text = text;
	}

	public void clearText() {
		setText ("");
	}

}
