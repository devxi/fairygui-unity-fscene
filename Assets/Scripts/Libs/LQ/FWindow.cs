using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using JetBrains.Annotations;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

namespace LQ
{

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
        /// 以非模式窗口形式弹窗窗体
        /// </summary>
        FWindow Show(object param = null, bool closeOther = false, bool showPopEffect = true);

        bool IsShowEffect { get; set; }
    }
    public class FWindow : FairyGUI.Window, IDialog
    {
        
        public static GRoot UIRoot { get; private set; }
        public static bool AutoDestoryAtClosed = true;
        protected static bool IsInit = false;


        public virtual bool CloseOnClickOutSide { get; set; } = true;
        public virtual ISceneCfg WinDefine { get; set; }
        public bool IsShowEffect { get; set; } = false;


        protected bool   autoDestoryAtClosed = AutoDestoryAtClosed;
        
        private Dictionary<string, GComponent> childrenMap = new Dictionary<string, GComponent>();
        protected Dictionary<string, Controller> ctrlsMap = new Dictionary<string, Controller>();


       public static void  Init() {
            // fairygui.UIConfig.modalLayerColor = "rgba(33,33,33,0.5)"
            // // Laya.stage.on(Laya.Event.RESIZE, this , this.onStageResize)
            // FWindow.root = new fgui.GRoot()
            // FWindow.root.displayObject.zOrder = 999
            // FWindow.root.setSize(Laya.stage.width, Laya.stage.height)
            // FWindow.root.displayObject.name = "FWindow弹窗层"
            // Laya.stage.addChild(FWindow.root.displayObject)
            if (IsInit)
            {
                Debug.Log("请勿重复初始化FWindow");
                
            }
            else
            {
                // UIConfig.modalLayerColor = new Color(33,33,33, 0.5f);
                UIRoot= new GRoot();
                UIRoot.displayObject.gameObject.name = "FWindow弹窗层";
                UIRoot.SetSize(Stage.inst.width, Stage.inst.height);
                Stage.inst.AddChild(UIRoot.displayObject);
                UIRoot.AddRelation(UIRoot.parent, RelationType.Size);
            }
        }

        public FWindow()
        {
            AfterConstructorCall();
            this.Hide();
        }



        private void AfterConstructorCall()
        {
            Debug.Log("FWindow AfterConstructorCall");
            BuildChildMap();
            BuildControllerMap();
            SetPivot(0.5f, 0.5f);
        }

        public void BuildChildMap()
        {
            foreach (var child in this._children)
            {
                if (child is GComponent)
                {   
                    // Debug.Log("BuildChildMap -> " + child.name);
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
                try
                {   
                    modal = true;
                    IsShowEffect = showPopEffect;
                    UIPackage.AddPackage(WinDefine.pkgPath);
                    contentPane = UIPackage.CreateObject(WinDefine.pkgName, WinDefine.componentName).asCom;
                    if (CloseOnClickOutSide)
                        UIRoot.ShowPopup(this);
                    else UIRoot.ShowWindow(this);
                    
                    contentPane.parent.displayObject.gameObject.name += "(" + WinDefine.name + ")";
                    
                    //居中关联
                    Center();
                    AddRelation(UIRoot, RelationType.Center_Center);
                    AddRelation(UIRoot, RelationType.Middle_Middle);
                    __OnOpened(param);
                    return this;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            }
            else
            {
                throw new Exception("该window无配置信息 winCfg ");
            }
            return null;
        }

        public FWindow Show(object param = null, bool closeOther = false, bool showPopEffect = true)
        {
            if (WinDefine != null)
            {
                try
                {
                    IsShowEffect = showPopEffect;
                    modal = false;
                    UIPackage.AddPackage(WinDefine.pkgPath);
                    contentPane = UIPackage.CreateObject(WinDefine.pkgName, WinDefine.componentName).asCom;
  
                    UIRoot.ShowWindow(this);
                    
                    contentPane.parent.displayObject.gameObject.name += "(" + WinDefine.name + ")";
                    
                    //居中关联
                    Center();
                    AddRelation(UIRoot, RelationType.Center_Center);
                    AddRelation(UIRoot, RelationType.Middle_Middle);
                    __OnOpened(param);
                    return this;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    throw;
                }
            }
            else
            {
                throw new Exception("该window无配置信息 winCfg ");
            }
            return null;
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

        public void Close()
        {
            Hide();
        }

        private  void __OnClosed()
        {
            if (autoDestoryAtClosed)
            {
                // 销毁 
                Dispose();
            }
            OnClosed();
        }

        /// <summary>
        /// 给子类重写OnClosed
        /// </summary>
        public virtual void OnClosed()
        {
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

