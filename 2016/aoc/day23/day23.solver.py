#!/usr/bin/env python

def getinput(): return open('day23.input.txt').read()

### PART 1

import re

def solve(program, a=0, hack=False):

    instrs = [l.split() + [''] for l in program.strip().splitlines()]
    tgl = { 'cpy': 'jnz', 'inc': 'dec', 'dec': 'inc', 'jnz': 'cpy', 'tgl': 'inc' }
    ip, regs = 0, { 'a': a, 'b': 0, 'c': 0, 'd': 0 }

    def get(v): return int(regs.get(v, v))

    while ip < len(instrs):

        if hack and ip == 4:
            regs['a'] = get('a') + get('b') * get('d')
            regs['b'] = get('b') - 1
            regs['c'] = 2 * get('b')
            ip = 16
            continue

        instr, ip = instrs[ip], ip + 1
        op, x, y = instr[0], instr[1], instr[2]

        if   op == 'cpy': regs[y] = get(x)
        elif op == 'inc': regs[x] += 1
        elif op == 'dec': regs[x] -= 1
        elif op == 'jnz': ip += get(y) - 1 if get(x) != 0 else 0
        elif op == 'tgl':
            ipx = ip + get(x) - 1
            if 0 <= ipx < len(instrs):
                instrs[ipx][0] = tgl[instrs[ipx][0]]

    return regs['a']

# sample

assert solve('''
    cpy 2 a
    tgl a
    tgl a
    tgl a
    cpy 1 a
    dec a
    dec a''') == 3

# problem

s1 = solve(getinput(), 7)
print(s1)
assert s1 == 11662 == solve(getinput(), 7, True)


### PART 2

s2 = solve(getinput(), 12, True)
print(s2)
assert s2 == 479008222
