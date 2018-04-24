using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedContent : MonoBehaviour {
    List<RectTransform> stack = new List<RectTransform>();

    public RectTransform Default;

	public void Push(RectTransform t)
    {
        if(stack.Count > 0)
        {
            stack[stack.Count - 1].gameObject.SetActive(false);
        } else
        {
            Default.gameObject.SetActive(false);
        }
        t.gameObject.SetActive(true);
        stack.Add(t);
    }

    public void Pop()
    {
        Debug.Assert(stack.Count > 0, "Push is not paired with Pop");
        var popped = stack[stack.Count - 1];
        stack.RemoveAt(stack.Count - 1);
        popped.gameObject.SetActive(false);
        if(stack.Count > 0)
        {
            stack[stack.Count - 1].gameObject.SetActive(true);
        } else
        {
            Default.gameObject.SetActive(true);
        }
    }

    public void ClearStack()
    {
        if (stack.Count > 0)
            stack[stack.Count - 1].gameObject.SetActive(false);
        stack.Clear();
        Default.gameObject.SetActive(true);
    }
}
