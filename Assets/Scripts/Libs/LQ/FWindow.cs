using System;
using System.Collections;
using System.Collections.Generic;
using FairyGUI;
using JetBrains.Annotations;
using TreeEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace LQ
{

    interface IDialog
    {
        bool CloseOnClickOutSide { get; set; }
        ISceneCfg WinDefine { get; set; }
    }
    public class FWindow : FairyGUI.Window, IDialog
    {
        
        public static GRoot UIRoot { get; private set; }
        public static bool AutoDestoryAtClosed = true;
        protected static bool IsInit = false;


        public virtual bool CloseOnClickOutSide { get; set; } = true;
        public virtual ISceneCfg WinDefine { get; set; }
        
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
        }



        private void AfterConstructorCall()
        {
            Debug.Log("FWindow AfterConstructorCall");
            BuildChildMap();
            BuildControllerMap();
            SetPivot(0.5f, 0.5f);
            modal = true;
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

        public FWindow Popup(object param = null)
        {
            if (WinDefine != null)
            {
                try
                {   
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
        
        

        public void Open(object param)
        {
            GRoot.inst.AddChild(this);
            //和父容器一样大
            SetSize(parent.width, parent.height);
            //和父容器宽高关联
            AddRelation(parent, RelationType.Size);
            __OnOpened(param);
        }

        public virtual void OnOpened([CanBeNull] object param)
        {
            Debug.Log(name + "OnOpened");
        }

        private void __OnOpened(object param)
        {
            OnOpened(param);
        }

        public void Close()
        {
            if (autoDestoryAtClosed)
            {
                //销毁 销毁包含了RemoveFromParent
                Dispose();
            }
            else
            {   //只从父节点移除 不销毁
                RemoveFromParent();
            }
            __OnClosed();
        }

        private void __OnClosed()
        {
            OnClosed();
        }

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

    }
}

