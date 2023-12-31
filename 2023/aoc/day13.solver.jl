day = 13
include("utils.jl")

input = getProblemInput() |> parseBlocks

sample = getSampleLines("""
    #.##..##.
    ..#.##.#.
    ##......#
    ##......#
    ..#.##.#.
    ..##..##.
    #.#.##.#.

    #...##..#
    #....#..#
    ..##..###
    #####.##.
    #####.##.
    ..##..###
    #....#..#
""") |> parseBlocks

function solve1(blocks)
    mirror = (arr, start) -> all(
        i -> arr[start-i+1] == arr[start+i],
        1:min(start, length(arr)-start))

    div = arr -> something(
        findfirst(i -> mirror(arr, i), 1:length(arr)-1), 0)

    total = 0
    for rows in blocks
        cols = [[] for _ in 1:length(rows[1])]
        for row in rows, (i, c) in enumerate(row)
            push!(cols[i], c)
        end
        cols = [join(col) for col in cols]

        total += div(rows)*100 + div(cols)
    end
    total
end

check("Day $day.1 Sample",  () -> solve1(sample),   405)
check("Day $day.1 Problem", () -> solve1(input),  36448)

#solve2 = lines -> solve(lines)
#
#check("Day $day.2 Sample",  () -> solve2(sample),  2)
#check("Day $day.2 Problem", () -> solve2(input), 908)
