import { getProblemInput, check, parseNumbers } from './utils'

const day = 20

const sample = parseNumbers(`
    1
    2
    -3
    3
    -2
    0
    4`)
const input = parseNumbers(getProblemInput(day))

function solve1(nums: number[]) {

    let array = nums.slice()

    for (let num of nums) {
        let index = array.indexOf(num)
        array.splice(index, 1)
        index = (index + num) % array.length
        array.splice(index, 0, num)
    }

    let result = 0

    let index = array.indexOf(0)
    for (let i = 0; i < 3; ++i) {
        index = (index + 1000) % array.length
        console.log(index, array[index])
        result += array[index]
    }

    return result
}

class Node {
    constructor(public value: number, public prev: Node, public next: Node) {}
}

function solve1x(nums) {
    let head: Node

    function toArray(start: Node): number[] {
        let array = Array<number>()
        let i = start
        do {
            array.push(i.value)
            i = i.next
        }
        while (i != start)

        return array
    }

    function find(start: Node, value: number): Node {
        for (let i = start; ; i = i.next)
            if (i.value == value)
                return i
    }

    head = new Node(nums[0], null, null)
    head.next = head.prev = head

    for (let i = 1; i < nums.length; ++i) {
        let node = new Node(nums[i], head.prev, head)
        head.prev.next = node
        head.prev = node
    }

    for (let num of nums) {
        if (num == 0)
            continue

        let i = find(head, num)
//        i.prev.next = i.next
//        i.next.prev = i.prev
        let old = i

        if (num < 0)
            --num
        while (num < 0) {
            i = i.prev
            ++num
        }
        while (num > 0) {
            i = i.next
            --num
        }

        if (i == old)
            continue

        old.prev.next = old.next
        old.next.prev = old.prev

        old.next = i.next
        old.prev = i
        i.next.prev = old
        i.next = old

        //console.log(toArray(head))
    }

    let result = 0

    let node = find(head, 0)
    for (let i = 0; ; ++i, node = node.next) {
        if (i == 1000 || i == 2000 || i == 3000) {
            result += node.value
            console.log(node.value)
        }
        if (i == 3000)
            break
    }

    return result
}

function solve2(nums) {
}

//check(`Day ${day}.1 Sample`,  () => solve1(sample), 3)
check(`Day ${day}.1x Sample`,  () => solve1x(sample), 3)
//check(`Day ${day}.1 Problem`, () => solve1(input),  -1)
check(`Day ${day}.1x Problem`, () => solve1x(input),  -1)
//check(`Day ${day}.2 Sample`,  () => solve2(sample), -1)
//check(`Day ${day}.2 Problem`, () => solve2(input),  -1)

//-8762 wrong
//4729 too low
//6248 too low
