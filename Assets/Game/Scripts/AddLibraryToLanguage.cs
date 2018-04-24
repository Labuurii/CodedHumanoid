using ArenaServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AddLibraryToLanguage : MonoBehaviour {

    public LanguageEditorState State;
    public bool Demo;

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(on_click);
	}

    private void on_click()
    {
        var lang = State.Language;
        if (lang == null)
            return;

        var demo = Demo && Application.isEditor;

        RuntimeEditor.Load(demo, lang, Arena.None);
    }
}
