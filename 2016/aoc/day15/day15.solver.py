#!/usr/bin/env python

def getinput(): return open('day15.input.txt').read()


### PART 1

import re

def solve1(text):

    periods, offsets = [], []

    fields = [int(v) for v in re.findall(r'\d+', text)]
    for i in range(0, len(fields) // 4):
        periods.append(fields[i*4+1])
        offsets.append((fields[i*4+3] + i + 1) % periods[i])

    print(periods, offsets)

    t = 0

    return t

# samples

#assert solve1("""
#    Disc #1 has 5 positions; at time=0, it is at position 4.
#    Disc #2 has 2 positions; at time=0, it is at position 1.
#    """) == 5

"""
t % 13 - (13 - 11 - 1) == 0    t % 13 ==  1
t %  5 - ( 5 -  0 - 2) == 0    t %  5 ==  3
t % 17 - (17 - 11 - 3) == 0    t % 17 ==  3
t %  3 - ( 3 -  0 - 4) == 0    t %  3 ==  2
t %  7 - ( 7 -  2 - 5) == 0    t %  7 ==  0
t % 19 - (19 - 17 - 6) == 0    t % 19 == 15
"""

# problem

s1 = solve1(getinput())
print(s1)
#assert s1 == 25427
# 100127 too low


### PART 2

#def solve2(salt): return solve(salt, 2016)

# samples

#assert solve2('abc') == 22551

# problem

#s2 = solve2(getinput())
#print(s2)
#assert s2 == 22045
