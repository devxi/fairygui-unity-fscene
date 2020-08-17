using System;
using System.Collections.Generic;
using FairyGUI;
using JetBrains.Annotations;
using UnityEngine;


namespace LQ
{

    public enum CloseType
    {
        Close,
        Yes,
        No,
    } 

    interface IDialog
    {
        /// <summary>
        /// 点击窗体外部空白处是否关闭窗体
        /// </summary>
        bool CloseOnClickOutSide { get; set; }
        ISceneCfg WinDefine { get; set; }
        /// <summary>
        /// 以模式窗口形式弹出窗体
        /// </summary></param>
        /// <returns></returns>
        FWindow Popup(object param = null, bool closeOther = false, bool showPopEffect = true);
        
        /// <summary>
        /// 关闭弹窗
        /// </summary>
        /// <param name="type">弹窗关闭原因</param>
        void Close(CloseType type = CloseType.Close,bool showCloseEffect = true);

        bool IsShowEffect { get; set; }
    }
    public class FWindow : FairyGUI.Window, IDialog
    {
        
        public static GRoot FWindowRoot { get; private set; }
        public static bool AutoDestoryAtClosed = true;
        private static bool IsInit = false;
        

        public virtual bool CloseOnClickOutSide { get; set; } = true;
        public virtual ISceneCfg WinDefine { get; set; }
        public bool IsShowEffect { get; set; } = false;

        public GameObject gameObject { get; private set; }

        private CloseType closeType = CloseType.Close;


        protected bool   autoDestoryAtClosed = AutoDestoryAtClosed;
        
        private Dictionary<string, GComponent> childrenMap = new Dictionary<string, GComponent>();
        protected Dictionary<string, Controller> ctrlsMap = new Dictionary<string, Controller>();


       private static void FWindowInit() {
           if (IsInit)
            {
                Debug.Log("请勿重复初始化FWindow");
            }
            else
           {
                IsInit = true;
                FWindowRoot = GRoot.inst;
                // GRoot.inst.AddChildAt(FWindowRoot,1);
                // FWindowRoot.name = FWindowRoot.displayObject.name = FWindowRoot.displayObject.gameObject.name = "FWindowRoot";
                // FWindowRoot.ApplyContentScaleFactor();
           }
        }

        public FWindow()
        {
            if (!IsInit) 
                FWindowInit();
            if (!FScene.IsInit)
                FScene.FSceneInit();
            AfterConstructorCall();
        }



        private void AfterConstructorCall()
        {
            SetPivot(0.5f, 0.5f);
        }

        public void BuildChildMap()
        {        
            if (this._children.Count > 0 && this._children[0] != null)
            {
                foreach (var child in _children[0].asCom._children)
                {
                    childrenMap.Add(child.name, child.asCom);   
                }
            }
        }

        public void BuildControllerMap()
        {
            foreach (var controller in _controllers)
            {
                ctrlsMap.Add(controller.name, controller);
            }
        }
        
        
        public FWindow Popup(object param = null, bool closeOther = false, bool showPopEffect = true)
        {
            if (WinDefine != null)
            {
                modal = true;
                IsShowEffect = showPopEffect;
                if (closeOther)
                {
                    GRoot.inst.CloseAllWindows(); 
                }
                DoShowWindow(param);
                return this;
            }
            else
            {
                throw new Exception("该window无配置信息 winCfg ");
            }
        }

        private void DoShowWindow(object param)
        {
            try
            {
                UIPackage.AddPackage(WinDefine.pkgPath);
                contentPane = UIPackage.CreateObject(WinDefine.pkgName, WinDefine.componentName).asCom;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                throw;
            }

            if (CloseOnClickOutSide || (CloseOnClickOutSide && modal))
                FWindowRoot.ShowPopup(this);
            else
                FWindowRoot.ShowWindow(this);

            gameObject = contentPane.parent.displayObject.gameObject;
            gameObject.name += "(" + WinDefine.name + ")";
            
            Center();
            AddRelation(FWindowRoot, RelationType.Center_Center);
            AddRelation(FWindowRoot, RelationType.Middle_Middle);

            //这两个要保证要__OnOpened之前调用
            BuildChildMap();
            BuildControllerMap();
            __OnOpened(param);
        }
        
        /// <summary>
        /// 给子类重写OnOpened
        /// </summary>
        public virtual void OnOpened([CanBeNull] object param)
        {
            
        }

        private void __OnOpened(object param)
        {
            OnOpened(param);
        }
        protected override void closeEventHandler(EventContext context)
        {
            __Close(CloseType.Close);
        }

        private  void __Close(CloseType type = CloseType.Close)
        {

            Close(type);
        }

        /// <summary>
        /// 关闭弹窗 重写时记得调用Base.Close()，否则不会关闭弹窗
        /// </summary>
        /// <param name="type"></param>
        public virtual void Close(CloseType type = CloseType.Close, bool showCloseEffect = true)
        {
            closeType = type;
            if (showCloseEffect)
                Hide();
            else
            {
                HideImmediately();
                //如果是不播放关闭动画 直接立即隐藏的，这里立即隐藏后要立即触发__OnClosed()
                __OnClosed();
            }
        }

        private  void __OnClosed()
        {
            if (autoDestoryAtClosed)
            {
                // 销毁 
                Dispose();
            }
            OnClosed(closeType);
        }

        /// <summary>
        /// 给子类重写OnClosed
        /// </summary>
        public virtual void OnClosed(CloseType type)
        {
            //这个是给子类重写的，让子类在弹窗关闭后可以做一些事
        }



        public void OnClickChild(string childName, EventCallback1 callback)
        {
            if (this.childrenMap.ContainsKey(childName))
            {
                this.childrenMap[childName].onClick.Add(callback);
            }
            else
            {
                Debug.LogWarning("无法为节点" + childName + "添加点击事件,找不到该子节点");
            }
        }

        #region 弹窗弹出动画、关闭动画

        protected override void DoShowAnimation()
        {
            if (!IsShowEffect)
            {
                base.DoShowAnimation();
                return;
            }
            visible = false;
            GTween.To(new Vector2(0, 0), new Vector2(1, 1), 0.3f)
                .SetEase(EaseType.BackOut)
                .OnUpdate((GTweener tweener) =>
                {
                    visible = true; 
                    scale = new Vector2(tweener.value.x, tweener.value.y); 
                    base.DoShowAnimation();
                });
        }

        protected override void DoHideAnimation()
        {
            if (!IsShowEffect)
            {
                base.DoHideAnimation();
                __OnClosed();
                return;
            }
            GTween.To(new Vector2(1, 1), new Vector2(0, 0), 0.3f)
                .SetTarget(this)
                .SetEase(EaseType.Custom)
                .OnUpdate((GTweener tweener) =>
                {
                    scale = new Vector2(tweener.value.x, tweener.value.y);
                }).OnComplete((tweener =>
                {
                    base.DoHideAnimation();
                    //如果有动画,那么关闭动画播放完毕后才触发__OnClosed
                    __OnClosed();
                }));
        }

        #endregion

    }
}

