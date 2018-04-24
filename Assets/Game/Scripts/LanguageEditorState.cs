using ArenaServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class LanguageEditorState : MonoBehaviour
{
    Language lang;
    public Language Language
    {
        get
        {
            return lang;
        }
        set
        {
            lang = value;
            OnStateChanged.Invoke(value, library);
        }
    }

    Arena library;
    public Arena Arena
    {
        get
        {
            return library;
        }
        set
        {
            library = value;
            OnStateChanged.Invoke(lang, value);
        }
    }

    private class StateChangedCB : UnityEvent<Language, Arena> { }

    public UnityEvent<Language, Arena> OnStateChanged = new StateChangedCB();
}
