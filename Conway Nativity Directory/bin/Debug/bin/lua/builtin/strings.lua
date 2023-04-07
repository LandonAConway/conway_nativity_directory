function string.split(self, sep)
    if sep == nil then
            sep = "%s"
    end
    local t={}
    for str in self.gmatch(self, "([^"..sep.."]+)") do
            table.insert(t, str)
    end
    return t
end

function string.join(t)
    local result = ""
    for _, v in pairs(t) do
        if type(v) == "string" then
            result = result .. v
        end
    end
    return result
end

function string.startswith(self, prefix)
    return (self:sub(1,#prefix) == prefix)
end

function string.endswith(self, suffix)
    return (self:sub(0-#suffix) == suffix)
end

function string.replace(self, old, new)
    return self:gsub(old, new)
end