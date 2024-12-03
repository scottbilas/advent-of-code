day = 23
include("utils.jl")

input = getProblemInput()

sample = getSampleLines("""
    #.#####################
    #.......#########...###
    #######.#########.#.###
    ###.....#.>.>.###.#.###
    ###v#####.#v#.###.#.###
    ###.>...#.#.#.....#...#
    ###v###.#.#.#########.#
    ###...#.#.#.......#...#
    #####.#.#.#######.#.###
    #.....#.#.#.......#...#
    #.#####.#.#.#########v#
    #.#...#...#...###...>.#
    #.#.#v#######v###.###v#
    #...#.>.#...>.>.#.###.#
    #####v#.#.###v#.#.###.#
    #.....#...#...#.#.#...#
    #.#########.###.#.#.###
    #...###...#...#...#.###
    ###.###.#.###v#####v###
    #...#...#.#.>.>.#.>.###
    #.###.###.#.###.#.#v###
    #.....###...###...#...#
    #####################.#
    """)

struct Work pos; dir; dist end
XY = xy -> CartesianIndex(xy[1], xy[2])
N,E,S,W = XY(0,-1), XY(1,0), XY(0,1), XY(-1,0)
Dirs = [N, E, S, W]

function solve1(lines)
    grid, _ = parseGrid(lines)

    start = findfirst(isequal('.'), grid)
    stop = findlast(isequal('.'), grid)

    todo = [Work(start, S, 0)]
    while length(todo)
        work = pop!(todo)

        while grid[work.pos+work.dir] != '#'
            work.pos += work.dir
            work.dist += 1
        end

        for dir in Dirs
            if dir != work.dir
                push!(todo, Work(work.pos, dir, work.dist))
            end
        end
    end
end

check("Day $day.1 Sample",  () -> solve1(sample), 94)
#check("Day $day.1 Problem", () -> solve1(input), -1)

#solve2 = lines -> solve(lines)
#
#check("Day $day.2 Sample",  () -> solve2(sample),  2)
#check("Day $day.2 Problem", () -> solve2(input), 908)
