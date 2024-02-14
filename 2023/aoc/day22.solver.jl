day = 22
include("utils.jl")
using Chain, DataStructures

input = getProblemInput()

sample = getSampleLines("""
    1,0,1~1,2,1
    0,0,2~2,0,2
    0,2,3~2,2,3
    0,0,4~0,2,4
    2,0,5~2,2,5
    0,1,6~2,1,6
    1,1,8~1,1,9
""")

function settle(lines)
    bricks, up = sort(parseInts.(lines), by=b->b[3]), DefaultDict(()->[])
    for (ib, (x0, y0, z0, x1, y1, z1)) in enumerate(bricks)
        top = 0
        for il in ib-1:-1:1
            (lx0, ly0, _, lx1, ly1, lz1) = bricks[il]
            if x1 >= lx0 && x0 <= lx1 && y1 >= ly0 && y0 <= ly1
                push!(up[il], ib)
                top = max(top, lz1)
            end
        end
        bricks[ib][3] = top+1
        bricks[ib][6] = top+1 + z1-z0
    end

    down = DefaultDict(()->[])
    for (from, to) in up
        up[from] = filter(v -> bricks[v][3] == bricks[from][6]+1, to) # prune
        for it in up[from]
            push!(down[it], from)
        end
    end

    bricks, up, down
end

function solve1(lines)
    bricks, up, down = settle(lines)
    count(
        ib -> all(iu -> length(down[iu]) > 1, up[ib]),
        1:length(bricks))
end

check("Day $day.1 Sample",  () -> solve1(sample),  5)
check("Day $day.1 Problem", () -> solve1(input), 391)

#solve2 = lines -> solve(lines)

#check("Day $day.2 Sample",  () -> solve2(sample),  2)
#check("Day $day.2 Problem", () -> solve2(input), 908)
