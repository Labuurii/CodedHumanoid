using ArenaServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EditLibraryAndLanguage : MonoBehaviour {
    public LanguageEditorState State;
    public bool Demo;

	// Use this for initialization
	void Start () {
        GetComponent<Button>().onClick.AddListener(on_click);
	}

    private void on_click()
    {
        var lang = State.Language;
        var arena = State.Arena;
        if (lang == null || arena == Arena.None)
            return;

        var demo = Demo && Application.isEditor;

        RuntimeEditor.Load(demo, lang, arena);
    }
}
