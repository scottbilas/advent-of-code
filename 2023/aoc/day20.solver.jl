day = 20
include("utils.jl")
using JSON3, Match

input = getProblemInput()

sample1 = getSampleLines("""
    broadcaster -> a, b, c
    %a -> b
    %b -> c
    %c -> inv
    &inv -> a
""")

sample2 = getSampleLines("""
    broadcaster -> a
    %a -> inv, con
    &inv -> b
    %b -> con
    &con -> output
""")

mutable struct FlipFlop state; outs; end
mutable struct Conjunction ins; states; outs; end
struct Broadcast outs; end

function solve1(lines)

    nodes = Dict()

    for line in lines
        name, links... = map(l->l.match, eachmatch(r"[%&a-z]+", line))
        name, node = @match name[1] begin
            '%' => (name[2:end], FlipFlop(false, links))
            '&' => (name[2:end], Conjunction([], [], links))
            _   => (name,        Broadcast(links))
        end
        nodes[name] = node
    end

    nodes["button"] = Broadcast(["broadcaster"])

    for (name, node) in nodes
        for out in node.outs
            if haskey(nodes, out)
                dst = nodes[out]
                if dst isa Conjunction
                    push!(dst.ins, name)
                    push!(dst.states, false)
                end
            else
                nodes[out] = Broadcast([])
            end
        end
    end

    lows, highs = 0, 0

    function button()
        pulses = [("", "button", false)]

        while !isempty(pulses)
            src, dst, sig = popfirst!(pulses)

            function pulse(tgt, tsig)
                #println("$dst -$(tsig ? "high" : "low")-> $tgt")
                push!(pulses, (dst, tgt, tsig))
                if tsig
                    highs += 1
                else
                    lows += 1
                end
            end

            node = nodes[dst]
            if node isa FlipFlop
                if !sig
                    node.state = !node.state
                    pulse.(node.outs, node.state)
                end
            elseif node isa Conjunction
                idx = findfirst(isequal(src), node.ins)
                node.states[idx] = sig
                pulse.(node.outs, !all(node.states))
            else
                pulse.(node.outs, sig)
            end
        end
    end

    for _ in 1:1000
        button()
    end

    lows * highs
end

check("Day $day.1 Sample 1",  () -> solve1(sample1), 32000000)
check("Day $day.1 Sample 2",  () -> solve1(sample2), 11687500)
check("Day $day.1 Problem",   () -> solve1(input),  794930686)

#solve2 = lines -> solve(lines)
#
#check("Day $day.2 Sample",  () -> solve2(sample),  2)
#check("Day $day.2 Problem", () -> solve2(input), 908)
