day = 15
include("utils.jl")
using Test

input = getProblemInputText()
sample = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7"

hash = str -> reduce((h, c) -> ((h + Int(c)) * 17) % 256, str, init=0)
@test hash("HASH") == 52

function solve1(line)
    sum(hash, split(line, ','))
end

check("Day $day.1 Sample",  () -> solve1(sample),  1320)
check("Day $day.1 Problem", () -> solve1(input), 513158)

# too high 513158

#solve2 = lines -> solve(lines)
#
#check("Day $day.2 Sample",  () -> solve2(sample),  2)
#check("Day $day.2 Problem", () -> solve2(input), 908)
