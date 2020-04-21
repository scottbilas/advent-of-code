#!/usr/bin/env python

def getinput(): return open('day25.input.txt').read()

import itertools
from itertools import islice

def run_vm(a):
    instrs = [l.split() + [''] for l in getinput().strip().splitlines()]
    ip, regs = 0, { 'a': a, 'b': 0, 'c': 0, 'd': 0 }

    def get(v): return int(regs.get(v, v))

    while ip < len(instrs):

        instr, ip = instrs[ip], ip + 1
        op, x, y = instr[0], instr[1], instr[2]

        if   op == 'cpy': regs[y] = get(x)
        elif op == 'inc': regs[x] += 1
        elif op == 'dec': regs[x] -= 1
        elif op == 'jnz': ip += get(y) - 1 if get(x) != 0 else 0
        elif op == 'out': yield get(x)

BASE = 2538 # see init portion of disasm.txt

# hand reassembled - simplified, optimized, functionally equivalent to run_vm
def run_optimized(seed):
    init = val = seed + BASE
    while True:
        yield val % 2
        val //= 2
        if val == 0:
            val = init

def solve_optimized():
    for seed in itertools.count():
        signal = list(islice(run_optimized(seed), 20))
        if signal[0::2] == [0]*10 and signal[1::2] == [1]*10:
            return seed

def validate():
    for seed in range(20):
        for x, y in islice(zip(run_vm(seed), run_optimized(seed)), 20):
            assert x == y

#validate()

# reverse engineered - build up 0b01010101 until it's bigger than the BASE, then
# the offset is the seed, and our answer.
def solve_trivial():
    v = 0
    for i in itertools.count(1, 2):
        v |= 1 << i
        if v > BASE:
            return v - BASE

s1 = solve_trivial()
print(s1)
assert solve_optimized() == s1
assert s1 == 192
