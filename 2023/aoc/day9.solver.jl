day = 9
include("utils.jl")

input = getProblemInput()

sample = getSampleLines("""
    0 3 6 9 12 15
    1 3 6 10 15 21
    10 13 16 21 30 45
""")

function setup(line)
    nums = []
    push!(nums, parseInts(line))

    while sum(nums[end]) != 0
        push!(nums, [])
        for i in 1:length(nums[end-1])-1
            push!(nums[end], nums[end-1][i+1] - nums[end-1][i])
        end
    end
    nums
end

function solve(lines, push, get, op)
    total = 0
    for nums in setup.(lines)
        push(nums[end], 0)
        for i in length(nums)-1:-1:1
            push(nums[i], op(get(nums[i]), get(nums[i+1])))
        end
        total += get(nums[begin])
    end
    total
end

solve1 = lines -> solve(lines, push!, a -> a[end], +)

check("Day $day.1 Sample",  () -> solve1(sample),       114)
check("Day $day.1 Problem", () -> solve1(input), 1637452029)

solve2 = lines -> solve(lines, pushfirst!, a -> a[begin], -)

check("Day $day.2 Sample",  () -> solve2(sample),  2)
check("Day $day.2 Problem", () -> solve2(input), 908)
