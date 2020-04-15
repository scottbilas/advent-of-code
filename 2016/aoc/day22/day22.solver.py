#!/usr/bin/env python

def getinput(): return open('day22.input.txt').read()

### PART 1

import re

def solve1():

    nums = [int(i) for i in re.findall(r'\d+', getinput())]
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

s1 = solve1()
print(s1)
assert s1 == 903


### PART 2

#s2 = solve('fbgdceah', getinput(), True)
#print(s2)
#assert s2 == 'aghfcdeb'
