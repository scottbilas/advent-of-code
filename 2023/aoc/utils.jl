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
    readlines("day$day.input.txt")
end

function getProblemInputText()
    strip(read("day$day.input.txt", String))
end

function getProblemSample()
    readlines("day$day.sample.txt")
end

function getSampleLines(text)
    map(strip, split(strip(text), '\n'))
end

function parseBlocks(lines)
    blocks = []
    block = []
    for line in lines
        if isempty(line)
            push!(blocks, block)
            block = []
        else
            push!(block, line)
        end
    end
    if !isempty(block)
        push!(blocks, block)
    end
    blocks
end

function slice_s(arr, range)
    arr[range_s(arr, range)]
end

function range_s(arr, range)
    start = max(range.start, 1)
    stop = min(range.stop, length(arr))
    start:stop
end

function validIndex(arr, index)
    1 <= index <= length(arr)
end

function element_s(arr, index, default)
    validIndex(arr, index) ? arr[index] : default
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
    result
end

function parseInts(text)
    map(m -> parse(Int, m.match), eachmatch(r"-?\d+", text))
end

function parseGrid(lines, convert = identity)
    board = [convert(lines[y][x]) for x in eachindex(lines[1]), y in eachindex(lines)]
    board, size(board)
end

function wrapIndex(arr, index)
    (index-1) % length(arr) + 1
end

function printGrid(grid, padCell = 0)
    cols, rows = size(grid)
    for y in 1:rows
        for x in 1:cols
            print(lpad(grid[x, y], padCell, ' '))
        end
        println()
    end
end
