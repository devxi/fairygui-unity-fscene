using System;
using System.Collections.Generic;
using FairyGUI;
using JetBrains.Annotations;
using UnityEngine;

namespace LQ
{
    
    public interface ISceneCfg
    {
        string name { get; set; }
        string pkgPath { get; set; }
        string pkgName { get; set; }
        [CanBeNull] string componentName { get; set; }
    }


    public class PkgDefine : ISceneCfg
    {
        public string name { get; set; }
        public string pkgPath { get; set; }
        public string pkgName { get; set; }
        [CanBeNull] public string componentName { get; set; }
    }

    public class FSceneConfig : ISceneCfg
    {
        public string name { get; set; }
        public string pkgPath { get; set; }
        public string pkgName { get; set; }
        public string componentName { get; set; }
        
    }

    public class FScene : GComponent
    {
        
        public static bool IsInit { get; private set; }
        public static GComponent FSceneRoot { get; private set; }

        public static bool AutoDestoryAtClosed = true;

        protected ISceneCfg sceneCfg;
        protected bool   autoDestoryAtClosed = AutoDestoryAtClosed;
        
        private Dictionary<string, GComponent> childrenMap = new Dictionary<string, GComponent>();
        protected Dictionary<string, Controller> ctrlsMap = new Dictionary<string, Controller>();

    


        public static void FSceneInit()
        {
            if (IsInit)
            {
                Debug.Log("请勿重复初始化FScene");
            }
            else
            {
                IsInit = true;
                GRoot.inst.SetContentScaleFactor(
                    1080,
                    1980, 
                    UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
                FSceneRoot = new GComponent();
                FSceneRoot.name = FSceneRoot.displayObject.gameObject.name = "FSceneRoot";
                GRoot.inst.AddChildAt(FSceneRoot, 0);
                FSceneRoot.SetSize(FSceneRoot.parent.width, FSceneRoot.parent.height);
                FSceneRoot.AddRelation(FSceneRoot.parent, RelationType.Size);
            }
        }
        
        public static T CreateFScene<T>(FSceneConfig cfg) where T: FScene
        {
            if (!IsInit)
            {
                FSceneInit();
            }
       
            UIPackage.AddPackage(cfg.pkgPath);
            GObject obj = UIPackage.CreateObject(cfg.pkgName, cfg.componentName, typeof(T));
            if (obj == null)
            {
                throw new Exception("找不到指定组件 path:" + cfg.pkgPath + "组件名称:" + cfg.componentName);
            }
            var scene = obj as T; 
            scene.AfterConstructorCall(cfg);
            return scene;
        }

        private void AfterConstructorCall(FSceneConfig cfg)
        {
            BuildChildMap();
            BuildControllerMap();
            displayObject.name = cfg.name + "hahaha";
            displayObject.gameObject.name += "(" + cfg.name + ")";
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

        public void Open(object param)
        {
            FSceneRoot.AddChild(this);
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

        public void OnClosed()
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