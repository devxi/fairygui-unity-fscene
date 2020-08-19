using System;
using FairyGUI;
using LQ;
using UI.Config;
using UnityEngine;
using XLua;

public class Launch : MonoBehaviour
{
    private LuaEnv luaEnv;
    private void Awake()
    {
        //先加载公用基础包，因为其他包对基础包有依赖
        UIPackage.AddPackage("UI/PublicPackage00");
        UIPackage.AddPackage("UI/PublicPackage01");

        var scene = FScene.CreateFScene<MatchGameRankScene>(FSceneDefine.MatchSyzzFSceneConfig);
        scene.Open(null);
        luaEnv = new LuaEnv();
        // luaEnv.AddLoader(LuaSciptLoader);
        luaEnv.DoString("CS.UnityEngine.Debug.Log('lua hello world')");
        luaEnv.DoString("require 'Main'");
        luaEnv.Dispose();
    }

    public byte[] LuaSciptLoader(ref string filepath)
    {
        string path = "lua/" + filepath + ".txt"; 
        Debug.Log("正在加载：" + path);
        TextAsset script = Resources.Load<TextAsset>(path);
        if (script == null)
            throw new Exception("读取错误:" + path);
        return script.bytes;
    }
    
    void Destroy()
    {
        if (luaEnv != null)
        {
            luaEnv.Dispose();
        }
    }
}