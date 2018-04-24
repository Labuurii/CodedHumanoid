using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LoadSceneOnClick : MonoBehaviour {

    public string Scene;

	void Start () {
        GetComponent<Button>().onClick.AddListener(on_click);
	}

    private void on_click()
    {
        SceneManager.LoadScene(Scene);
    }
}
