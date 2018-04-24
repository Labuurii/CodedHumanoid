using ArenaServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public static class RuntimeEditor
{
    const string DemoScene = "DemoRuntimeEditor";
    const string ReleaseScene = "RuntimeEditor";

    public static void Load(bool demo, Language lang, Arena arena)
    {
        SceneManager.sceneLoaded += (scene, mode) => //Be really careful about not copying the this pointer. Because it will be destroyed.
        {
            var target_scene_state_go = GameObject.Find("State");
            var target_scene_state = target_scene_state_go.GetComponent<LanguageEditorState>();
            target_scene_state.Language = lang;
            target_scene_state.Arena = arena;

            GameObject.Find("ArgsGenerator").GetComponent<InputField>().text = lang.args_script;
            GameObject.Find("LibraryCode").GetComponent<InputField>().text = lang.GetStdLib(arena)?.code;
            GameObject.Find("Name").GetComponent<InputField>().text = lang.name;
            var dropdown = GameObject.Find("Arena").GetComponent<Dropdown>();
            dropdown.value = DropdownOps.OptionValueByString(dropdown, arena.ToString());
            GameObject.Find("CompileCommand").GetComponent<InputField>().text = lang.compile_command;
            GameObject.Find("RunCommand").GetComponent<InputField>().text = lang.run_command;
            GameObject.Find("CodeExtension").GetComponent<InputField>().text = lang.file_extension;
        };
        SceneManager.LoadScene(demo ? DemoScene : ReleaseScene);
    }
}
