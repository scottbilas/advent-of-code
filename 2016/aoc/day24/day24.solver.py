#!/usr/bin/env python

def getinput(): return open('day24.input.txt').read()

### PART 1

from collections import deque
import itertools

def solve(text, roundtrip=False):

    walls, pois = set(), {}
    for y, l in enumerate([l.strip() for l in text.strip().splitlines()]):
        for x, c in enumerate(l):
            if c == '#': walls.add((x, y))
            elif c.isdigit(): pois[int(c)] = (x, y)

    poisr = {v: k for k, v in pois.items()}

    def walk(src):
        start = pois[src]
        work = deque([(start, 0)])

        visited = walls.copy()
        visited.add(start)

        while len(work):
            nxt, steps = work.popleft()
            dst = poisr.get(nxt)
            if dst not in [src, None]:
                yield dst, steps

            for adj in [(0,-1), (1,0), (0,1), (-1,0)]:
                adj = nxt[0]+adj[0], nxt[1]+adj[1]
                if adj not in visited:
                    work.append((adj, steps+1))
                    visited.add(adj)

    costs = {(src, dst): steps
        for src in pois.keys()
        for dst, steps in walk(src)}

    tries = [p for p in itertools.permutations(pois.keys()) if p[0] == 0]
    if roundtrip:
        tries = [p + (0,) for p in tries]

    minsteps = 10000000
    for t in tries:
        minsteps = min(sum([costs[p] for p in zip(t[0:], t[1:])]), minsteps)

    return minsteps


# sample

assert solve('''
    ###########
    #0.1.....2#
    #.#######.#
    #4.......3#
    ###########
    ''') == 14

# problem

s1 = solve(getinput())
print(s1)
assert s1 == 428


### PART 2

s2 = solve(getinput(), True)
print(s2)
assert s2 == 680
