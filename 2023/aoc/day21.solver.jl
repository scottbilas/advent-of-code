day = 21
include("utils.jl")

input = getProblemInput()

sample = getSampleLines("""
    ...........
    .....###.#.
    .###.##..#.
    ..#.#...#..
    ....#.#....
    .##..S####.
    .##..#...#.
    .......##..
    .##.#.####.
    .##..##.##.
    ...........
""")

function solve1(lines, steps)
    board = parseGrid(lines)[1]

    seen = Set()
    work = [(0, Tuple(findfirst(isequal('S'), board)))]
    total = 0

    while true
        step, pos = popfirst!(work)
        if step > steps
            break
        end

        if pos ∈ seen
            continue
        end
        if board[pos...] == '#'
            continue
        end

        push!(seen, pos)

        if step % 2 == 0
            board[pos...] = 'O'
            total += 1
        end

        push!(work, (step+1, pos .+ ( 1,  0)))
        push!(work, (step+1, pos .+ (-1,  0)))
        push!(work, (step+1, pos .+ ( 0,  1)))
        push!(work, (step+1, pos .+ ( 0, -1)))
    end

    total
end

check("Day $day.1 Sample",  () -> solve1(sample, 6),   16)
check("Day $day.1 Problem", () -> solve1(input, 64), 3758)

#solve2 = lines -> solve(lines)
#
#check("Day $day.2 Sample",  () -> solve2(sample),  2)
#check("Day $day.2 Problem", () -> solve2(input), 908)
