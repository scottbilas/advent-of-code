#!/usr/bin/env python

def getinput(): return open('day22.input.txt').read()

### PART 1

import re

def solve1(text):

    nums = [int(i) for i in re.findall(r'\d+', text)]
    avail = sorted(zip(nums[3::6], nums[4::6]), key=lambda v: (v[1], v[0]))
    used = [v for v in sorted(avail) if v[0] > 0]
    pairs, ia = 0, 0

    for iu, vu in enumerate(used):
        while ia < len(avail) and vu[0] > avail[ia][1]:
            ia += 1
        pairs += len(avail) - ia
        if vu[0] <= vu[1]:
            pairs -= 1

    return pairs

s1 = solve1(getinput())
print(s1)
assert s1 == 903


### PART 2

def solve2(text):
    nums = [int(i) for i in re.findall(r'\d+', text)]
    grid = {}
    for x, y, size, used in [tuple(nums[i:i+4]) for i in range(0, len(nums), 6)]:
        grid[(x, y)] = (used, size)
    start = (max(grid.keys())[0], 0)

# sample

assert solve2('''
    Filesystem            Size  Used  Avail  Use%
    /dev/grid/node-x0-y0   10T    8T     2T   80%
    /dev/grid/node-x0-y1   11T    6T     5T   54%
    /dev/grid/node-x0-y2   32T   28T     4T   87%
    /dev/grid/node-x1-y0    9T    7T     2T   77%
    /dev/grid/node-x1-y1    8T    0T     8T    0%
    /dev/grid/node-x1-y2   11T    7T     4T   63%
    /dev/grid/node-x2-y0   10T    6T     4T   60%
    /dev/grid/node-x2-y1    9T    8T     1T   88%
    /dev/grid/node-x2-y2    9T    6T     3T   66%
    ''') == 7

# problem

#s2 = solve2(getinput())
#print(s2)
#assert s2 ==
