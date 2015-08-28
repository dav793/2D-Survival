using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class GRenderEventManager : MonoBehaviour {

	private static GRenderEventManager RenderEventManager;

	private Dictionary <string, UnityEvent> eventDictionary;

	public static GRenderEventManager instance {
		get {
			if(!RenderEventManager) {
				RenderEventManager = FindObjectOfType(typeof (GRenderEventManager)) as GRenderEventManager;
				if(!RenderEventManager) {
					Debug.LogError("There needs to be an active GRenderEventManager script on the GameRenderer GameObject in your scene.");
				}
				else {
					RenderEventManager.Init();
				}
			}
			return RenderEventManager;
		}
	}

	void Init() {
		if (eventDictionary == null) {
			eventDictionary = new Dictionary<string, UnityEvent> ();
		}
	}

	public static void StartListening (string eventName, UnityAction listener) {
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent)) {
			thisEvent.AddListener (listener);
		} 
		else {
			thisEvent = new UnityEvent ();
			thisEvent.AddListener(listener);
			instance.eventDictionary.Add (eventName, thisEvent);
		}
	}
	
	public static void StopListening (string eventName, UnityAction listener) {
		if(RenderEventManager == null) return;
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent)) {
			thisEvent.RemoveListener (listener);
		} 
	}
	
	public static void TriggerEvent(string eventName) {
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent)) {
			thisEvent.Invoke ();
		}
	}

}
