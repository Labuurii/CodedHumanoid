using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackContentOpenBtn : MonoBehaviour {

    public StackedContent StackedContent;
    public RectTransform Target;

	// Use this for initialization
	void Start () {
        ButtonOps.OnClick(gameObject, on_click);
	}

    private void on_click()
    {
        StackedContent.Push(Target);
    }
}
