using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuaLancher : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        XLuaManager.Instance.StartGame();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
