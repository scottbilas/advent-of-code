#!/usr/bin/env python

def getinput(): return open('day7.input.txt').read().splitlines()

import re

### PART 1

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
        count += 1 if a[0] != 0 and a[1] == 0 else 0
    return count

# samples

assert solve1(['abba[mnop]qrst']) == 1
assert solve1(['abcd[bddb]xyyx']) == 0
assert solve1(['aaaa[qwer]tyui']) == 0
assert solve1(['ioxxoj[asdfgh]zxcvbn']) == 1

# problem

s1 = solve1(getinput())
print(s1)
assert s1 == 110


### PART 2

def solve2(addresses):

    def aba(s):
        for i in range(2, len(s)):
            if s[i-2] != s[i-1] and s[i-2] == s[i]:
                yield s[i-2:i+1]

    count = 0
    for address in addresses:
        abas = [set(), set()]
        for (i, v) in enumerate([aba(v) for v in re.findall('\w+', address)]):
            abas[i%2].update(v)
        count += 1 if any(f'{v[1]}{v[0]}{v[1]}' in abas[1] for v in abas[0]) else 0

    return count

# samples

assert solve2(['aba[bab]xyz']) == 1
assert solve2(['xyx[xyx]xyx']) == 0
assert solve2(['aaa[kek]eke']) == 1
assert solve2(['zazbz[bzb]cdb']) == 1

# problem

s2 = solve2(getinput())
print(s2)
assert s2 == 242
