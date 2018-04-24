using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotController : MonoBehaviour {
    

    static readonly Type[] static_types = new[]
    {
        typeof(Vector3),
        typeof(Quaternion)
    };

    static readonly Type[] registered_types = new[]
    {
        typeof(RobotContext),
        typeof(Transform),
        typeof(Vector3),
        typeof(Quaternion)
    };

    Script script;
    RobotContext robot_context;
    IUserDataDescriptor robot_context_desc;

    public InputField CodeField;



    class RobotContext
    {
        MonoBehaviour mono;

        public RobotContext(MonoBehaviour mono)
        {
            this.mono = mono;
        }

        public Vector3 position
        {
            get
            {
                return mono.transform.position;
            }
        }

        public Quaternion rotation
        {
            get
            {
                return mono.transform.rotation;
            }
        }
    }


    private void Start()
    {
        script = new Script();
        robot_context = new RobotContext(this);
        foreach (var type in registered_types)
            UserData.RegisterType(type);
        robot_context_desc = UserData.GetDescriptorForObject(robot_context);
    }

    public void Run()
    {
        var code = CodeField.text;

        script.Globals.Set("robot", UserData.Create(robot_context, robot_context_desc));
        script.Globals["print"] = DynValue.NewCallback(lua_print_generic);
        foreach(var static_ in static_types)
        {
            script.Globals.Set(static_.Name, UserData.CreateStatic(static_));
        }
        
        script.DoString(code);
    }

    private DynValue lua_print_generic(ScriptExecutionContext context, CallbackArguments args)
    {
        for(var i = 0; i < args.Count; ++i)
        {
            var arg = args.RawGet(i, false);
            Debug.Log(arg.ToPrintString());
        }

        return DynValue.Nil;
    }
}
