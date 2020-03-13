#!/usr/bin/env python

def getinput(): return open('day14.input.txt').read().strip()

import hashlib, itertools, re

def solve(salt, repeat):

    def hashstr(s):
        return hashlib.md5(s.encode('utf-8')).hexdigest()

    class HashDict(dict):
        def __missing__(self, key):
            hash = hashstr(salt + str(key))
            for i in range(repeat):
                hash = hashstr(hash)
            self[key] = hash
            return hash

    hashed, found = HashDict(), 0
    for index in itertools.count():
        three = re.search(r'(.)\1\1', hashed[index])
        if three:
            five = three.group(0)[0] * 5
            for seek in range(index + 1, index + 1001):
                if (five in hashed[seek]):
                    found += 1
                    if (found == 64):
                        return index
                    break


### PART 1

def solve1(salt): return solve(salt, 0)

# samples

assert solve1('abc') == 22728

# problem

s1 = solve1(getinput())
print(s1)
assert s1 == 25427


### PART 2

def solve2(salt): return solve(salt, 2016)

# samples

assert solve2('abc') == 22551

# problem

s2 = solve2(getinput())
print(s2)
assert s2 == 22045
