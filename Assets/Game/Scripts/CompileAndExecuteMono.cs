using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class LogCB : UnityEvent<string>
{
}

public class CompileAndExecuteMono : MonoBehaviour
{
    CompileAndExecute c = new CompileAndExecute();

    public string CompilerCommand {
        get
        {
            return c.CompilerCommand;
        }
        set
        {
            c.CompilerCommand = value;
        }
    }
    public string RunCommand {
        get
        {
            return c.RunCommand;
        }
        set
        {
            c.RunCommand = value;
        }
    }
    public string CodeExtension {
        get
        {
            return c.CodeExtension;
        }
        set
        {
            c.CodeExtension = value;
        }
    }
    public string ArgumentsGeneratorCode {
        get
        {
            return c.ArgumentsGeneratorCode;
        }
        set
        {
            c.ArgumentsGeneratorCode = value;
        }
    }
    public string StandardLibraryCode {
        get
        {
            return c.StandardLibraryCode;
        }
        set
        {
            c.StandardLibraryCode = value;
        }
    }
    public string RunnableCode {
        get
        {
            return c.RunnableCode;
        }
        set
        {
            c.RunnableCode = value;
        }
    }
    public Dictionary<string, string> LibraryFiles {
        get
        {
            return c.LibraryFiles;
        }
        set
        {
            c.LibraryFiles = value;
        }
    }

    public UnityEvent<string> OnLog = new LogCB();

    private void Start()
    {
        c.OnLog.AddListener(on_log);
    }

    private void on_log(string msg)
    {
        MainThreadEventQueue.Enqueue(() =>
        {
            OnLog.Invoke(msg);
        });
    }

    public void RunAsync()
    {
        c.RunAsync();
    }
}
