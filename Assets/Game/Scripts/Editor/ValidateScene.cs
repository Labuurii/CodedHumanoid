using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ValidateScene : MonoBehaviour {

    [MenuItem("CH/Validate Scene")]
    static void Validate()
    {
        if(SceneManager.sceneCount > 1)
        {
            Debug.LogError("There can only be one scene open at once.");
            return;
        }

        {
            var go = GameObject.Find("MainThreadEventQueue");
            if(go == null)
            {
                Debug.LogError("There are no 'MainThreadEventQueue'");
                return;
            }

            if(go.GetComponent<MainThreadEventQueue>() == null)
            {
                Debug.LogError("GameObject 'MainThreadEventQueue' does not have any 'MainThreadEventQueue' component");
            }
        }

        Debug.Log("Scene valid!");
    }
}
