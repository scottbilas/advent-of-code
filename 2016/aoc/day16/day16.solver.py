#!/usr/bin/env python

def getinput(): return open('day16.input.txt').read().strip()

def step(a):
    return a + ['0'] + ['1' if c == '0' else '0' for c in a[::-1]]

def generate(str, count):
    str = [c for c in str]
    while len(str) < count:
        str = step(str)
    return str[:count]

def checksum(str):
    while len(str) % 2 == 0:
        str = [str[i*2] == str[i*2+1] for i in range(len(str)//2)]
    return ''.join('1' if c else '0' for c in str)

def solve(text, count): return checksum(generate(text, count))

# tests

def t_step(str): return ''.join(step(list(str)))
assert t_step('1') == '100'
assert t_step('1') == '100'
assert t_step('0') == '001'
assert t_step('11111') == '11111000000'
assert t_step('111100001010') == '1111000010100101011110000'

assert checksum('110010110100') == '100'


### PART 1

# samples

assert solve('10000', 20) == '01100'

# problem

s1 = solve(getinput(), 272)
print(s1)
assert s1 == '10010110010011110'


### PART 2

# 1m21 originally

s2 = solve(getinput(), 35651584)
print(s2)
assert s2 == '01101011101100011'
