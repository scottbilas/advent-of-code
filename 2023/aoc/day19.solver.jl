day = 19
include("utils.jl")
using Match

input = getProblemInput()

sample = getSampleLines("""
    px{a<2006:qkq,m>2090:A,rfg}
    pv{a>1716:R,A}
    lnx{m>1548:A,A}
    rfg{s<537:gd,x>2440:R,A}
    qs{s>3448:A,lnx}
    qkq{x<1416:A,crn}
    crn{x>2662:A,R}
    in{s<1351:px,qqz}
    qqz{s>2770:qs,m<1801:hdj,R}
    gd{a>3333:R,R}
    hdj{m>838:A,pv}

    {x=787,m=2655,a=1222,s=2876}
    {x=1679,m=44,a=2067,s=496}
    {x=2036,m=264,a=79,s=2244}
    {x=2461,m=1339,a=466,s=291}
    {x=2127,m=1623,a=2188,s=1013}
""")

function solve1(lines)
    ruleLines, shapeLines = parseBlocks(lines)

    rules = Dict()
    total = 0

    for ruleLine in ruleLines
        name, exprs = match(r"(\w+){([^}]+)}", ruleLine)
        rules[name] = exprs
    end

    for shapeLine in shapeLines
        x, m, a, s = parseInts(shapeLine)

        rule = rules["in"]
        while true

            result=""
            for expr in split(rule, ',')
                if length(expr) == 1 || !occursin(expr[2], "<>")
                    result = expr
                    break
                end
                var = @match expr[1] begin
                    'x' => x; 'm' => m
                    'a' => a; 's' => s
                end
                test = parseInts(expr)[1]

                if @match expr[2] begin
                        '<' => var < test
                        '>' => var > test
                    end
                    result = split(expr, ':')[2]
                    break
                end
            end

            if result == "A"
                total += x+m+a+s
                break
            elseif result == "R"
                break
            else
                rule = rules[result]
            end
        end
    end

    total
end

check("Day $day.1 Sample",  () -> solve1(sample),  19114)
check("Day $day.1 Problem", () -> solve1(input),  382440)

#solve2 = lines -> solve(lines)
#
#check("Day $day.2 Sample",  () -> solve2(sample),  2)
#check("Day $day.2 Problem", () -> solve2(input), 908)
