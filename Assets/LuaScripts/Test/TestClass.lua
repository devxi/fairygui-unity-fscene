--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2020-08-19 11:20:23
]]

local TestClass = class("TestClass")


function TestClass:ctor(  )
    print "TestClass ctor"
    self._name = "测试testClass";
end

function TestClass:funA(  )
    print "基类的方法 funA"
end


return TestClass