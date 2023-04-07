local function getInput()
    local label = LabelElement("How many nativities would you like to add?")
    local textBox = TextBoxElement("Auto", 22)
    local button = ButtonElement("Ok", 100, 22)
    local stackPanel = StackPanelElement()
    local form = CndForm("Bulk Add Nativities", 400, 135)
    label:SetMargin(0, 0, 0, 7)
    label:SetHorizontalAlignment("Center")
    textBox:SetMargin(0, 0, 0, 7)
    button:SetHorizontalAlignment("Center")
    stackPanel:SetMargin(10, 10, 10, 10)
    stackPanel:AddElement("label1", label)
    stackPanel:AddElement("textBox1", textBox)
    stackPanel:AddElement("button1", button)
    form:SetResizeMode("NoResize")
    form:SetContent(stackPanel)
    --behavior
    form.OnRendered = function()
        textBox:ForceFocus()
    end
    local confirm = false
    button.OnClick = function()
        confirm = true
        form:Close()
    end
    textBox.PreviewKeyDown = function(sender, key)
        if key == "Return" then       
            confirm = true
            form:Close()
            return true
        end
    end
    form:ShowDialog()
    if confirm then
        return textBox:GetText()
    end
end

local function generateNativities(n)
    local list = cnd.get_nativities()
    table.sort(list, function(a, b) return a:GetId() < b:GetId() end)
    local last = table.last(list)
    local lastId = 0
    if last then lastId = last:GetId() end
    for i = 1, n, 1 do
        local id = lastId + i
        local title = "Nativity "..id
        local nativity = NativityObj()
        nativity:SetId(id)
        nativity:SetTitle(title)
        cnd.add_nativity(nativity)
    end
    last = table.last(cnd.get_nativities())
    last:ScrollIntoView()
end

--warn users if they entered a number greater than 'max'
local max = 100
local function run()
    if cnd.is_project_open() then
        local input = getInput()
        if input then
            local n = tonumber(input)
            if n and n > 0 then
                if n > max then
                    local mbr = message("Generating more than "..max.." nativities can take a long time. Would you like to continue?",
                                    "Bulk Add Nativities", "YesNoCancel", "Warning")
                    if mbr == "Yes" then
                        generateNativities(n)
                    elseif mbr == "No" then
                        run()
                    end
                else
                    generateNativities(n)
                end
            else
                message("The input must be a number greater than zero.")
                run()
            end
        end
    else
        message("A project must be open to perform this operation.")
    end
end

run()