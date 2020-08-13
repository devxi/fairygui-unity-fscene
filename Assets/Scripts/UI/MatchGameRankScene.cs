using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using LQ;
using UI;
using UnityEngine;

public class MatchGameRankScene : FScene
{
    public override void OnOpened(object param)
    {
        Debug.Log("MatchGameRankScene " + "OnOpened");
        OnClickChild("Btn_Reward", OnClickReward) ;
    }

    private void OnClickReward(EventContext content)
    {
        Debug.Log("点击了奖励弹窗");
        new MatchSyzzShowRewardFWin().Popup();
    }

}
