using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SaveInEditor : MonoBehaviour {
#if UNITY_EDITOR

    private void Start()
    {
        var field = GetComponent<InputField>();
        if (field != null)
            field.text = EditorPrefs.GetString("save_in_editor_" + gameObject.name);
    }

    private void OnDestroy()
    {
        var text = GetComponent<InputField>();
        if (text != null)
            EditorPrefs.SetString("save_in_editor_" + gameObject.name, text.text);
    }

#endif
}
