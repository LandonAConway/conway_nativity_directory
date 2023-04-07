function table.containsKey(t, key)
    for k, _ in pairs(t) do
        if k == key then
            return true
        end
    end
    return false
end

function table.containsValue(t, value)
    for _, v in pairs(t) do
        if v == value then
            return true
        end
    end
    return false
end

function table.contains(t, element)
    if table.containsKey(t, element) then return true end
    if table.containsValue(t, element) then return true end
    return false
end

function table.count(t)
    local c = 0
    for _, _ in pairs(t) do c = c + 1 end
    return c
end

function table.where(t, func)
    local result = {}
    for k, v in pairs(t) do
        if func(k, v) then
            result[k] = v
        end
    end
    return result
end

function table.select(t, func, useKeys)
    local result = {}
    for k, v in pairs(t) do
        if useKeys then
            result[k] = func(k, v)
        else
            table.insert(result, func(k, v))
        end
    end
    return result
end

function table.first(t)
    return t[1]
end

function table.last(t)
    return t[table.count(t)]
end

function table.is_array(t)
    local result = true
    for k, v in pairs(t) do
        if type(k) ~= "number" then
            result = false
        end
    end
    return result
end