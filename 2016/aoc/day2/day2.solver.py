#!/usr/bin/env python

input = [l.strip() for l in open('day2.input.txt').readlines()]

### PART 1

def solve1(instrs):
    offsets = {'U': (0, -1), 'R': (1, 0), 'D': (0, 1), 'L': (-1, 0)}
    code, x, y = '', 0, 0
    for instr in instrs:
        for move in instr:
            x = min(2, max(0, x + offsets[move][0]))
            y = min(2, max(0, y + offsets[move][1]))
        code += str(y * 3 + x + 1)
    return code

# samples

assert solve1(['ULL', 'RRDDD', 'LURDL', 'UUUUD']) == '1985'

# problem

s1 = solve1(input)
print(s1)
assert s1 == '95549'


### PART 2


# samples


# problem
