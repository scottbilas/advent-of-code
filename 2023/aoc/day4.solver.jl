day = 4
include("utils.jl")
using Chain

input = getProblemInput()

sample = getSampleLines("""
    Card 1: 41 48 83 86 17 | 83 86  6 31 17  9 48 53
    Card 2: 13 32 20 16 61 | 61 30 68 82 17 32 24 19
    Card 3:  1 21 53 59 44 | 69 82 63 72 16 21 14  1
    Card 4: 41 92 73 84 69 | 59 84 76 51 58  5 54 83
    Card 5: 87 83 26 28 32 | 88 30 70 12 93 22 82 36
    Card 6: 31 18 13 56 72 | 74 77 10 23 35 67 36 11
""")

function wins(lines)
    nums = t -> @chain t eachmatch(r"\d+", _) map(m -> parse(Int, m.match), _) Set
    card = c -> @chain split(c, ":")[2] split("|") (@.nums)
    lines .|> card .|> c -> length(c[1] ∩ c[2])
end

solve1 = lines -> @chain lines (@.wins) map.(w -> div(2^w, 2), _) sum

check("Day $day.1 Sample",  () -> solve1(sample),   13)
check("Day $day.1 Problem", () -> solve1(input), 25651)

function solve2(lines)
    make, work = wins(lines), ones(Int, length(lines))
    for i in eachindex(work), j in 1:make[i]
        work[i+j] += work[i]
    end
    work |> sum
end

check("Day $day.2 Sample",  () -> solve2(sample),      30)
check("Day $day.2 Problem", () -> solve2(input), 19499881)
