#!/usr/bin/env python

inputData = [l.strip() for l in open('day2.input.txt').readlines()]

def parse(start, text):
    p, d = (0, 0), {}
    for y, line in enumerate(text.splitlines()):
        for x, c in enumerate(line):
            if not c.isspace():
                d[(x, y)] = c
                if c == str(start):
                    p = x, y
    return p, d

def solve(instrs, start, buttons):
    offsets = {'U': (0, -1), 'R': (1, 0), 'D': (0, 1), 'L': (-1, 0)}
    (x, y), buttons = parse(start, buttons)
    code = ''
    for instr in instrs:
        for move in instr:
            xa, ya = x + offsets[move][0], y + offsets[move][1]
            if (xa, ya) in buttons:
                x, y = xa, ya
        code += buttons[(x, y)]
    return code


### PART 1

def solve1(instrs): return solve(instrs, 1, "123\n456\n789")

# samples

assert solve1(['ULL', 'RRDDD', 'LURDL', 'UUUUD']) == '1985'

# problem

s1 = solve1(inputData)
print(s1)
assert s1 == '95549'


### PART 2

def solve2(instrs): return solve(instrs, 5, """
      1
     234
    56789
     ABC
      D""")

# samples

assert solve2(['ULL', 'RRDDD', 'LURDL', 'UUUUD']) == '5DB3'

# problem

s2 = solve2(inputData)
print(s2)
assert s2 == 'D87AD'
