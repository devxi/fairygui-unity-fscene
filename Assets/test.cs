using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("屏幕尺寸： " + Screen.width + "*" + Screen.height);
        var uiPanel = GetComponent<UIPanel>();
        var ui = uiPanel.ui;
        ui.GetChildAt(0).visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
