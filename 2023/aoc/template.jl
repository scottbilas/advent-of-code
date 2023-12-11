day =
include("utils.jl")

input = getProblemInput()

sample = getSampleLines("""
""")

function solve(lines)
end

solve1 = lines -> solve(lines)

check("Day $day.1 Sample",  () -> solve1(sample), 0)
check("Day $day.1 Problem", () -> solve1(input),  0)

#solve2 = lines -> solve(lines)
#
#check("Day $day.2 Sample",  () -> solve2(sample),  2)
#check("Day $day.2 Problem", () -> solve2(input), 908)
