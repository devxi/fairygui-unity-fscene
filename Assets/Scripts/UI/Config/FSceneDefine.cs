using System;
using LQ;

namespace UI.Config
{
    public static  class FSceneDefine
    {
        public  static FSceneConfig MatchSyzzFSceneConfig = new FSceneConfig
        {
            name = "比赛场排行榜",
            pkgPath = PkgDefine.MatchPackage.pkgPath,
            pkgName = PkgDefine.MatchPackage.pkgName,
            componentName = "UIPanel_Match",
        };
    }
}