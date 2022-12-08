import _ = require('lodash')
import u = require('./utils')

const day = 8

const sample = `
    30373
    25512
    65332
    33549
    35390`

const input = u.getProblemInput(day)

function solve(text, pred) {

    let [grid, cx, cy] = u.parseNumGrid(text)

    function test(sx, sy, dx, dy) {
        let see = 0
        for (let [x, y] = [sx+dx, sy+dy]; x>=0 && x<cx && y>=0 && y<cy; x+=dx, y+=dy) {
            ++see
            if (grid[y*cx+x] >= grid[sy*cx+sx])
                return [false, see]
        }
        return [true, see]
    }

    let result = 0

    for (let y = 0; y < cy; ++y)
        for (let x = 0; x < cx; ++x)
            result = pred(result, [
                test(x, y, 1, 0), test(x, y, -1,  0),
                test(x, y, 0, 1), test(x, y,  0, -1)])

    return result
}

const solve1 = text => solve(text, (visible, walk) =>
    visible + (_.sum(walk.map(w => w[0])) ? 1 : 0))

const solve2 = text => solve(text, (max, walk) =>
    Math.max(max, walk.reduce((t, i) => t * i[1], 1)))

u.test(`Day ${day}.1 Sample`,  () => solve1(sample), 21);
u.test(`Day ${day}.1 Problem`, () => solve1(input),  1538);
u.test(`Day ${day}.2 Sample`,  () => solve2(sample), 8);
u.test(`Day ${day}.2 Problem`, () => solve2(input),  496125);
