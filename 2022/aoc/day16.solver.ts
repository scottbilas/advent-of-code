import { arrayOfArrays, check, getProblemInput, getProblemSample, parseLines, parseNumbers } from './utils'
import _ = require('lodash')

const day = 16

class Graph {
    rates: number[]
    costs: number[][]
    get count() { return this.rates.length }

    constructor(text: string) {
        // parse
        let nodes = parseLines(text).sort().map(l => l.match(/[A-Z][A-Z]|\d+/g))
        let lut = new Map(nodes.map((v, i) => [v[0], i]))

        this.rates = nodes.map(n => +n[1])
        this.costs = arrayOfArrays(this.count, () => Array(this.count).fill(0))

        // build graph
        let links: number[][] = arrayOfArrays(this.count)
        for (let node of nodes)
            for (let to of node.slice(2))
                links[lut.get(node[0])].push(lut.get(to))

        // calc cost to move from any node to any other, including opening the valve
        for (let origin = 0; origin < this.count; ++origin) {
            let queue: number[] = [origin]
            let cost = this.costs[origin]
            while (queue.length) {
                let from = queue.shift()
                for (let to of links[from]) {
                    if (cost[to])
                        continue

                    cost[to] = cost[from] + 1
                    queue.splice(_.sortedIndexBy(queue, to, n => cost[n]), 0, to)
                }
            }

            // fixup
            for (let i = 0; i < this.count; ++i) {
                if (this.rates[i])
                    ++cost[i]
                else
                    cost[i] = undefined
            }
        }
    }
}

const sample = new Graph(getProblemSample(day))
const input = new Graph(getProblemInput(day))

function solve1(graph: Graph) {
    let best = 0

    function walk(nodes: number[], remain, score, from, ito) {

        let to = nodes[ito]
        remain -= graph.costs[from][to]

        if (remain > 0) {
            score += graph.rates[to] * remain

            nodes[ito] = 0
            for (let i = 0; i < nodes.length; ++i)
                if (nodes[i])
                    walk(nodes, remain, score, to, i)
            nodes[ito] = to
        }

        best = Math.max(best, score)
    }

    let nodes = _.range(0, graph.count).filter(i => graph.rates[i])

    for (let i = 0; i < nodes.length; ++i)
        walk(nodes, 30, 0, 0, i)

    return best
}

function solve2(graph: Graph) {

    let best = 0

    function walk(
        nodes: number[],   // nodes to walk, 0 to mean used
        remain: number,    // total time remaining
        use: number,       // time used in step to get here
        score: number,     // current score
        otherPos: number,  // position of other agent before this step
        otherUse: number,  // accumulated time used by other agent while not being chosen
        ito: number) {     // index into nodes of target node for this step

        remain -= use
        otherUse += use

        if (remain > 0) {
            let to = nodes[ito]
            score += graph.rates[to] * remain

            nodes[ito] = 0
            for (let i = 0; i < nodes.length; ++i) {
                let next = nodes[i]
                if (!next)
                    continue

                let c0 = graph.costs[to][next]
                let c1 = graph.costs[otherPos][next] - otherUse
                if (c0 < c1)
                    walk(nodes, remain, c0, score, otherPos, otherUse, i)
                else
                    walk(nodes, remain, c1, score, to, 0, i)
            }
            nodes[ito] = to
        }

        best = Math.max(best, score)
    }

    let nodes = _.range(0, graph.count).filter(i => graph.rates[i])

    for (let i = 0; i < nodes.length; ++i)
        walk(nodes, 26, graph.costs[0][nodes[i]], 0, 0, 0, i)

    return best
}

check(`Day ${day}.1 Sample`,  () => solve1(sample), 1651)
check(`Day ${day}.1 Problem`, () => solve1(input),  1595)
check(`Day ${day}.2 Sample`,  () => solve2(sample), 1707)
check(`Day ${day}.2 Problem`, () => solve2(input),  2189)
