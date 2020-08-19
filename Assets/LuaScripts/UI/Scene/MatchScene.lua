--[[
    author:nothing
    time:2020-08-19 20:12:39
]]

local MatchScene = class("MatchScene", FSceneBase)

function MatchScene:ctor(sceneDefine)
    MatchScene.super.ctor(self, sceneDefine)
end

function MatchScene:onOpen(param)
    print("比赛场 onOpen")
end

return MatchScene