using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class StatusText : MonoBehaviour {
    Text text;

    public Color DefaultColor, WarningColor = Color.yellow, ErrorColor = Color.red, SuccessColor = Color.green;
    public State DemoState;

    public enum State
    {
        NoDemo,
        Default,
        Warning,
        Error,
        Success
    }

	// Use this for initialization
	void Start ()
    {
        init_if_required();
    }

    private void OnValidate()
    {
        if(Application.isEditor && !Application.isPlaying)
        {
            const string str = "Some status";
            switch(DemoState)
            {
                case State.NoDemo:
                    Message("");
                    break;
                case State.Default:
                    Message(str);
                    break;
                case State.Error:
                    Error(str);
                    break;
                case State.Success:
                    Success(str);
                    break;
                case State.Warning:
                    Warning(str);
                    break;
                default:
                    throw new System.Exception("Unhandled enum value " + DemoState);
            }
        }
    }

    private void init_if_required()
    {
        if(text == null)
            text = GetComponent<Text>();
    }

    public void Message(string msg)
    {
        set(msg, DefaultColor);
    }

    public void Warning(string msg)
    {
        set(msg, WarningColor);
    }

    public void Error(string msg)
    {
        set(msg, ErrorColor);
    }

    public void Success(string msg)
    {
        set(msg, SuccessColor);
    }

    private void set(string msg, Color c)
    {
        init_if_required();
        text.text = msg;
        text.color = c;
    }
}
