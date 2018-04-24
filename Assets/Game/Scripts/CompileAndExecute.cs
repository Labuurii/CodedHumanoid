using MoonSharp.Interpreter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class CompileAndExecute {

    public string CompilerCommand { get; set; }
    public string RunCommand { get; set; }
    public string CodeExtension { get; set; }
    public string ArgumentsGeneratorCode { get; set; }
    public string StandardLibraryCode { get; set; }
    public string RunnableCode { get; set; }
    public Dictionary<string, string> LibraryFiles { get; set; }

    /// <summary>
    /// Can run from any thread.
    /// </summary>
    public UnityEvent<string> OnLog = new LogCB();

    public void RunAsync()
    {
        new Script(); //To make sure all required resources are loaded.

        Task.Run(() =>
        {
            var lib_files = new HashSet<string>();

            OnLog.Invoke("Exporting library files...");
            try
            {
                {
                    var runnable_file_name = FileOps.CreateTmpFile(CodeExtension);
                    File.WriteAllText(runnable_file_name, RunnableCode);
                    lib_files.Add(runnable_file_name);
                }

                if(!string.IsNullOrEmpty(StandardLibraryCode))
                {
                    var library_file_name = FileOps.CreateTmpFile(CodeExtension);
                    File.WriteAllText(library_file_name, StandardLibraryCode);
                    lib_files.Add(library_file_name);
                }
                

                if(LibraryFiles != null)
                {
                    var lib_dir = FileOps.CreateTmpDir();
                    foreach (var other_lib_file in LibraryFiles)
                    {
                        var lib_file = Path.Combine(lib_dir, other_lib_file.Key);
                        lib_file = Path.ChangeExtension(lib_file, CodeExtension);
                        File.WriteAllText(lib_file, other_lib_file.Value);
                        lib_files.Add(lib_file);
                    }
                }
                
            } catch(Exception e)
            {
                OnLog.Invoke(string.Format("Failure writing down all library files:\n{0}", e));
                return;
            }

            var code_generator = new ArbitraryCodeExecutor(CompilerCommand, RunCommand, ArgumentsGeneratorCode, true);

            OnLog.Invoke("Generating commands...");
            CompileTask compiler;
            try
            {
                compiler = code_generator.Compile(new CompileInstructions
                {
                    files = lib_files
                });
            } catch(SyntaxErrorException e)
            {
                OnLog.Invoke(e.DecoratedMessage);
                return;
            }
            catch (Exception e)
            {
                OnLog.Invoke(e.Message);
                return;
            }

            if (compiler.RequiresCompile)
            {
                try
                {
                    OnLog.Invoke("Compiling...");
                    OnLog.Invoke(compiler.Command);
                    var p = compiler.Process;
                    p.OutputDataReceived += on_process_output;
                    p.ErrorDataReceived += on_process_output;
                    compiler.Start();
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    p.WaitForExit();

                    if (compiler.IsCompilerError())
                    {
                        OnLog.Invoke("Failed to compile.");
                        return;
                    }
                } catch(Exception e)
                {
                    OnLog.Invoke(e.ToString());
                    return;
                }
            }

            OnLog.Invoke("Running...");
            try
            {
                var executable = compiler.Run();
                var p = executable.Process;
                p.OutputDataReceived += on_process_output;
                p.ErrorDataReceived += on_process_output;
                executable.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();

                if (executable.IsRunError())
                {
                    OnLog.Invoke("Failed to run code.");
                }
                else
                {
                    OnLog.Invoke("Successfully ran code.");
                }
            } catch(Exception e)
            {
                OnLog.Invoke(e.ToString());
                return;
            }
        });
    }

    private void on_process_output(object sender, DataReceivedEventArgs e)
    {
        OnLog.Invoke(e.Data);
    }
}
