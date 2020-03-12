#!/usr/bin/env python

def getinput(): return open('day13.input.txt').read()

import sys

def iswall(favorite, x, y):
    v = x*x + 3*x + 2*x*y + y + y*y + favorite
    return bin(v).count('1') % 2 != 0

def solve(favorite, end, maxdist):
    queued, visited = [], set()

    def enter(pos):
        if pos not in visited and not iswall(favorite, pos[0], pos[1]):
            visited.add(pos)
            queued.append(pos)

    enter((1, 1))
    for dist in range(maxdist):
        work, queued = queued, []
        for pos in work:
            if pos == end: return dist
            enter((pos[0]+1, pos[1]))
            enter((pos[0], pos[1]+1))
            if pos[0] > 0: enter((pos[0]-1, pos[1]))
            if pos[1] > 0: enter((pos[0], pos[1]-1))

    return len(visited)


### PART 1

def solve1(favorite, end): return solve(favorite, end, 9999)

# samples

def render(favorite, w, h):
    for y in range(h):
        yield ''.join(('#' if iswall(favorite, x, y) else '.') for x in range(w))

assert list(render(10, 10, 7)) == [
        ".#.####.##",
        "..#..#...#",
        "#....##...",
        "###.#.###.",
        ".##..#..#.",
        "..##....#.",
        "#...##.###",
    ]

assert solve1(10, (7, 4)) == 11

# problem

s1 = solve1(int(getinput()), (31, 39))
print(s1)
assert s1 == 92


### PART 2

def solve2(favorite): return solve(favorite, None, 50)

s2 = solve2(int(getinput()))
print(s2)
assert s2 == 124
