import { getProblemInput, check, number3, parseNumber3s } from './utils'

const day = 18

const sample = `
    2,2,2
    1,2,2
    3,2,2
    2,1,2
    2,3,2
    2,2,1
    2,2,3
    2,2,4
    2,2,6
    1,2,5
    3,2,5
    2,1,5
    2,3,5`
const input = getProblemInput(day)

function solve1(text) {

    let sides = new Map<string, number>()
    const add = key => sides.set(key, (sides.get(key) ?? 0) + 1)

    for (let pos of parseNumber3s(text)) {
        add('x'+pos), add('x'+pos.addX(1))
        add('y'+pos), add('y'+pos.addY(1))
        add('z'+pos), add('z'+pos.addZ(1))
    }

    return [...sides.values()].filter(x => x == 1).length
}

function solve2(text) {
    let cubes = new Set<string>()
    let [min, max] = [number3.maxNumber, number3.minNumber]

    // get bounds
    for (let pos of parseNumber3s(text)) {
        cubes.add(pos.toString())
        min = min.min(pos), max = max.max(pos)
    }

    // slightly grow to avoid making unwalkable regions at bounding box edge
    min = min.sub(number3.one); max = max.add(number3.one)

    // find all outside cubes
    let outside = new Set<string>()
    for (let queue = [number3.zero]; queue.length;) {
        let work = queue.pop()
        if (work.lt(min).any || work.gt(max).any)
            continue

        let key = work.toString()
        if (!cubes.has(key) && !outside.has(key)) {
            outside.add(key)
            queue.push(work.addX(-1)), queue.push(work.addX(+1))
            queue.push(work.addY(-1)), queue.push(work.addY(+1))
            queue.push(work.addZ(-1)), queue.push(work.addZ(+1))
        }
    }

    // build all possible cubes that aren't outside
    let inside = []
    for (let x = min.x; x <= max.x; ++x) {
        for (let y = min.y; y <= max.y; ++y) {
            for (let z = min.z; z <= max.z; ++z) {
                let key = number3.toString(x, y, z)
                if (!outside.has(key))
                    inside.push(key)
            }
        }
    }

    // new set does not have holes, run it through the other solver
    return solve1(inside.join(' '))
}

check(`Day ${day}.1 Sample`,  () => solve1(sample),   64)
check(`Day ${day}.1 Problem`, () => solve1(input),  3494)
check(`Day ${day}.2 Sample`,  () => solve2(sample),   58)
check(`Day ${day}.2 Problem`, () => solve2(input),  2062)
