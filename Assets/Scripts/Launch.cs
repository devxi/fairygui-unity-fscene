using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using LQ;
using UI.Config;
using UnityEngine;

public class Launch : MonoBehaviour
{
    private void Awake()
    {
        //先加载公用基础包，因为其他包对基础包有依赖
        UIPackage.AddPackage("UI/PublicPackage00");
        UIPackage.AddPackage("UI/PublicPackage01");
        MatchGameRankScene matchRankScene = FScene.CreateFScene<MatchGameRankScene>(FSceneDefine.MatchSyzzFSceneConfig);
        matchRankScene.Open(FSceneDefine.MatchSyzzFSceneConfig);
    }
}
