#!/usr/bin/env python

input = open('day9.input.txt').read().strip()


### PART 1

import re

def solve1(file):
    length, i = 0, 0
    while i < len(file):
        if file[i] == '(':
            end = file.index(')', i) + 1
            take, repeat = [int(v) for v in re.findall('\d+', file[i:end])[:2]]
            length += take * repeat
            i = end + take
        else:
            length += 1
            i += 1
    return length

# samples

assert solve1('ADVENT') == 6
assert solve1('A(1x5)BC') == 7
assert solve1('(3x3)XYZ') == 9
assert solve1('(6x1)(1x3)A') == 6
assert solve1('X(8x2)(3x3)ABCY') == 18

# problem

s1 = solve1(input)
print(s1)
assert s1 == 107035


### PART 2


# samples


# problem
