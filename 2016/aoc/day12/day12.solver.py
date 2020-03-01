#!/usr/bin/env python

def getinput(): return open('day12.input.txt').read()

def run(c): # hand-disassembled

    a, b, d = 1, 1, 26 if c == 0 else 26+7

    for _ in range(d):
        c = a
        a += b
        b = c

    return a + 17 * 16


### PART 1

import re

def solve1(program, c=0):
    regs = { 'a': 0, 'b': 0, 'c': c, 'd': 0 }
    instrs = [line.split() for line in program.strip().splitlines()]
    ip = 0

    while ip < len(instrs):
        instr, ip = instrs[ip], ip + 1
        op, x = instr[0], instr[1]
        if   op == 'cpy': regs[instr[2]] = int(regs.get(x, x))
        elif op == 'inc': regs[x] += 1
        elif op == 'dec': regs[x] -= 1
        elif op == 'jnz': ip += int(instr[2]) - 1 if int(regs.get(x, x)) != 0 else 0

    return regs['a']

# samples

assert solve1("""
    cpy 41 a
    inc a
    inc a
    dec a
    jnz a 2
    dec a
    """) == 42

# problem

s1 = solve1(getinput())
print(s1)
assert s1 == run(0) == 318083


### PART 2

s2 = run(1)
print(s2)
assert s2 == 9227737
