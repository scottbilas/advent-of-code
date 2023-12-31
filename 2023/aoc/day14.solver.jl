day = 14
include("utils.jl")

input = getProblemInput()

sample = getSampleLines("""
    O....#....
    O.OO#....#
    .....##...
    OO.#O....O
    .O.....O#.
    O.#..O.#.#
    ..O..#O..O
    .......O..
    #....###..
    #OO..#....
""")

function solve1(lines)
    board, (cx, cy) = parseGrid(lines)

    total = 0
    for x in 1:cx, y in 1:cy
        if board[x, y] == 'O'
            ty = y
            while ty > 1 && board[x, ty-1] == '.'
                board[x, ty-1] = 'O'
                board[x, ty] = '.'
                ty -= 1
            end
            total += cy-ty+1
        end
    end

    total
end

check("Day $day.1 Sample",  () -> solve1(sample),   136)
check("Day $day.1 Problem", () -> solve1(input), 109466)

# too high 109466

#solve2 = lines -> solve(lines)
#
#check("Day $day.2 Sample",  () -> solve2(sample),  2)
#check("Day $day.2 Problem", () -> solve2(input), 908)
