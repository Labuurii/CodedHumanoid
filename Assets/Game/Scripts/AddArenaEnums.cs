using ArenaServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class AddArenaEnums : MonoBehaviour {

    private void Awake()
    {
        var options = new List<Dropdown.OptionData>();
        foreach (var name in Enum.GetNames(typeof(Arena)))
        {
            options.Add(new Dropdown.OptionData
            {
                text = name
            });
        }
        GetComponent<Dropdown>().options = options;
    }
}
