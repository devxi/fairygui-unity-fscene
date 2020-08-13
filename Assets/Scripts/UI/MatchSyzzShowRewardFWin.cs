using LQ;
using UI.Config;
using UnityEngine;

namespace UI
{
    

    
    public class MatchSyzzShowRewardFWin: FWindow
    {
        public override ISceneCfg WinDefine => FWindowDefine.MatchSyzzShowRewardFWinConfig;

        public override void OnOpened(object param)
        {
            Debug.Log(("打开了奖励弹窗"));
        }
    }
}