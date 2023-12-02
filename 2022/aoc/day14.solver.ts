import { check, getProblemInput, Grid, number2, parseLines, parseNumbers } from './utils'

const day = 14

const sample = `
    498,4 -> 498,6 -> 496,6
    503,4 -> 502,4 -> 502,9 -> 494,9`

const input = getProblemInput(day)

function solve1(text) {
    let tl = new number2(1000, 1000)
    let br = new number2(0, 0)
    let nums = parseNumbers(text)
    for (let i = 0; i < nums.length;) {
        let [x, y] = [nums[i++], nums[i++]]
        tl = new number2(Math.min(tl.x, x), Math.min(tl.y, y))
        br = new number2(Math.max(br.x, x), Math.max(br.y, y))
    }

    tl = new number2(Math.min(tl.x, 500), Math.min(tl.y, 0))
    br = new number2(Math.max(br.x, 500), Math.max(br.y, 0))

    let cx = br.x - tl.x + 1
    let cy = br.y - tl.y + 1

    let grid = new Grid<string>(Array(cx*cy).fill('.'), cx, cy)
    for (let line of parseLines(text)) {
        let nums = parseNumbers(line)
        let cur = new number2(nums[0], nums[1])
        for (let i = 2; i < nums.length;) {
            let end = new number2(nums[i++], nums[i++])
            let dx = end.x - cur.x
            dx = dx > 0 ? 1 : dx < 0 ? -1 : 0
            let dy = end.y - cur.y
            dy = dy > 0 ? 1 : dy < 0 ? -1 : 0

            while (cur.x != end.x || cur.y != end.y) {
                grid.set(cur.sub(tl), '#')
                cur.x += dx
                cur.y += dy
            }
            grid.set(cur.sub(tl), '#')
        }
    }

    let rest = 0
    let pos = new number2(500 - tl.x, 0)

    for (;;) {

        function lookup(ox: number, oy: number): [string, number2] {
            let test = pos.add(new number2(ox, oy))
            return [grid.try(test), test]
        }

        let d = lookup(0, 1)
        if (d[0] == '.') {
            pos = d[1]
            continue
        }
        else if (d[0] == undefined) {
            break
        }
        let l = lookup(-1, 1)
        if (l[0] == '.') {
            pos = l[1]
            continue
        }
        else if (l[0] == undefined) {
            break
        }
        let r = lookup(1, 1)
        if (r[0] == '.') {
            pos = r[1]
            continue
        }
        else if (r[0] == undefined) {
            break
        }

        grid.set(pos, 'o')
        ++rest
        pos = new number2(500 - tl.x, 0)
    }

    return rest
}

function solve2(text) {
    let tl = new number2(1000, 1000)
    let br = new number2(0, 0)
    let nums = parseNumbers(text)
    for (let i = 0; i < nums.length;) {
        let [x, y] = [nums[i++], nums[i++]]
        tl = new number2(Math.min(tl.x, x), Math.min(tl.y, y))
        br = new number2(Math.max(br.x, x), Math.max(br.y, y))
    }

    tl = new number2(Math.min(tl.x, 500), Math.min(tl.y, 0))
    br = new number2(Math.max(br.x, 500), Math.max(br.y, 0))

    let cy = br.y - tl.y + 1 + 2

    tl.x = Math.min(tl.x, 500 - cy)
    br.x = Math.max(br.x, 500 + cy)

    let cx = br.x - tl.x + 1

    let grid = new Grid<string>(Array(cx*cy).fill('.'), cx, cy)

    for (let x = 0; x < cx; ++x)
        grid.set(new number2(x, cy-1), '#')

    for (let line of parseLines(text)) {
        let nums = parseNumbers(line)
        let cur = new number2(nums[0], nums[1])
        for (let i = 2; i < nums.length;) {
            let end = new number2(nums[i++], nums[i++])
            let dx = end.x - cur.x
            dx = dx > 0 ? 1 : dx < 0 ? -1 : 0
            let dy = end.y - cur.y
            dy = dy > 0 ? 1 : dy < 0 ? -1 : 0

            while (cur.x != end.x || cur.y != end.y) {
                grid.set(cur.sub(tl), '#')
                cur.x += dx
                cur.y += dy
            }
            grid.set(cur.sub(tl), '#')
        }
    }

    let rest = 0
    let pos = new number2(500 - tl.x, 0)

    for (;;) {

        function lookup(ox: number, oy: number): [string, number2] {
            let test = pos.add(new number2(ox, oy))
            return [grid.try(test), test]
        }

        let d = lookup(0, 1)
        if (d[0] == '.') {
            pos = d[1]
            continue
        }
        let l = lookup(-1, 1)
        if (l[0] == '.') {
            pos = l[1]
            continue
        }
        let r = lookup(1, 1)
        if (r[0] == '.') {
            pos = r[1]
            continue
        }

        grid.set(pos, 'o')
        ++rest

        if (pos.x == 500 - tl.x && pos.y == 0)
            return rest

        pos = new number2(500 - tl.x, 0)

    }
}

check(`Day ${day}.1 Sample`,  () => solve1(sample),    24)
check(`Day ${day}.1 Problem`, () => solve1(input),    655)
check(`Day ${day}.2 Sample`,  () => solve2(sample),    93)
check(`Day ${day}.2 Problem`, () => solve2(input),  26484)
