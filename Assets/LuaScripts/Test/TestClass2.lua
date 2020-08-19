--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2020-08-19 11:30:47
]]


local TestClass = require ("TestClass")
local TestClass2 = class("TestClass2", TestClass)

function TestClass2:ctor(  )
    TestClass2.super.ctor(self)
    self._id = "fuck"
    print("TestClass2  ctor")
end

function TestClass2:sum( x, y )
    print("sum->" .. (x + y) .. "访问继承的父类字段 _name" .. self._name)
end

function TestClass2:descript( )
   for k,v in pairs(self) do
       print(k, v);
   end
end


function TestClass2:funA( )
    TestClass2.super:funA()
    print "派生类的方法 funA"
end

return TestClass2;