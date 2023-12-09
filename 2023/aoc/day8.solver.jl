day = 8
include("utils.jl")
using Chain

input = getProblemInput()

sample1 = getSampleLines("""
    RL

    AAA = (BBB, CCC)
    BBB = (DDD, EEE)
    CCC = (ZZZ, GGG)
    DDD = (DDD, DDD)
    EEE = (EEE, EEE)
    GGG = (GGG, GGG)
    ZZZ = (ZZZ, ZZZ)
""")

sample2 = getSampleLines("""
    LLR

    AAA = (BBB, BBB)
    BBB = (AAA, ZZZ)
    ZZZ = (ZZZ, ZZZ)
""")

function parseit(lines)
    rules = Dict()
    for line in lines[3:end]
        src, dstl, dstr = @chain line eachmatch(r"\w+", _) map(m -> m.match, _)
        rules[src] = (dstl, dstr)
    end

    lines[1], rules
end

function solve1(lines)
    instrs, rules = parseit(lines)

    pos = "AAA"
    steps = 0

    while pos != "ZZZ"
        rot = instrs[wrapIndex(instrs, steps+1)] == 'L' ? 1 : 2
        pos = rules[pos][rot]
        steps += 1
    end

    steps
end

check("Day $day.1 Sample 1", () -> solve1(sample1),   2)
check("Day $day.1 Sample 2", () -> solve1(sample2),   6)
check("Day $day.1 Problem",  () -> solve1(input), 18023)

sample3 = getSampleLines("""
    LR

    11A = (11B, XXX)
    11B = (XXX, 11Z)
    11Z = (11B, XXX)
    22A = (22B, XXX)
    22B = (22C, 22C)
    22C = (22Z, 22Z)
    22Z = (22B, 22B)
    XXX = (XXX, XXX)
""")

function solve2(lines)
    instrs, rules = parseit(lines)

    result = 1

    for pos in @chain rules keys filter(r -> r[3] == 'A', _)
        steps = 0

        while pos[3] != 'Z'
            rot = instrs[wrapIndex(instrs, steps+1)] == 'L' ? 1 : 2
            pos = rules[pos][rot]
            steps += 1
        end

        result = lcm(result, steps)
    end

    result
end


check("Day $day.2 Sample",  () -> solve2(sample3),            6)
check("Day $day.2 Problem", () -> solve2(input), 14449445933179)
