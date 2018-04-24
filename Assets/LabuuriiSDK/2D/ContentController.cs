using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentController : MonoBehaviour {
    RectTransform last;

    public RectTransform Default;

    private void Start()
    {
        if (Default != null)
            SetContent(Default);
    }

    public void SetContent(RectTransform t)
    {
        if(last != null)
        {
            last.gameObject.SetActive(false);
        }
        t.gameObject.SetActive(true);
        last = t;
    }
}
