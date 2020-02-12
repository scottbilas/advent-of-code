#!/usr/bin/env python

inputData = open('day10.input.txt').read()


### PART 1

import re
from collections import defaultdict

def solve1(a, b, instrs):

    bots = defaultdict(lambda:[])

    for m in re.finditer('value (\d+).*?(\w+ \d+)', instrs):
        bots[m.group(2)].append(int(m.group(1)))

    while True:
        for m in re.finditer('(\w+ \d+).*?(\w+ \d+).*?(\w+ \d+)', instrs):
            src = bots[m.group(1)]
            if len(src) == 2:
                if src == [a, b] or src == [b, a]:
                    return int(m.group(1).split()[1])
                bots[m.group(2)].append(min(src))
                bots[m.group(3)].append(max(src))
                src.clear()

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

s1 = solve1(61, 17, inputData)
print(s1)
assert s1 == 141


### PART 2


# samples


# problem
