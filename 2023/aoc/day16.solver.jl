day = 16
include("utils.jl")
using Match

input = getProblemInput()

sample = getSampleLines(raw"""
    .|...\....
    |.-.\.....
    .....|-...
    ........|.
    ..........
    .........\
    ..../.\\..
    .-.-/..|..
    .|....-|.\
    ..//.|....
""")

@enum N E S W
nextStep = Dict([ (N, [0, -1]), (E, [1, 0]), (S, [0, 1]), (W, [-1, 0]) ])

### OPTIMIZE: record for each cell when it has a beam enter it and from what
# direction. when a new beam comes along that has already been seen from that
# direction, just kill it.

### SOLVER: not great to have to keep repeating multiple times looking for diffeences.
# instead, iterate on beams one at a time, running each until it is no longer needed.

function solve1(lines)
    board, (cx, cy) = parseGrid(lines)

    beams = []
    push!(beams, (E, [0, 1]))

    used = Set()
    total = 0

    while true
        i = 1
        while i <= length(beams)
            dir, pos = beams[i]
            pos += nextStep[dir]

            if pos[1] < 1 || pos[1] > cx || pos[2] < 1 || pos[2] > cy
                splice!(beams, i)
                continue
            end

            if board[pos...] == '/'
                dir = @match dir begin
                    $N => E; $E => N; $S => W; $W => S
                end
            elseif board[pos...] == '\\'
                dir = @match dir begin
                    $N => W; $E => S; $S => E; $W => N
                end
            elseif board[pos...] == '|'
                if dir == W || dir == E
                    dir = N
                    push!(beams, (S, pos))
                end
            elseif board[pos...] == '-'
                if dir == N || dir == S
                    dir = E
                    push!(beams, (W, pos))
                end
            end

            push!(used, pos)
            beams[i] = dir, pos
            i += 1
        end

        #if length(used) == total
        #    break
        #end
        total = length(used)
        print("$total ")
    end
    total

    for (x, y) in used
        board[x, y] = '#'
    end

    printGrid(board)

    total
end

#check("Day $day.1 Sample",  () -> solve1(sample), 46)
check("Day $day.1 Problem", () -> solve1(input), 8021)

# too low: 7891, 7892

#solve2 = lines -> solve(lines)
#
#check("Day $day.2 Sample",  () -> solve2(sample),  2)
#check("Day $day.2 Problem", () -> solve2(input), 908)
