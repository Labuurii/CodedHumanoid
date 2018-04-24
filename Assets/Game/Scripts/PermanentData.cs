using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LangsWrapper
{
    public List<Language> languages;
}

public static class PermanentData {
    private const string LanguagesKey = "pl_war_languages";

    private static List<Language> languages_cache;
    public static List<Language> Languages
    {
        get
        {
            if (languages_cache != null)
                return languages_cache;

            var setting = PlayerPrefs.GetString(LanguagesKey);
            if (string.IsNullOrEmpty(setting))
            {
                languages_cache = new List<Language>();
            } else
            {
                try
                {
                    languages_cache = JsonUtility.FromJson<LangsWrapper>(setting).languages;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }

                if (languages_cache == null)
                    languages_cache = new List<Language>();
            }


            return languages_cache;
        }
        set
        {
            Debug.Assert(value == languages_cache, "Another list have been created. This probably means inconsistent data");
            PlayerPrefs.SetString(LanguagesKey, JsonUtility.ToJson(new LangsWrapper
            {
                languages = value
            }));
        }
    }
}
