using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RunCode : MonoBehaviour {

    public RobotController Robot;

	void Start () {
        GetComponent<Button>().onClick.AddListener(on_click);
	}

    private void on_click()
    {
        Robot.Run();
    }
}
