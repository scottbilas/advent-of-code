day = 11
include("utils.jl")
using Chain

input = getProblemInput()

sample = getSampleLines("""
    ...#......
    .......#..
    #.........
    ..........
    ......#...
    .#........
    .........#
    ..........
    .......#..
    #...#.....
""")

function solve(lines, expand)
    # axis-coord -> galaxy id
    makeaxis = len -> [[i, []] for i in 1:len]
    id, xaxis, yaxis = 0, makeaxis(length(lines[1])), makeaxis(length(lines))

    for (y, row) in enumerate(lines), (x, cell) in enumerate(row)
        if cell == '#'
            id += 1
            push!(xaxis[x][2], id)
            push!(yaxis[y][2], id)
        end
    end

    for axis in [xaxis, yaxis], (i, value) in enumerate(axis)
        if isempty(value[2])
            for next in axis[i+1:end]
                next[1] += expand-1
            end
        end
    end

    all = [[0, 0] for _ in 1:id]
    for col in xaxis, id in col[2] all[id][1] = col[1] end
    for row in yaxis, id in row[2] all[id][2] = row[1] end

    total = 0
    for src in eachindex(all), dst in src+1:length(all)
        total += abs(all[src][1] - all[dst][1]) + abs(all[src][2] - all[dst][2])
    end
    total
end

check("Day $day.1 Sample",  () -> solve(sample, 2),     374)
check("Day $day.1 Problem", () -> solve(input,  2), 9734203)

check("Day $day.2 Sample.10",  () -> solve(sample,     10),         1030)
check("Day $day.2 Sample.100", () -> solve(sample,    100),         8410)
check("Day $day.2 Problem",    () -> solve(input, 1000000), 568914596391)
