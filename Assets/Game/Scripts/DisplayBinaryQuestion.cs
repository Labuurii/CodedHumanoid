using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBinaryQuestion : MonoBehaviour {
    private Action yes_cb, no_cb;

    public Text Header, Description;
    public Button Yes, No;

    void Start () {
        Yes.onClick.AddListener(on_yes);
        No.onClick.AddListener(on_no);

        if (!Application.isEditor)
            gameObject.SetActive(false);
	}

    public void Display(string header, string desc, Action on_yes, Action on_no)
    {
        gameObject.SetActive(true);
        Header.text = header;
        Description.text = desc;
        yes_cb = on_yes;
        no_cb = on_no;
    }

    private void on_no()
    {
        yes_cb();
        gameObject.SetActive(false);
    }

    private void on_yes()
    {
        no_cb();
        gameObject.SetActive(false);
    }
}
