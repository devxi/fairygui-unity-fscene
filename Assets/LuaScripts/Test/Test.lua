--[[
    author:nothing
    time:2020-08-19 19:53:48
]]



UIPackage.AddPackage("UI/PublicPackage00");
UIPackage.AddPackage("UI/PublicPackage01");
UIPackage.AddPackage("UI/MatchPackage");

local sceneDefine = require ("UI.Config.FSceneDefine")

-- local com = UIPackage.CreateObject(sceneDefine.pkgName, sceneDefine.componentName)

-- GRoot.inst:AddChild(com)
local FScene = require("Framework.UI.FScene")
local MatchScene = require("UI.Scene.MatchScene") 
local s = MatchScene.new(sceneDefine)
s:open()
-- UI\Config\Scene\MatchScene.lua