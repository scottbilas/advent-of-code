#!/usr/bin/env python

def getinput(): return open('day14.input.txt').read().strip()


### PART 1

import hashlib, itertools, re

def solve1(salt):

    class HashDict(dict):
        def __missing__(self, key):
            self[key] = hash = hashlib.md5((salt + str(key)).encode('utf-8')).hexdigest()
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

# samples

assert solve1('abc') == 22728

# problem

s1 = solve1(getinput())
print(s1)
assert s1 == 25427


### PART 2

#def solve2(favorite): return solve(favorite, None, 50)

#s2 = solve2(int(getinput()))
#print(s2)
#assert s2 == 124
