using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public struct CompileInstructions
{
    public HashSet<string> files;
}

/// <summary>
/// Reference semantics.
/// </summary>
public struct CompileTask
{
    Process compile;
    Process run;

    public CompileTask(Process compile, Process run)
    {
        this.compile = compile;
        this.run = run;            
    }

    public string Command
    {
        get
        {
            return compile.StartInfo.FileName + " " + compile.StartInfo.Arguments;
        }
    }

    public bool RequiresCompile
    {
        get
        {
            return compile != null;
        }
    }

    public Process Process
    {
        get
        {
            return compile;
        }
    }

    public void Start()
    {
        compile.Start();
    }

    public bool HasCompiled()
    {
        return compile.HasExited;
    }

    /// <summary>
    /// Call this after <see cref="NextCompileOutputLine"/> has returned a null or empty string.
    /// </summary>
    /// <returns></returns>
    public bool IsCompilerError()
    {
        System.Diagnostics.Debug.Assert(HasCompiled());
        return compile.ExitCode != 0;
    }

    public RunTask Run()
    {
        System.Diagnostics.Debug.Assert(HasCompiled() && !IsCompilerError());
        return new RunTask(run);
    }
}

public struct RunTask
{
    Process process;

    public RunTask(Process process)
    {
        this.process = process;
    }

    public Process Process
    {
        get
        {
            return process;
        }
    }

    public void Start()
    {
        process.Start();
    }

    public bool HasRun()
    {
        return process.HasExited;
    }

    public bool IsRunError()
    {
        return process.ExitCode != 0;
    }
}

[Serializable]
public class ArbitraryCodeExecutor {
    string compiler_command;
    string run_command;
    string code;
    bool show_window;

    public ArbitraryCodeExecutor(string compiler_command, string run_command, string code, bool show_window)
    {
        this.compiler_command = compiler_command;
        this.run_command = run_command;
        this.code = code;
        this.show_window = show_window;
    }

    public CompileTask Compile(CompileInstructions instructions)
    {
        Process compile = null;
        Process run = null;

        var script_engine = new Script();
        script_engine.Globals.Set("files", DynValue.FromObject(script_engine, instructions.files));
        script_engine.Globals["create_tmp_file"] = (Func<string, string>)FileOps.CreateTmpFile;
        script_engine.Globals["create_tmp_dir"] = (Func<string>)FileOps.CreateTmpDir;

        var script_inst_dyn = script_engine.DoString(code, null, "");
        if (script_inst_dyn.Type != DataType.Table)
            throw new Exception(string.Format("Script should return a lua table, not '{0}'", script_inst_dyn.Type));

        var script_inst = script_inst_dyn.Table;

        //Compile arguments
        {
            var compile_args_dyn = script_inst.Get("compile_args");
            if (compile_args_dyn != null)
            {

                if (compile_args_dyn.Type != DataType.String)
                    throw new Exception(string.Format("'compile_args' should be a string of command line parameters, not '{0}'", compile_args_dyn.Type));
                var compile_args = compile_args_dyn.String;
                compile = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = !show_window,
                        FileName = compiler_command,
                        Arguments = compile_args,
                    },
                };
            }
        }

        //Run arguments
        {
            var run_args_dyn = script_inst.Get("run_args");
            if (run_args_dyn == null)
                throw new Exception("'run_args' is required to be able to run the code.");
            if (run_args_dyn.Type != DataType.String)
                throw new Exception(string.Format("'run_args' has to be a a string. Not '{0}'", run_args_dyn.Type));

            string command;
            var custom_command_dyn = script_inst.Get("run_file");
            if(custom_command_dyn != null)
            {
                if (custom_command_dyn.Type != DataType.String)
                    throw new Exception(string.Format("'run_file' has to be a string, not '{0}'", custom_command_dyn.String));
                command = custom_command_dyn.String;
            } else
            {
                command = run_command;
            }

            run = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = !show_window,
                    FileName = command,
                    Arguments = run_args_dyn.String,
                },
            };
        }

        return new CompileTask(compile, run);
    }

    
}
