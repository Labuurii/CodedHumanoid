using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct ErrorMsg
{
    public string Header, Description;
}

public class DisplayErrorMessage : MonoBehaviour {

    private Queue<ErrorMsg> queue = new Queue<ErrorMsg>();

    public Text Header;
    public Text Description;
    public Button Ok;
    public float MinErrorMsgDeltaTime = 0.2f;

	// Use this for initialization
	void Start () {
        Ok.onClick.AddListener(on_click);
	}

    private void on_click()
    {
        gameObject.SetActive(false);
        if(queue.Count != 0)
        {
            StartCoroutine(display_next_error());
        }
    }

    private IEnumerator display_next_error()
    {
        yield return new WaitForSeconds(MinErrorMsgDeltaTime);
        var next_error_msg = queue.Dequeue();
        display_msg(next_error_msg);
    }

    private void display_msg(ErrorMsg msg)
    {
        gameObject.SetActive(true);
        Header.text = msg.Header;
        Description.text = msg.Description;
    }

    public void Push(ErrorMsg msg)
    {
        if(gameObject.activeSelf) //Error msg are displayed already
        {
            queue.Enqueue(msg);
        } else
        {
            display_msg(msg);
        }
    }
}
