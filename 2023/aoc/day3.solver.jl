day = 3
include("utils.jl")
using Chain, DataStructures

input = getProblemInput()

sample = getSampleLines(raw"""
    467..114..
    ...*......
    ..35..633.
    ......#...
    617*......
    .....+.58.
    ..592.....
    ......755.
    ...$.*....
    .664.598..
""")

function solve1(lines)
    near = (sx, dx, sy) -> any(
        slice_s(lines, sy-1:sy+1) .|>
        line -> occursin(r"[^0-9.]", slice_s(line, sx-1:sx+dx)))
    nums = eachindex(lines) .|>
        y -> eachmatch(r"\d+", lines[y]) .|>
        m -> near(m.offset, length(m.match), y) ? parse(Int, m.match) : 0
    nums |> flatten |> sum
end

check("Day $day.1 Sample",  () -> solve1(sample),  4361)
check("Day $day.1 Problem", () -> solve1(input), 528819)

function solve2(lines)

    function gear(sx, dx, sy)
        coords = []
        for y in range_s(lines, sy-1:sy+1)
            l = lines[y]
            r = range_s(l, sx-1:sx+dx)
            for m in eachmatch(r"\*", l[r])
                push!(coords, (m.offset+r.start-1, y))
            end
        end
        coords
    end

    gears = DefaultDict([])

    for y in eachindex(lines),
        m in eachmatch(r"\d+", lines[y]),
        c in gear(m.offset, length(m.match), y)
        gears[c] = vcat(gears[c], parse(Int, m.match))
    end

    (gears |> values .|> n -> length(n) == 2 ? prod(n) : 0) |> sum
end

check("Day $day.2 Sample",  () -> solve2(sample),  467835)
check("Day $day.2 Problem", () -> solve2(input), 80403602)
