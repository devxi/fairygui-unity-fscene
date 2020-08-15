using FairyGUI;
using LQ;
using UI.Config;
using UnityEngine;

namespace UI
{
    

    
    public class MatchSyzzShowRewardFWin: FWindow
    {
        private GameObject go;

        public override bool CloseOnClickOutSide { get; set; } = false;

        public override ISceneCfg WinDefine => FWindowDefine.MatchSyzzShowRewardFWinConfig;

        public override void OnOpened(object param)
        {
            Debug.Log(("打开了奖励弹窗"));
            var prefab = Resources.Load<GameObject>("Role/npc");
            go = (GameObject)GameObject.Instantiate(prefab);
            go.transform.position = new Vector3(2.9f,-5.93f, -2);
            go.AddComponent<RotationSelf>();
            
            var holder = new GGraph();
            holder.color = Color.red;
            contentPane.AddChild(holder);
            holder.Center();
            GoWrapper wrapper = new GoWrapper(go);
            holder.SetNativeObject(wrapper);
            go.transform.localScale = new Vector3(400, 400, 400);
        }

        public override void Close(CloseType type = CloseType.Close)
        {
            GameObject.Destroy(go);
            base.Close();
        }
    }
}