import { check, getProblemInput } from './utils'
import _ = require('lodash')

const day = 6

const samples = [
    { text: 'mjqjpqmgbljsphdztnvjfqwrcgsmlb',    answer: [ 7, 19] },
    { text: 'bvwbjplbgvbhsrlpgdmjqwftvncz',      answer: [ 5, 23] },
    { text: 'nppdvjthqldpwncqszvftbrmjlhg',      answer: [ 6, 23] },
    { text: 'nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg', answer: [10, 29] },
    { text: 'zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw',  answer: [11, 26] }]

const input = getProblemInput(day).trim()

function solve(text, size) {
    for (let i = size;; ++i)
        if (_.uniq(text.slice(i-size, i)).length === size)
            return i
}

const solve1 = text => solve(text, 4)
const solve2 = text => solve(text, 14)

for (let i = 0; i < samples.length; ++i)
    check(`Day ${day}.1 Sample ${i+1}`, () => solve1(samples[i].text), samples[i].answer[0])
check(`Day ${day}.1 Problem`, () => solve1(input), 1262)

for (let i = 0; i < samples.length; ++i)
    check(`Day ${day}.2 Sample ${i+1}`, () => solve2(samples[i].text), samples[i].answer[1])
check(`Day ${day}.2 Problem`, () => solve2(input), 3444)
