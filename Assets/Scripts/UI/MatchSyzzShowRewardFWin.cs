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
            var stageCamera = GameObject.Find("Stage Camera");
            var camera = stageCamera.GetComponent<Camera>();
            camera.cullingMask = -1;
            Debug.Log(("打开了奖励弹窗"));
            Object prefab = Resources.Load("Role/npc");
            go = (GameObject)Object.Instantiate(prefab);
            go.transform.position = new Vector3(2.9f,-5.93f, -2);
            go.AddComponent<RotationSelf>();
            
            var holder = new GGraph();
            holder.color = Color.red;
            contentPane.AddChild(holder);
            holder.Center();
            GoWrapper wrapper = new GoWrapper(go);
            holder.SetNativeObject(wrapper);
            go.transform.localScale = new Vector3(400,400,400);
            var clickChild = new GGraph();
            clickChild.DrawRect(100,100,0,Color.black, Color.red);
            holder.parent.AddChild(clickChild);
        }

        public override void Close(CloseType type = CloseType.Close)
        {
            GameObject.Destroy(go);
            base.Close();
        }
    }
}