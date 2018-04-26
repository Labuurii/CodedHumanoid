using UnityEngine;
using UnityEngine.UI;

public static class TextOps {
	
	public static void SetText(GameObject go, string text) {
		{
			var comp = go.GetComponent<Text>();
			if(comp != null)
			{
				comp.text = text;
				return;
			}
		}
		
		Debug.LogErrorFormat("GameObject '{0}' does not have any texty component like Text", go.name);
	}
}