#!/usr/bin/env python

def getinput(): return open('day16.input.txt').read().strip()

def tobits(text):
    return [c == '1' for c in text]

def frombits(bits):
    return ''.join('1' if b else '0' for b in bits)

def step(bits):
    return bits + [False] + [not c for c in bits[::-1]]

def generate(bits, count):
    while len(bits) < count:
        bits = step(bits)
    return bits[:count]

def checksum(bits):
    while len(bits) % 2 == 0:
        bits = [c[0] == c[1] for c in zip(bits[::2], bits[1::2])]
    return bits

def solve(text, target):
    return frombits(checksum(generate(tobits(text), target)))

# tests

def t_step(text): return frombits(step(tobits(text)))

assert t_step('1') == '100'
assert t_step('0') == '001'
assert t_step('11111') == '11111000000'
assert t_step('111100001010') == '1111000010100101011110000'

def t_checksum(text): return frombits(checksum(tobits(text)))

assert t_checksum('110010110100') == '100'
assert t_checksum('10000011110010000111') == '01100'


### PART 1

# samples

assert solve('10000', 20) == '01100'

# problem

s1 = solve(getinput(), 272)
print(s1)
assert s1 == '10010110010011110'


### PART 2

s2 = solve(getinput(), 35651584)
print(s2)
assert s2 == '01101011101100011'
