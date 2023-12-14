day = 12
include("utils.jl")
using Chain, Combinatorics

input = getProblemInput()

sample = getSampleLines("""
    ???.### 1,1,3
    .??..??...?##. 1,1,3
    ?#?#?#?#?#?#?#? 1,3,1,6
    ????.#...#... 4,1,1
    ????.######..#####. 1,6,5
    ?###???????? 3,2,1
""")

function solve(line)

    cached = Dict()
    function cache(f, ispan, remain)
        key = ispan, length(remain)
        if !haskey(cached, key)
            cached[key] = f(ispan, remain)
        end
        return cached[key]
    end

    spans = @chain line eachmatch(r"\d+", _) map(m->parse(Int, m.match), _)

    function walk(ispan, remain)
        m = match(r"[#?]+", remain)
        if isnothing(m)
            return 0 # no more springs
        end

        remain = remain[m.offset:end]
        dstLen, srcLen = length(m.match), spans[ispan]
        ok = 0

        for i in 1:dstLen-srcLen+1
            next = remain[i+srcLen:end]
            if ispan < length(spans)
                if next[1] != '#' # spans must be separated
                    ok += cache(walk, ispan+1, next[2:end])
                end
            elseif !contains(next, '#') # can't have any springs unused
                ok += 1
            end

            # iterating would leave this required spring behind
            if remain[i] == '#'
                break
            end
        end

        # try next area if this one has no required springs
        if !contains(remain[1:dstLen], '#')
            ok += cache(walk, ispan, remain[dstLen+1:end])
        end

        ok
    end

    walk(1, line)
end

solve1 = lines -> solve.(lines) |> sum

check("Day $day.1 Sample",  () -> solve1(sample), 21)
check("Day $day.1 Problem", () -> solve1(input),  7674)

solve2 = lines -> @chain lines map(v ->
    join([
        join(repeat([split(v)[1]], 5), '?')
        join(repeat([split(v)[2]], 5), ',')
    ], ' '), _) solve1

check("Day $day.2 Sample",  () -> solve2(sample),       525152)
check("Day $day.2 Problem", () -> solve2(input), 4443895258186)
