day = 6
include("utils.jl")
using Chain

input = getProblemInput()

sample = getSampleLines("""
    Time:      7  15   30
    Distance:  9  40  200
""")

solve1 = lines -> @chain lines begin
    zip(parseInts(_[1]), parseInts(_[2]))
    map(((t, d),) -> count(h -> h*(t-h) > d, 1:t), _)
    prod
end

check("Day $day.1 Sample",  () -> solve1(sample),   288)
check("Day $day.1 Problem", () -> solve1(input), 449820)

solve2 = lines -> replace.(lines, " "=>"") |> solve1

check("Day $day.2 Sample",  () -> solve2(sample),   71503)
check("Day $day.2 Problem", () -> solve2(input), 42250895)
