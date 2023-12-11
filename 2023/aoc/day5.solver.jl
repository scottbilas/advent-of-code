day = 5
include("utils.jl")
using Chain

struct Instr srcStart; srcEnd; dstStart end

function solve(lines, seedRanges)
    sections = []

    for line in lines[3:end]
        if contains(line, "map")
            push!(sections, [])
        end
        instr = parseInts(line)
        if !isempty(instr)
            push!(sections[end], Instr(instr[2], instr[2] + instr[3], instr[1]))
        end
    end

    jobs = [[] for _ in 1:length(sections)+1]
    jobs[1] = seedRanges

    for isection in eachindex(sections)
        while !isempty(jobs[isection])
            jobStart, jobEnd = pop!(jobs[isection])
            for instr in sections[isection]
                if jobStart >= instr.srcEnd || jobEnd <= instr.srcStart
                    continue
                end
                if jobStart < instr.srcStart
                    push!(jobs[isection], (jobStart, instr.srcStart))
                    jobStart = instr.srcStart
                end
                if jobEnd > instr.srcEnd
                    push!(jobs[isection], (instr.srcEnd, jobEnd))
                    jobEnd = instr.srcEnd
                end

                offset = instr.dstStart - instr.srcStart
                jobStart += offset
                jobEnd += offset
                break
            end

            push!(jobs[isection+1], (jobStart, jobEnd))
        end
    end

    minimum(map(x -> x[1], jobs[end]))
end

input = getProblemInput()
sample = getProblemSample()

solve1 = lines -> @chain lines[1] begin
    parseInts
    map(v -> (v[1], v[1]+1), _)
    solve(lines, _)
end

check("Day $day.1 Sample",  () -> solve1(sample),       35)
check("Day $day.1 Problem", () -> solve1(input), 199602917)

solve2 = lines -> @chain lines[1] begin
    parseInts
    zip(_[1:2:end], _[2:2:end])
    map(v -> (v[1], v[1]+v[2]), _)
    solve(lines, _)
end

check("Day $day.2 Sample",  () -> solve2(sample),      46)
check("Day $day.2 Problem", () -> solve2(input),  2254686)
