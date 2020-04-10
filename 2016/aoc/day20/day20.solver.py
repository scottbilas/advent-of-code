#!/usr/bin/env python

def getinput(): return open('day20.input.txt').read()

import re

def solve(text):
    nums, target = [int(v) for v in re.findall('\d+', text)], 1
    for (l, u) in sorted(zip(nums[::2], nums[1::2])):
        if target < l: yield target, l
        target = max(target, u + 1)


### PART 1

def solve1(text):
    return next(solve(text))[0]

# samples

assert solve1('5-8 0-2 4-7') == 3

# problem

s1 = solve1(getinput())
print(s1)
assert s1 == 19449262


### PART 2

def solve2(text):
    return sum([l - t for t, l in solve(text)])

# problem

s2 = solve2(getinput())
print(s2)
assert s2 == 119
