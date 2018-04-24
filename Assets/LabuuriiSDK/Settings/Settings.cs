using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum SettingType
{
    Float,
    Int,
    String
}

public struct Setting
{
    string name;
    SettingType type;

    internal Setting(string name, SettingType type)
    {
        this.name = name;
        this.type = type;
    }

    public bool Has()
    {
        return PlayerPrefs.HasKey(name);
    }

    public int Int()
    {
        Debug.Assert(type == SettingType.Int);
        return PlayerPrefs.GetInt(name);
    }

    public float Float()
    {
        Debug.Assert(type == SettingType.Float);
        return PlayerPrefs.GetFloat(name);
    }

    public string String()
    {
        Debug.Assert(type == SettingType.String);
        return PlayerPrefs.GetString(name);
    }

    public void SetInt(int v)
    {
        Debug.Assert(type == SettingType.Int);
        PlayerPrefs.SetInt(name, v);
    }

    public void SetFloat(float v)
    {
        Debug.Assert(type == SettingType.Float);
        PlayerPrefs.SetFloat(name, v);
    }

    public void SetString(string v)
    {
        Debug.Assert(type == SettingType.String);
        PlayerPrefs.SetString(name, v);
    }
}
