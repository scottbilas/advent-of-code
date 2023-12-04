using Printf

function prettyMs(seconds)
    if seconds < 1.0
        @sprintf("%.0fms", seconds*1000)
    else
        @sprintf("%.2fs", seconds)
    end
end

function check(name, solver, expected)
    start = time()
    result = solver()
    finish = time()

    if result == expected
        println("√ $name ($(prettyMs(finish - start)))")
    else
        println("× $name ($(prettyMs(finish - start)))")
        println("   Result: $result")
        println(" Expected: $expected")
    end
end

function getProblemInput()
    return readlines("day$day.input.txt")
end

function getSampleLines(text)
    return map(strip, split(strip(text), '\n'))
end

function slice_s(arr, range)
    return arr[range_s(arr, range)]
end

function range_s(arr, range)
    start = max(range.start, 1)
    stop = min(range.stop, length(arr))
    return start:stop
end

function validindex(arr, index)
    return 1 <= index <= length(arr)
end

function flatten(arr)
    result = []
    for item in arr
        if isa(item, AbstractArray)
            append!(result, flatten(item))
        else
            push!(result, item)
        end
    end
    return result
end
