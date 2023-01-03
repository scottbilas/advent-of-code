import { check, getProblemInput, parseNumGrid } from './utils'

const day = 8

const sample = `
    30373
    25512
    65332
    33549
    35390`

const input = getProblemInput(day)

function solve(text, pred) {

    let grid = parseNumGrid(text)

    function test(sx, sy, dx, dy) {
        let see = 0
        for (let [x, y] = [sx+dx, sy+dy]; x>=0 && x<grid.cx && y>=0 && y<grid.cy; x+=dx, y+=dy) {
            ++see
            if (grid.get2(x, y) >= grid.get2(sx, sy))
                return [false, see]
        }
        return [true, see]
    }

    let result = 0

    for (let y = 0; y < grid.cy; ++y)
        for (let x = 0; x < grid.cx; ++x)
            result = pred(result, [
                test(x, y, 1, 0), test(x, y, -1,  0),
                test(x, y, 0, 1), test(x, y,  0, -1)])

    return result
}

const solve1 = text => solve(text, (visible, walk) =>
    visible + (walk.map(w => w[0]).sum() > 0))
const solve2 = text => solve(text, (max, walk) =>
    Math.max(max, walk.map(w => w[1]).product()))

check(`Day ${day}.1 Sample`,  () => solve1(sample),     21);
check(`Day ${day}.1 Problem`, () => solve1(input),    1538);
check(`Day ${day}.2 Sample`,  () => solve2(sample),      8);
check(`Day ${day}.2 Problem`, () => solve2(input),  496125);
