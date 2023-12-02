day = 2
include("utils.jl")
using DataFrames, DataFramesMeta

input = getProblemInput()

sample = [
    "Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green",
    "Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue",
    "Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red",
    "Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red",
    "Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green",
]

maxes = game -> @chain DataFrame(
    eachmatch(r"(\d+) (\w+)", game) .|>
    m -> m.captures |> c -> (color=c[2], count=parse(Int, c[1]))
    ) begin
        groupby(:color)
        combine(:count => maximum => :max)
        Dict(row.color => row.max for row in eachrow(_))
    end

valid1  = counts -> counts["red"] <= 12 && counts["green"] <= 13 && counts["blue"] <= 14
select1 = ((id, line),) -> line |> maxes |> valid1 ? id : 0
solve1  = lines -> enumerate(lines) .|> select1 |> sum

check("Day $day.1 Sample",  () -> solve1(sample),  8)
check("Day $day.1 Problem", () -> solve1(input), 1931)

solve2 = lines -> @chain lines begin
    @.maxes       # get maxes of each color for each game
    @.values      # just need the maxes
    reduce.(*, _) # multiply together to get cube power per game
    sum
end

check("Day $day.2 Sample",  () -> solve2(sample), 2286)
check("Day $day.2 Problem", () -> solve2(input), 83105)
