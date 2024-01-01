day = 17
include("utils.jl")

input = getProblemInput()

sample = getSampleLines("""
    2413432311323
    3215453535623
    3255245654254
    3446585845452
    4546657867536
    1438598798454
    4457876987766
    3637877979653
    4654967986887
    4564679986453
    1224686865563
    2546548887735
    4322674655533
""")

@enum Dir N E S W
struct Work pos; dir; repeat end
const MaxRepeat, Dirs = 3, 4

function solve1(lines)
    board, (cx, cy) = parseGrid(lines, c -> parse(Int, c))
    dist = [fill(typemax(Int), MaxRepeat*Dirs) for _ in 1:cx, _ in 1:cy]
    work = [Work((1, 1), E, 0)]

    fill!(dist[1, 1], 0)

    function enter(src, dstDir, dstPos)
        dstRepeat = src.dir == dstDir ? src.repeat + 1 : 1
        if dstRepeat > MaxRepeat return end

        srcIndex = Int(src.dir)*MaxRepeat + src.repeat
        dstIndex = Int(dstDir)*MaxRepeat + dstRepeat

        cost = dist[src.pos...][srcIndex] + board[dstPos...]
        if cost < dist[dstPos...][dstIndex]
            dist[dstPos...][dstIndex] = cost
            push!(work, Work(dstPos, dstDir, dstRepeat))
        end
    end

    while !isempty(work)
        src = popfirst!(work)
        x, y = src.pos
        if x < cx && src.dir != W enter(src, E, (x+1, y)) end
        if y < cy && src.dir != N enter(src, S, (x, y+1)) end
        if x >  1 && src.dir != E enter(src, W, (x-1, y)) end
        if y >  1 && src.dir != S enter(src, N, (x, y-1)) end
    end

    minimum(dist[end])
end

check("Day $day.1 Sample",  () -> solve1(sample), 102)
check("Day $day.1 Problem", () -> solve1(input),  963)

#solve2 = lines -> solve(lines)
#
#check("Day $day.2 Sample",  () -> solve2(sample),  2)
#check("Day $day.2 Problem", () -> solve2(input), 908)
