#!/usr/bin/env python

def getinput(): return open('day9.input.txt').read().strip()

import re

def expand(str, start, end, nest):
    length, i = 0, start
    while i < end:
        if str[i] == '(':
            close = str.index(')', i) + 1
            take, repeat = [int(v) for v in re.findall('\d+', str[i:close])]
            length += nest(close, take) * repeat
            i = close + take
        else:
            length += 1
            i += 1
    return length


### PART 1

def solve1(file): return expand(file, 0, len(file), lambda _, take: take)

# samples

assert solve1('ADVENT') == 6
assert solve1('A(1x5)BC') == 7
assert solve1('(3x3)XYZ') == 9
assert solve1('(6x1)(1x3)A') == 6
assert solve1('X(8x2)(3x3)ABCY') == 18

# problem

s1 = solve1(getinput())
print(s1)
assert s1 == 107035


### PART 2

def solve2(file):
    def nest(start, count):
        return expand(file, start, start + count, nest)
    return expand(file, 0, len(file), nest)

# samples

assert solve2('(3x3)XYZ') == len('XYZXYZXYZ')
assert solve2('X(8x2)(3x3)ABCY') == len('XABCABCABCABCABCABCY')
assert solve2('(27x12)(20x12)(13x14)(7x10)(1x12)A') == 241920
assert solve2('(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN') == 445

# problem

s2 = solve2(getinput())
print(s2)
assert s2 == 11451628995
