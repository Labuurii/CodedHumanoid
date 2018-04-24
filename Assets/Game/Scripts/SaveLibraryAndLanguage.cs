using ArenaServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SaveLibraryAndLanguage : MonoBehaviour {

    public LanguageEditorState LanguageEditorState;
    public DisplayErrorMessage DisplayErrorMessage;
    public InputField Name;
    public InputField CompilerCommand;
    public InputField RunCommand;
    public InputField CodeExtension;
    public Dropdown ArenaDropdown;
    public InputField ArgumentsGeneratorCode;
    public InputField StandardLibraryCode;

    void Start () {
        GetComponent<Button>().onClick.AddListener(on_click);
	}

    private void on_click()
    {
        if(string.IsNullOrEmpty(Name.text))
        {
            DisplayErrorMessage.Push(new ErrorMsg
            {
                Header = "Could not save.", //TRANSLATE
                Description = "Name is required." //TRANSLATE
            });
            return;
        }

        var arena = (Arena) Enum.Parse(typeof(Arena), ArenaDropdown.options[ArenaDropdown.value].text);

        if(arena == Arena.None)
        {
            DisplayErrorMessage.Push(new ErrorMsg
            {
                Header = "Could not save.", //TRANSLATE
                Description = "Arena is not set." //TRANSLATE
            });
            return;
        }

        var langs = PermanentData.Languages;
        var lang = LanguageEditorState.Language;
        if(lang == null)
        {
            lang = new Language();
            langs.Add(lang);
        }

        StandardLib std_lib;
        {
            var idx = lang.standard_libraries.FindIndex((lib) => lib.arena == arena);
            if(idx == -1)
            {
                std_lib = new StandardLib();
                lang.standard_libraries.Add(std_lib);
            } else
            {
                std_lib = lang.standard_libraries[idx];
            }
        }

        std_lib.code = StandardLibraryCode.text;
        std_lib.arena = arena;
        lang.name = Name.text;
        lang.compile_command = CompilerCommand.text;
        lang.run_command = RunCommand.text;
        lang.file_extension = CodeExtension.text;
        lang.args_script = ArgumentsGeneratorCode.text;
        PermanentData.Languages = langs;
        PlayerPrefs.Save();
    }
}
