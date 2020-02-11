#!/usr/bin/env python

input = open('day4.input.txt').readlines()

### PART 1

import re

def solve1(rooms):
    sum = 0
    for room in rooms:
        m = re.match('(.*)-(\d+)\[(\w+)\]', room)

        d = {}
        for c in m.group(1).replace('-', ''):
            d[c] = d.get(c, 0) + 1

        freqs = sorted(d.items(), key=lambda kv: (-kv[1], kv[0]))[0:5]
        if m.group(3) == ''.join([v[0] for v in freqs]):
            sum += int(m.group(2))

    return sum

# samples

assert solve1([
    'aaaaa-bbb-z-y-x-123[abxyz]',
    'a-b-c-d-e-f-g-h-987[abcde]',
    'not-a-real-room-404[oarel]',
    'totally-real-room-200[decoy]',
    ]) == 1514

# problem

s1 = solve1(input)
print(s1)
assert s1 == 245102


### PART 2


# samples


# problem
