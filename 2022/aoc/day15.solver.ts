import { check, getProblemInput, getProblemSample, parseNumbers } from './utils'
import _ = require('lodash')

const day = 15

const sample = getProblemSample(day)
const input = getProblemInput(day)

function solve1(text: string, y: number) {
    let used = new Set<number>()

    for (let [sx, sy, bx, by] of _.chunk(parseNumbers(text), 4)) {
        // 8, 7 -> 2, 10
        // r = abs(2-8)+abs(10-7) = 9
        // min y = -2
        // max y = 16
        // for y = 3, min x = abs(sy-3)=4, r-4=5, so xstart = sx-5=3
        // max x = abs(sy-3)=4, r-4=5, so xend = sx+5=13

        let r = Math.abs(bx - sx) + Math.abs(by - sy)
        let miny = sy - r
        let maxy = sy + r
        if (y < miny || y > maxy)
            continue

        let rx = r - Math.abs(sy - y)
        let minx = sx - rx
        let maxx = sx + rx

        for (let x = minx; x <= maxx; x++)
        {
            if (y != by || x != bx)
                used.add(x)
        }
    }

    return used.size
}

function solve2(text, max) {
    let sensors = _
        .chunk(parseNumbers(text), 4)
        .map(a => [a[0], a[1], Math.abs(a[2]-a[0]) + Math.abs(a[3]-a[1])])

    function test(x, y): boolean {
        if (x < 0 || y < 0 || x > max || y > max)
            return false
        for (let [sx, sy, r] of sensors) {
            if (Math.abs(sx-x) + Math.abs(sy-y) <= r)
                return false
        }
        return true
    }

    for (let [sx, sy, r] of sensors) {
        for (let i = 0; i < r+1; ++i) {
            let x = sx + i, y = sy - (r+1) + i
            if (test(x, y))
                return x*4000000 + y
            x = sx + r+1 - i, y = sy + i
            if (test(x, y))
                return x*4000000 + y
            x = sx - i, y = sy + r+1 - i
            if (test(x, y))
                return x*4000000 + y
            x = sx - (r+1) + i, y = sy - i
            if (test(x, y))
                return x*4000000 + y
        }
    }
}

check(`Day ${day}.1 Sample`,  () => solve1(sample,     10),              26)
check(`Day ${day}.1 Problem`, () => solve1(input, 2000000),         4582667)
check(`Day ${day}.2 Sample`,  () => solve2(sample,     20),        56000011)
check(`Day ${day}.2 Problem`, () => solve2(input, 4000000),  10961118625406)
