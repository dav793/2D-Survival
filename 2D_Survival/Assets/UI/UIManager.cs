using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	public static UIManager UI;

	public GameObject canvas;
	public GameObject SmallHoveringPanel_Prefab;
	public UI_InfoPanel info_panel;

	void Awake() {
		if (UI == null) {
			UI = this;
			DontDestroyOnLoad(UI);
		}
		else if (UI != this) {
			Destroy(gameObject);
		}
	}

	public void Init() {
		info_panel.gameObject.SetActive( false);
	}

	public void setInfo(string text) {
		info_panel.gameObject.SetActive (true);
		info_panel.setText (text);
	}

	public void hideInfo() {
		info_panel.clearText ();
		info_panel.gameObject.SetActive (false);
	}

	public SmallHoveringPanel createSHP(GameObject anchor) {
		GameObject panelObj = Instantiate (SmallHoveringPanel_Prefab) as GameObject;
		panelObj.transform.SetParent (canvas.transform);
		SmallHoveringPanel smh = panelObj.GetComponent<SmallHoveringPanel> ();
		smh.Init (anchor, this);
		anchor.GetComponent<GRObject> ().assoc_shp = smh;
		return smh;
	}

	public void destroySHP(GameObject anchor) {
		if (anchor.GetComponent<GRObject> ().assoc_shp != null) {
			SmallHoveringPanel shp = anchor.GetComponent<GRObject> ().assoc_shp;
			anchor.GetComponent<GRObject> ().assoc_shp = null;
			Destroy (shp.gameObject);
		}
	}

	public Vector3 GetScreenPosition(Transform transform, Camera cam) {
		Vector3 pos;
		float width = canvas.GetComponent<RectTransform>().sizeDelta.x;
		float height = canvas.GetComponent<RectTransform>().sizeDelta.y;
		float x = Camera.main.WorldToScreenPoint(transform.position).x / Screen.width;
		float y = Camera.main.WorldToScreenPoint(transform.position).y / Screen.height;
		pos = new Vector3(width * x, y * height);    
		return pos;    
	}

}
