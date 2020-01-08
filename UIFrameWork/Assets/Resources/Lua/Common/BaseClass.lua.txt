local Base = class("BaseClass")

function Base:PrintBase()
    print ("base print")
    self.b = 5
end 

return Base 