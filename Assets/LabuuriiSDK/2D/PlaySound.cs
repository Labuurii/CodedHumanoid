using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySound : MonoBehaviour {
    bool has_init;
    public AudioSource source;
    public bool AllowOverlap;
	
	// Update is called once per frame
	void Update () {
		if(!has_init)
        {
            has_init = true;
            {
                var slider = GetComponent<Slider>();
                if(slider != null)
                {
                    slider.onValueChanged.AddListener(play_sound);
                    return;
                }
            }

            {
                var btn = GetComponent<Button>();
                if(btn != null)
                {
                    btn.onClick.AddListener(play_sound);
                }
            }

            Debug.LogWarning("Could not register event handler. Only button and slider is supported.");
        }
	}

    private void play_sound()
    {
        if (!source.isPlaying || AllowOverlap)
            source.Play();
    }

    private void play_sound(float v)
    {
        play_sound();
    }
}
