using ArenaServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class DisplayStandardLibs : MonoBehaviour
{
    List<Button> buttons = new List<Button>();

    public Button Prefab;
    public LanguageEditorState LanguageEditorState;

    public void Display(Language lang)
    {
        foreach (var btn in buttons)
            Destroy(btn.gameObject);
        buttons.Clear();

        if(lang.standard_libraries != null)
        {
            foreach(var lib in lang.standard_libraries)
            {
                var go = Instantiate(Prefab.gameObject, transform);
                var btn = go.GetComponent<Button>();
                var txt = go.GetComponentInChildren<Text>();
                txt.text = lib.arena.ToString();
                buttons.Add(btn);
                btn.onClick.AddListener(() => on_click(lib.arena));
            }
        }
    }

    private void on_click(Arena arena)
    {
        LanguageEditorState.Arena = arena;
    }
}
