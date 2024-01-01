day = 18
include("utils.jl")
using Match

input = getProblemInput()

sample = getSampleLines("""
    R 6 (#70c710)
    D 5 (#0dc571)
    L 2 (#5713f0)
    D 2 (#d2c081)
    R 2 (#59c680)
    D 2 (#411b91)
    L 5 (#8ceee2)
    U 2 (#caa173)
    L 1 (#1b58a2)
    U 2 (#caa171)
    R 2 (#7807d2)
    U 3 (#a77fa3)
    L 2 (#015232)
    U 2 (#7a21e3)
""")

function solve1(lines)
    trench = Set()

    pos = (1, 1)
    push!(trench, pos)

    for line in lines
        dir, len = match(r"(\w) (\d+)", line).captures
        move = @match dir begin
            "U" => ( 0, -1)
            "D" => ( 0,  1)
            "L" => (-1,  0)
            "R" => ( 1,  0)
        end
        for _ in 1:parse(Int, len)
            pos = pos .+ move
            push!(trench, pos)
        end
    end

    minx = minimum(map(p -> p[1], collect(trench)))
    maxx = maximum(map(p -> p[1], collect(trench)))
    miny = minimum(map(p -> p[2], collect(trench)))
    maxy = maximum(map(p -> p[2], collect(trench)))

    text = fill('.', maxx - minx + 1, maxy - miny + 1)
    for i in trench
        text[i[1] - minx + 1, i[2] - miny + 1] = '#'
    end


#=
    #           #   #           #   #
    #           #   #           #   #
    #####   #####   #   #####   #####
        #   #       #   #   #
        #   #       #   #   #

=#

    total = length(trench)
    for y in miny:maxy
        inside, entered = false, nothing
        for x in minx:maxx
            if (x, y) ∈ trench
                up = (x, y-1) ∈ trench
                dn = (x, y+1) ∈ trench

                if up && dn
                    inside = !inside
                elseif up
                    if entered == 'v'
                        inside = !inside
                    else
                        entered = '^'
                    end
                elseif dn
                    if entered == '^'
                        inside = !inside
                    else
                        entered = 'v'
                    end
                end
            else
                entered = nothing
                if inside
                    total += 1
                    #text[x - minx + 1, y - miny + 1] = '*'
                end
            end
        end
    end

    if size(text) < (80, 80)
        printGrid(text)
    else
#        printGrid(text[1:80,1:80])
    end

    total
end

check("Day $day.1 Sample",  () -> solve1(sample),   62)
check("Day $day.1 Problem", () -> solve1(input), 92758)

#solve2 = lines -> solve(lines)
#
#check("Day $day.2 Sample",  () -> solve2(sample),  2)
#check("Day $day.2 Problem", () -> solve2(input), 908)
