#!/usr/bin/env python

input = open('day7.input.txt').read().splitlines()


### PART 1

import re

def solve1(addresses):

    def abba(s):
        for i in range(3, len(s)):
            if s[i-3] != s[i-2] and s[i-3] == s[i] and s[i-2] == s[i-1]:
                return 1
        return 0

    count = 0
    for address in addresses:
        a = [0, 0]
        for (i, v) in enumerate([abba(v) for v in re.findall('\w+', address)]):
            a[i%2] += v
        if a[0] != 0 and a[1] == 0:
            count += 1
    return count

# samples

assert solve1(['abba[mnop]qrst']) == 1
assert solve1(['abcd[bddb]xyyx']) == 0
assert solve1(['aaaa[qwer]tyui']) == 0
assert solve1(['ioxxoj[asdfgh]zxcvbn']) == 1

# problem

s1 = solve1(input)
print(s1)
assert s1 == 110


### PART 2


# samples


# problem
