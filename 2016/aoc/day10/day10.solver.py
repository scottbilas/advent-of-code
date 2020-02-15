#!/usr/bin/env python

def getinput(): return open('day10.input.txt').read()

import re
from collections import defaultdict

def solve(instrs, check):

    bots = defaultdict(lambda:[])

    for m in re.finditer('value (\d+).*?(\w+ \d+)', instrs):
        bots[m.group(2)].append(int(m.group(1)))

    while True:
        for m in re.finditer('(\w+ \d+).*?(\w+ \d+).*?(\w+ \d+)', instrs):
            src = bots[m.group(1)]
            if len(src) == 2:
                bots[m.group(2)].append(min(src))
                bots[m.group(3)].append(max(src))
                result = check(bots, m.group(1))
                if result is not None:
                    return result
                src.clear()


### PART 1

def solve1(a, b, instrs):
    return solve(instrs, lambda bots, name:
        int(name.split()[1]) if sorted(bots[name]) == sorted([a, b]) else None)

# samples

assert solve1(5, 2, """
    value 5 goes to bot 2
    bot 2 gives low to bot 1 and high to bot 0
    value 3 goes to bot 1
    bot 1 gives low to output 1 and high to bot 0
    bot 0 gives low to output 2 and high to output 0
    value 2 goes to bot 2
    """) == 2

# problem

s1 = solve1(61, 17, getinput())
print(s1)
assert s1 == 141


### PART 2

from iteration_utilities import flatten
from functools import reduce

def solve2(instrs):

    def check(bots, _):
        nums = list(flatten([bots[f'output {i}'] for i in range(3)]))
        return reduce(lambda a,b: a*b, nums) if len(nums) == 3 else None

    return solve(instrs, check)

s2 = solve2(getinput())
print(s2)
assert s2 == 1209
