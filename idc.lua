function Stdsort(ids)
    local minval = 1;
    for i, _ in ipairs(ids) do
        for j, id in ipairs(ids) do
            if id > minval then
                minval = j
            end
        end
        
    end
end