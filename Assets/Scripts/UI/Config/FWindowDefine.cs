using LQ;

namespace UI.Config
{
    public static class FWindowDefine
    {
        public static FSceneConfig MatchSyzzShowRewardFWinConfig = new FSceneConfig
        {        
            name         = "显示奖励弹窗_水鱼至尊挑战赛",
            componentName= "Win_ClearFuFen",
            pkgPath      = PkgDefine.MatchPackage.pkgPath,
            pkgName      = PkgDefine.MatchPackage.pkgName,
        };
    }
}