using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ContentButton : MonoBehaviour {

    public RectTransform Target;
    public ContentController ContentController;

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(on_click);
	}

    private void on_click()
    {
        ContentController.SetContent(Target);
    }
}
