import u = require('./utils')

const day = 9

const sample1 = `
    R 4
    U 4
    L 3
    D 1
    R 4
    D 1
    L 5
    R 2`

const sample2 = `
    R 5
    U 8
    L 8
    D 3
    R 17
    D 10
    L 25
    U 20`

const input = u.getProblemInput(day)

function solve(text, len) {
    let rope = Array(len).fill(0).map(_ => ({x: 0, y: 0}))
    let used = new Set(['0,0'])
    let moves = u.parseWords(text)

    for (let move of moves.chunk(2)) {
        let [dir, dist] = [move[0], +move[1]]

        for (let istep = 0; istep < dist; istep++) {
            rope.first().x += {L: -1, R: 1}[dir] ?? 0
            rope.first().y += {U: -1, D: 1}[dir] ?? 0

            for (let irope = 1; irope < rope.length; irope++) {
                let [h, t] = [rope[irope-1], rope[irope]]
                let [dx, dy] = [h.x-t.x, h.y-t.y]
                if (Math.abs(dx) >= 2 || Math.abs(dy) >= 2) {
                    t.x += u.minmax(dx, -1, 1)
                    t.y += u.minmax(dy, -1, 1)
                }
            }

            used.add(rope.last().x + ',' + rope.last().y)
        }
    }

    return used.size
}

const solve1 = text => solve(text, 2)
const solve2 = text => solve(text, 10)

u.test(`Day ${day}.1 Sample 1`, () => solve1(sample1),   13)
u.test(`Day ${day}.1 Problem`,  () => solve1(input),   6037)
u.test(`Day ${day}.2 Sample 1`, () => solve2(sample1),    1)
u.test(`Day ${day}.2 Sample 2`, () => solve2(sample2),   36)
u.test(`Day ${day}.2 Problem`,  () => solve2(input),   2485)
