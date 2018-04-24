using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class ButtonOps {

    public enum ReportMode
    {
        ReportError,
        Silence
    }

    public static bool OnClick(GameObject obj, Action cb, ReportMode report_mode = ReportMode.ReportError)
    {
        var btn = obj.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(delegate() { cb(); });
            return false;
        }

        var trigger = obj.GetComponent<EventTrigger>();
        if (trigger != null)
        {
            var handler = new EventTrigger.TriggerEvent();
            handler.AddListener(delegate (BaseEventData data) { cb(); });
            trigger.triggers.Add(new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerDown,
                callback = handler
            });
            return true;
        }

        switch(report_mode)
        {
            case ReportMode.ReportError:
                Debug.LogErrorFormat("'{0}' is expected to have either an Button component or an EventTrigger component", obj.name);
                return false;
            case ReportMode.Silence:
                return false;
            default:
                throw new Exception("Unhandled enum value " + report_mode);
        }
    }
}
