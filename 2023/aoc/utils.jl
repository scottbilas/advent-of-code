using Pipe, Printf

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
