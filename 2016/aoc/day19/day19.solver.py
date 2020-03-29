#!/usr/bin/env python

def getinput(): return int(open('day19.input.txt').read())


### PART 1

def solve1(count):
    counts = [1 for i in range(count)]
    nexts = [i+1 for i in range(count)]
    nexts[-1], i = 0, 0

    while True:
        n = nexts[i]
        counts[i] += counts[n]
        nn = nexts[n]
        if i == nn:
            return i+1
        nexts[i], i = nn, nn

# samples

assert solve1(5) == 3
assert solve1(9) == 3

# problem

s1 = solve1(getinput())
print(s1)
assert s1 == 1815603


### PART 2

# samples

assert solve2(5) == 2

# problem

s2 = solve2(getinput())
print(s2)
#assert s2 == 20005203
