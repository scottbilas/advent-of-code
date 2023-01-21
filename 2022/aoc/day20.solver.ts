import { getProblemInput, check, parseNumbers } from './utils'

const day = 20

const sample = `
    1
    2
    -3
    3
    -2
    0
    4`
const input = getProblemInput(day)

class Node {
    constructor(public value: number) {}
    next: Node; prev: Node
}

function parse(text) {
    let nodes = parseNumbers(text).map(n => new Node(n))

    for (let i = 0; i < nodes.length; ++i) {
        nodes[i].prev = nodes[i-1]
        nodes[i].next = nodes[i+1]
    }

    nodes[0].prev = nodes.at(-1)
    nodes.at(-1).next = nodes[0]

    return nodes
}

function mix(nodes: Node[]) {
    for (let node of nodes) {
        if (node.value == 0)
            continue

        let count = node.value % (nodes.length-1)
        if (node.value < 0)
            count += nodes.length-1

        let dst = node
        for (; count > 0; --count)
            dst = dst.next

        node.prev.next = node.next // unlink
        node.next.prev = node.prev
        node.prev      = dst       // link inner
        node.next      = dst.next
        node.prev.next = node      // link outer
        node.next.prev = node
    }
}

function score(nodes: Node[]): number {
    let node = nodes.find(n => n.value == 0)
    for (let [i, result] = [0, 0]; ; ++i, node = node.next) {
        if (i == 1000 || i == 2000)
            result += node.value
        else if (i == 3000)
            return result + node.value
    }
}

function solve1(text) {
    let nodes = parse(text)
    mix(nodes)
    return score(nodes)
}

function solve2(text) {
    let nodes = parse(text)
    for (let node of nodes)
        node.value *= 811589153
    for (let i = 0; i < 10; ++i)
        mix(nodes)
    return score(nodes)
}

check(`Day ${day}.1 Sample`,  () => solve1(sample),             3)
check(`Day ${day}.1 Problem`, () => solve1(input),           7584)
check(`Day ${day}.2 Sample`,  () => solve2(sample),    1623178306)
check(`Day ${day}.2 Problem`, () => solve2(input),  4907679608191)
