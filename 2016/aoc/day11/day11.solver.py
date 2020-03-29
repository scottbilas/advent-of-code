#!/usr/bin/env python

def getinput(): return open('day11.input.txt').read()

"""
Key rules:
  * Goal is all inventory on top floor
  / Elevator will not move if empty
  / Each elevator move counts, even if no item exchange
  / M fries if near G that is not its own
  / Elevator considered part of floor it is on
  / Elevator starts on floor 1

Algo:
  * Recursive walk all possibilities: up/down + 1-2 items
  * Abort if board state seen before
  * Abort if new/old floor state invalid
  * Abort if num moves >= current min
"""


### PART 1

from collections import defaultdict
from itertools import combinations
import re

def valid(floor):
    generators, unpaired = 0, 0
    for item in floor:
        if item[-1] == 'g':
            generators += 1
        else:
            unpaired += item[0]+'g' not in floor
    return generators == 0 or unpaired == 0

assert valid(['ag', 'am']) == True
assert valid(['ag', 'bm']) == False
assert valid(['ag', 'bg']) == True
assert valid(['am', 'bm']) == True
assert valid(['ag', 'bg', 'bm']) == True
assert valid(['ag', 'am', 'bm']) == False
assert valid(['ag', 'am', 'bg']) == True
assert valid(['am', 'bg', 'bm']) == False
assert valid(['ag', 'am', 'bg', 'bm']) == True

def solve1(arrangement):
    floors = [
        sorted([item[0]+item[1] for item in re.findall('a (\w)\S+ (m|g)', line)])
        for line in arrangement.strip().splitlines()]

    visited = defaultdict(lambda: 99999)
    target = sum([len(i) for i in floors])
    mindist = 99999

    def walk(floor, dist):
        nonlocal mindist
        if dist > 100:
            return

        state = str([floor]+floors)
        if dist > visited[state]: return
        visited[state] = dist

        if len(floors[-1]) == target:
            mindist = min(dist, mindist)
            return

        def move(floorn, take):
            oldc, oldn = floors[floor], floors[floorn]
            for remove in combinations(oldc, take):
                newc = [i for i in oldc if i not in remove]
                newn = sorted(oldn + list(remove))
                if valid(newc) and valid(newn):
                    floors[floor], floors[floorn] = newc, newn
                    walk(floorn, dist+1)
                    floors[floor], floors[floorn] = oldc, oldn

        if floor < len(floors)-1:
            move(floor+1, 1)
            move(floor+1, 2)
        if floor > 0:
            move(floor-1, 1)
            move(floor-1, 2)

    walk(0, 0)
    return mindist

# samples

assert solve1("""
    The first floor contains a hydrogen-compatible microchip and a lithium-compatible microchip.
    The second floor contains a hydrogen generator.
    The third floor contains a lithium generator.
    The fourth floor contains nothing relevant.
    """) == 11

# problem

s1 = solve1(getinput())
print(s1)
#assert s1 == 141


### PART 2
