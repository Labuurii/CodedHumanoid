using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DisplayLanguages : MonoBehaviour
{
    List<Button> buttons;

    public Button Prefab;
    public Button AddButton;
    public DisplayStandardLibs DisplayStandardLibs;
    public LanguageEditorState State;

    private void Start()
    {
        AddButton.transform.SetParent(null);

        var langs = PermanentData.Languages;
        buttons = new List<Button>();
        foreach(var lang in langs)
        {
            var go = Instantiate(Prefab.gameObject, transform);
            var txt = go.GetComponentInChildren<Text>();
            txt.text = lang.name;
            var btn = go.GetComponent<Button>();
            buttons.Add(btn);
            btn.onClick.AddListener(() => on_click(lang));
        }

        AddButton.transform.SetParent(transform);
    }

    private void on_click(Language lang)
    {
        DisplayStandardLibs.Display(lang);
        State.Language = lang;
    }
}
