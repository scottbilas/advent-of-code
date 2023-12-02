import _ = require('lodash')
import { getProblemInput, check, parseNumbers, parseLines } from './utils'

const day = 19

const sample = `
    Blueprint 1:
    Each ore robot costs 4 ore.
    Each clay robot costs 2 ore.
    Each obsidian robot costs 3 ore and 14 clay.
    Each geode robot costs 2 ore and 7 obsidian.

    Blueprint 2:
    Each ore robot costs 2 ore.
    Each clay robot costs 3 ore.
    Each obsidian robot costs 3 ore and 8 clay.
    Each geode robot costs 3 ore and 12 obsidian.`

const input = getProblemInput(day)

function solve(minutes, text) {

    let result = 0
    const ore = 0, cly = 1, obs = 2, geo = 3

    for (let [id, oreCostOre, clayCostOre, obsidianCostOre, obsidianCostClay, geodeCostOre, geodeCostObsidian] of parseNumbers(text).chunk(7)) {
        let best = 0

        // 0=ore, 1=clay, 2=obs, 3=geo
        let robots = Array<number>(minutes*4).fill(0)
        robots[0] = 1
        let quants = Array<number>(minutes*4).fill(0)

        function walk(minute: number) {

            let base = (minute-1)*8
            let base2 = base+8

            if (minute == minutes) {
                best = Math.max(best, robots[base+geo] + quants[base+geo])
                return
            }

            if (quants[base+ore] >= geodeCostOre && quants[base+obs] >= geodeCostObsidian) {
                quants[base2+ore] = quants[base+ore] + robots[base+ore] - geodeCostOre
                quants[base2+cly] = quants[base+cly] + robots[base+cly]
                quants[base2+obs] = quants[base+obs] + robots[base+obs] - geodeCostObsidian
                quants[base2+geo] = quants[base+geo] + robots[base+geo]
                robots[base2+ore] = robots[base+ore]
                robots[base2+cly] = robots[base+cly]
                robots[base2+obs] = robots[base+obs]
                robots[base2+geo] = robots[base+geo] + 1
                walk(minute+cly)
                return
            }

            if (robots[base+obs] < geodeCostObsidian && quants[base+ore] >= obsidianCostOre && quants[base+cly] >= obsidianCostClay) {
                quants[base2+ore] = quants[base+ore] + robots[base+ore] - obsidianCostOre
                quants[base2+cly] = quants[base+cly] + robots[base+cly] - obsidianCostClay
                quants[base2+obs] = quants[base+obs] + robots[base+obs]
                quants[base2+geo] = quants[base+geo] + robots[base+geo]
                robots[base2+ore] = robots[base+ore]
                robots[base2+cly] = robots[base+cly]
                robots[base2+obs] = robots[base+obs] + 1
                robots[base2+geo] = robots[base+geo]
                walk(minute+cly)
            }

            if (robots[base+cly] < obsidianCostClay && quants[base+ore] >= clayCostOre) {
                quants[base2+ore] = quants[base+ore] + robots[base+ore] - clayCostOre
                quants[base2+cly] = quants[base+cly] + robots[base+cly]
                quants[base2+obs] = quants[base+obs] + robots[base+obs]
                quants[base2+geo] = quants[base+geo] + robots[base+geo]
                robots[base2+ore] = robots[base+ore]
                robots[base2+cly] = robots[base+cly] + 1
                robots[base2+obs] = robots[base+obs]
                robots[base2+geo] = robots[base+geo]
                walk(minute+cly)
            }

            if (robots[base+ore] < Math.max(oreCostOre, clayCostOre, obsidianCostOre, geodeCostOre) && quants[base+ore] >= oreCostOre) {
                quants[base2+ore] = quants[base+ore] + robots[base+ore] - oreCostOre
                quants[base2+cly] = quants[base+cly] + robots[base+cly]
                quants[base2+obs] = quants[base+obs] + robots[base+obs]
                quants[base2+geo] = quants[base+geo] + robots[base+geo]
                robots[base2+ore] = robots[base+ore] + 1
                robots[base2+cly] = robots[base+cly]
                robots[base2+obs] = robots[base+obs]
                robots[base2+geo] = robots[base+geo]
                walk(minute+cly)
            }

            quants[base2+ore] = quants[base+ore] + robots[base+ore]
            quants[base2+cly] = quants[base+cly] + robots[base+cly]
            quants[base2+obs] = quants[base+obs] + robots[base+obs]
            quants[base2+geo] = quants[base+geo] + robots[base+geo]
            robots[base2+ore] = robots[base+ore]
            robots[base2+cly] = robots[base+cly]
            robots[base2+obs] = robots[base+obs]
            robots[base2+geo] = robots[base+geo]
            walk(minute+cly)
        }

        walk(1)
//        console.log(`${id}: ${best}`)
        result += id * best
    }

    return result
}

function solve2(text) {
}

check(`Day ${day}.1 Sample`,  () => solve(24, sample),   33)
check(`Day ${day}.1 Problem`, () => solve(24, input),  1616)  // 113s
//check(`Day ${day}.2 Sample`,  () => solve(32, sample), 56 * 62)
check(`Day ${day}.2 Problem`, () => solve(32, parseLines(input).slice(0, 3).join('\n')), 8990) // ~20min

