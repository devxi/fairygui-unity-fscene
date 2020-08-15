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
        OnClickChild("Btn_Reward", OnClickReward);
        this.GetChildAt(0).alpha = 0.0f;
        Debug.Log("MatchGameRankScene 场景大小：" + this.height + "," + this.width);
    }

    private void OnClickReward(EventContext content)
    {
        Debug.Log("点击了奖励弹窗");
        new MatchSyzzShowRewardFWin().Popup();
        new MatchSyzzShowRewardFWin().Show();
    }

}
