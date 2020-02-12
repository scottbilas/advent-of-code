#!/usr/bin/env python

input = open('day8.input.txt').read().splitlines()


### PART 1

import re

def solve1(cx, cy, instrs):
    grid = [['.'] * cx for _ in range(cy)]
    for instr in instrs:
        nums = [int(v) for v in re.findall('\d+', instr)]
        if 'rect' in instr:
            w, h = nums
            for y in range(h):
                for x in range(w):
                    grid[y][x] = '#'
        elif 'column' in instr:
            x, step = nums
            copy = [grid[y][x] for y in range(cy)]
            for y in range(cy):
                grid[y][x] = copy[(y - step) % cy]
        elif 'row' in instr:
            y, step = nums
            copy = [grid[y][x] for x in range(cx)]
            for x in range(cx):
                grid[y][x] = copy[(x - step) % cx]
    return sum(sum(1 for x in y if x == '#') for y in grid)

# samples

assert solve1(7, 3, [
    'rect 3x2',
    'rotate column x=1 by 1',
    'rotate row y=0 by 4',
    'rotate column x=1 by 1',
    ]) == 6

# problem

s1 = solve1(50, 6, input)
print(s1)
assert s1 == 119


### PART 2


# samples


# problem
