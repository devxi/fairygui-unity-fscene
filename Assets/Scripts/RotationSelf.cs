using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationSelf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.Rotate(Vector3.up * 90 * Time.deltaTime);
        if (Input.GetMouseButtonDown(0))
        {
            // Debug.Log("GetMouseButtonDown");
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("点击了3D角色");
    }
}
