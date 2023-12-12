day = 10
include("utils.jl")
using Chain, Match, Test

input = getProblemInput()

sample1 = getSampleLines("""
    -L|F7
    7S-7|
    L|7||
    -L-J|
    L|-JF
""")

sample2 = getSampleLines("""
    7-F7-
    .FJ|7
    SJLL7
    |F--J
    LJ.LJ
""")

@enum Dir N E S W

function setup(lines)
    board = @chain lines join(_, "") collect
    length(lines[1]), findfirst(c -> c == 'S', board), board
end

function solve(lines)
    cx, start, board = setup(lines)

    # all samples/input connect south from start :)
    pos, dir = start+cx, S
    while pos != start
        newPos, dir = @match (board[pos], dir) begin
            ('|', $S) => (pos + cx, S); ('J', $S) => (pos -  1, W)
            ('|', $N) => (pos - cx, N); ('J', $E) => (pos - cx, N)
            ('-', $E) => (pos +  1, E); ('7', $E) => (pos + cx, S)
            ('-', $W) => (pos -  1, W); ('7', $N) => (pos -  1, W)
            ('L', $S) => (pos +  1, E); ('F', $W) => (pos + cx, S)
            ('L', $W) => (pos - cx, N); ('F', $N) => (pos +  1, E)
        end
        board[pos] = 'S'
        pos = newPos
    end
    board
end

solve1 = lines -> @chain lines solve count(v -> v == 'S', _)÷2

check("Day $day.1 Sample",  () -> solve1(sample1),   4)
check("Day $day.1 Sample",  () -> solve1(sample2),   8)
check("Day $day.1 Problem", () -> solve1(input),  6738)

sample3 = getSampleLines("""
    ...........
    .S-------7.
    .|F-----7|.
    .||.....||.
    .||.....||.
    .|L-7.F-J|.
    .|..|.|..|.
    .L--J.L--J.
    ...........
""")

sample4 = getSampleLines("""
    .F----7F7F7F7F-7....
    .|F--7||||||||FJ....
    .||.FJ||||||||L7....
    FJL7L7LJLJ||LJ.L-7..
    L--J.L7...LJS7F-7L7.
    ....F-J..F7FJ|L7L7L7
    ....L7.F7||L7|.L7L7|
    .....|FJLJ|FJ|F7|.LJ
    ....FJL-7.||.||||...
    ....L---J.LJ.LJLJ...
""")

sample5 = getSampleLines("""
    FF7FSF7F7F7F7F7F---7
    L|LJ||||||||||||F--J
    FL-7LJLJ||||||LJL-77
    F--JF--7||LJLJ7F7FJ-
    L---JF-JLJ.||-FJLJJ7
    |F|F-JF---7F7-L7L|7|
    |FFJF7L7F-JF7|JL---7
    7-L-JL7||F7|L7F-7F7|
    L.L7LFJ|||||FJL7||LJ
    L7JLJL-JLJLJL--JLJ.L
""")

function solve2(lines)
    (cx, start, board), solved = setup(lines), solve(lines)

    sides = map(v->element_s(board, start+v, '.'), [-cx, cx, -1, +1])
    board[start] = @match sides begin
        ['F'|'|'|'7', 'L'|'|'|'J',           _,           _] => '|'
        [          _,           _, 'F'|'-'|'L', '7'|'-'|'J'] => '-'
        ['F'|'|'|'7',           _,           _, '7'|'-'|'J'] => 'L'
        ['F'|'|'|'7',           _, 'F'|'-'|'L',           _] => 'J'
        [          _, 'L'|'|'|'J',           _, '7'|'-'|'J'] => 'F'
        [          _, 'L'|'|'|'J', 'F'|'-'|'L',           _] => '7'
    end

    inside, entered, total = false, '.', 0
    for (s, b) in zip(solved, board)
        if s == 'S'
            if @match (entered, b) begin
                    (_, 'L'|'|'|'F') => true
                    ('F', '7') => true;
                    ('L', 'J') => true
                    _ => false
                end
                inside, entered = !inside, b
            end
        elseif inside
            total += 1
        end
    end
    total
end

check("Day $day.2 Sample 3", () -> solve2(sample3),  4)
check("Day $day.2 Sample 4", () -> solve2(sample4),  8)
check("Day $day.2 Sample 5", () -> solve2(sample5), 10)
check("Day $day.2 Problem",  () -> solve2(input),  579)
