import _ = require('lodash')
import u = require('./utils')

const day = 6

const samples = [
    { text: 'mjqjpqmgbljsphdztnvjfqwrcgsmlb',    answer: [ 7, 19] },
    { text: 'bvwbjplbgvbhsrlpgdmjqwftvncz',      answer: [ 5, 23] },
    { text: 'nppdvjthqldpwncqszvftbrmjlhg',      answer: [ 6, 23] },
    { text: 'nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg', answer: [10, 29] },
    { text: 'zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw',  answer: [11, 26] }]

const input = u.getProblemInput(day).trim()

function solve(text, size) {
    for (let i = size;; ++i)
        if (_.uniq(text.slice(i-size, i)).length === size)
            return i
}

const solve1 = text => solve(text, 4)
const solve2 = text => solve(text, 14)

for (let i = 0; i < samples.length; ++i)
    test(`Day ${day}.1 Sample ${i+1}`, () => { expect(solve1(samples[i].text)).toBe(samples[i].answer[0]); });
test(`Day ${day}.1 Problem`, () => { expect(solve1(input)).toBe(1262); });

for (let i = 0; i < samples.length; ++i)
    test(`Day ${day}.2 Sample ${i+1}`, () => { expect(solve2(samples[i].text)).toBe(samples[i].answer[1]); });
test(`Day ${day}.2 Problem`, () => { expect(solve2(input)).toBe(3444); });
