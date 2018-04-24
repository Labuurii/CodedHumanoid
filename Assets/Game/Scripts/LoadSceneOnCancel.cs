using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnCancel : MonoBehaviour {
    public string Scene;
    public bool Async;

    private void Update()
    {
        if(Input.GetButtonUp("Cancel"))
        {
            if(Async)
            {
                SceneManager.LoadSceneAsync(Scene);
            } else
            {
                SceneManager.LoadScene(Scene);
            }
        }
    }
}
