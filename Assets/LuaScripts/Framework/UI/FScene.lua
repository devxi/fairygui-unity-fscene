--[[
    FScene是对fgui的封装
    author: nothing
    time  : 2020-08-19 12: 02: 20
]]





local ClassName = "FScene"

local FScene = class(ClassName)


FScene._inited = false
FScene.FSceneRoot = nil

--[[
    @desc: 初始化FScene
    author:nothing
    time:2020-08-19 21:36:35
    @return:
]]
local function initFScene()
    assert(FScene._inited == false, "Fscene已经初始化，请勿重复初始化")
    GRoot.inst:SetContentScaleFactor(1080, 1980)
    FScene.FSceneRoot = GRoot.inst
    print("FScene init")

    -- IsInit = true;
    -- GRoot.inst.SetContentScaleFactor(
    --     1080,
    --     1980, 
    --     UIContentScaler.ScreenMatchMode.MatchWidthOrHeight);
    -- FScene.FSceneRoot = new GComponent()
    -- FSceneRoot.name = FSceneRoot.displayObject.gameObject.name = "FSceneRoot";
    -- GRoot.inst.AddChildAt(FSceneRoot, 0);
    -- FSceneRoot.SetSize(FSceneRoot.parent.width, FSceneRoot.parent.height);
    -- FSceneRoot.AddRelation(FSceneRoot.parent, RelationType.Size);
end

function FScene:ctor(sceneDefine)

    assert(sceneDefine, "场景声明配置为空")
    assert(sceneDefine.pkgPath, "场景所在包路径为空")
    assert(sceneDefine.pkgName, "场景包名称为空")
    assert(sceneDefine.componentName, "场景组件名称为空")

    if FScene._inited == false then
        initFScene()
    end

    UIPackage.AddPackage(sceneDefine.pkgPath)
    --fgui 组件
    self._uiComponent  = UIPackage.CreateObject(sceneDefine.pkgName, sceneDefine.componentName)
    self._sceneDefine  = sceneDefine
    self.closeType     = CloseType.Close
    --子节点映射
    self.childMap      = {} 
    --控制器映射
    self.controllerMap = {}
end

--[[
    @desc: 打开场景
    author:nothing
    time:2020-08-19 21:37:04
    --@param: 打开时传递的参数 会 传递给onOpened方法
    @return:
]]
function FScene:open(param)
    FScene.FSceneRoot:AddChild(self._uiComponent)
    local parent = FScene.FSceneRoot
    self._uiComponent:SetSize(parent.width, parent.height)
    self._uiComponent:AddRelation(parent, RelationType.Size)
    self._uiComponent.displayObject.gameObject.name = self._uiComponent.displayObject.gameObject.name .. "(" .. self._sceneDefine.name .. ")"; 
    self:_buildChildMap()
    self:_buildControllerMap()
    self:_onOpened(param)
end


function FScene:_onOpened(param)
    self:onOpened(param)
end

--[[
    @desc: 场景打开后回调onOpen方法
    author:nothing
    time:2020-08-19 21:38:21
    --@param: 打开场景时传递的参数
    @return:
]]
function FScene:onOpened(param)
--给子类重写    
end

function FScene:close(ctype)
    if self.autoDestoryAtClosed then
        self._uiComponent:Dispose()
    else
        self._uiComponent:RemoveFromParent()
    end
    self.closeType = ctype
    self:_onClosed()
end

function FScene:_onClosed( )
    self:closed(self.closeType)
end

--[[
    @desc: 场景被关闭后回调
    author:nothing
    time:2020-08-19 21:35:06
    --@ctype: 关闭类型 有 Close Yes No 三种类型
    @return:
]]
function FScene:onClosed(ctype)
    
end

--[[
    @desc: 构建子节点映射 方便 获取子节点
    author:nothing
    time:2020-08-19 21:51:20
    @return:
]]
function FScene:_buildChildMap()
    print("_buildChildMap")
end

--[[
    @desc: 构建控制器映射 方便 获取控制器
    author:nothing
    time:2020-08-19 21:51:50
    @return:
]]
function FScene:_buildControllerMap()
    print("_buildControllerMap")
end

return FScene