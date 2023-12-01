day = 1
include("utils.jl")

input = getProblemInput()

sample1 = [
    "1abc2",
    "pqr3stu8vwx",
    "a1b2c3d4e5f",
    "treb7uchet",
]

solve1 = lines ->
    @pipe lines |>
    map(l -> replace(l, r"\D+" => ""), _) |>
    map(l -> parse(Int, l[begin]*l[end]), _) |>
    sum

check("Day $day.1 Sample",  () -> solve1(sample1), 142)
check("Day $day.1 Problem", () -> solve1(input), 54953)

sample2 = [
    "two1nine",
    "eightwothree",
    "abcone2threexyz",
    "xtwone3four",
    "4nineeightseven2",
    "zoneight234",
    "7pqrstsixteen",
]

function solve2(lines)
    words = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"]

    function fixup(line)
        c = 1
        while c <= length(line)
            for (num, word) in enumerate(words)
                if startswith(line[c:end], word)
                    line = string(line[begin:c-1], num, line[c:end])
                    c += 1
                end
            end
            c += 1
        end
        return line
    end

    return @pipe lines |> map(fixup, _) |> solve1
end

check("Day $day.2 Sample",  () -> solve2(sample2), 281)
check("Day $day.2 Problem", () -> solve2(input), 53868)
