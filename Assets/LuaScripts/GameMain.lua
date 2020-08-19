

print("\n********** \n".." Hello,World, This is Lua ".."\n**********")




GameMain = {}



local function init(  )
    require("Util.Util")
    require("Framework.3rd.FairyGUI.FairyGUI")
    require("Global.GlobalDefine")
end

local function test() 
    -- local TestClass = require "TestClass"
    -- local obj       = TestClass.new()
    -- print (obj._name);

    -- local TestClass2 = require "TestClass2"
    -- local obj2       = TestClass2.new()
    -- obj2: sum(2, 6)
    -- obj2: descript()
    -- obj2: funA()

    -- local sc = require "Framework.UI.FScene"
    require("Test.Test")

end

local function luaDebug(  )
    local breakSocketHandle,debugXpCall = require("LuaDebug")("localhost",7003)  --本机调试 win mac
end



function GameMain:Start()
    print("lua start")
    luaDebug()
    init()
    test()
end



-- GameMain.Main = Main;