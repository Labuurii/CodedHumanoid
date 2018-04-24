using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackedContentGoBackBtn : MonoBehaviour {

    public StackedContent StackedContent;


	// Use this for initialization
	void Start () {
        ButtonOps.OnClick(gameObject, on_click);
	}

    private void on_click()
    {
        StackedContent.Pop();
    }
}
