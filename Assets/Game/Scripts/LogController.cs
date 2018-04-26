using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogController : MonoBehaviour {
	List<GameObject> log_entries = new List<GameObject>();

	public static LogController Instance;

	public RectTransform LogParent;
	
	public GameObject TextPrefab;
	
	void Awake() {
		Debug.Assert(Instance == null, "LogController is initialized multiple times");
		Instance = this;
	}
	
	void OnDestroy() {
		Instance = null;
	}

	public void Clear() {
		foreach(var entry in log_entries)
			Destroy(entry);
		log_entries.Clear();
	}
	
	public void Log(string text) {
		var go = Instantiate(TextPrefab, LogParent);
		TextOps.SetText(go, text);
		log_entries.Add(go);
	}
}
