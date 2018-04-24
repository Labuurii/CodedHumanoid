using ArenaServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Language
{
    public string
        name,
        compile_command,
        run_command,
        file_extension,
        args_script;
    public List<StandardLib> standard_libraries;

    public Language()
    {
        if(standard_libraries == null)
            standard_libraries = new List<StandardLib>();
    }

    public StandardLib GetStdLib(Arena arena)
    {
        return standard_libraries.Find((l) => l.arena == arena);
    }
}

[Serializable]
public class StandardLib
{
    public Arena arena;
    public string code;
}
