using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RunPLInDev : MonoBehaviour {
    public CompileAndExecuteMono CompileAndExecuteMono;
    public InputField CompilerCommand;
    public InputField RunCommand;
    public InputField CodeExtension;
    public InputField ArgumentsGeneratorCode;
    public InputField StandardLibraryCode;
    public InputField RunnableCode;
    public Text LogText;

    // Use this for initialization
    void Start () {
        GetComponent<Button>().onClick.AddListener(on_click);
        CompileAndExecuteMono.OnLog.AddListener(on_log);
	}

    private void on_log(string arg0)
    {
        var text = LogText.text;
        text += arg0;
        text += "\n";
        LogText.text = text;
    }

    private void on_click()
    {
        CompileAndExecuteMono.CompilerCommand = CompilerCommand.text;
        CompileAndExecuteMono.RunCommand = RunCommand.text;
        CompileAndExecuteMono.CodeExtension = CodeExtension.text;
        CompileAndExecuteMono.ArgumentsGeneratorCode = ArgumentsGeneratorCode.text;
        CompileAndExecuteMono.StandardLibraryCode = StandardLibraryCode.text;
        CompileAndExecuteMono.RunnableCode = RunnableCode.text;
        CompileAndExecuteMono.RunAsync();
    }
}
