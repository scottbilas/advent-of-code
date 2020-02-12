#!/usr/bin/env python

def getinput(): return open('day5.input.txt').read().strip()

import hashlib, itertools


### PART 1

def solve1(id):
    code = ''
    for i in itertools.count():
        hash = hashlib.md5((id + str(i)).encode('utf-8')).hexdigest()
        if hash.startswith('00000'):
            code += hash[5]
            if len(code) == 8:
                return code

# samples

assert solve1('abc') == '18f47a30'

# problem

s1 = solve1(input)
print(s1)
assert s1 == '801b56a7'


### PART 2

def solve2(id):
    code, used = '_'*8, 0
    for i in itertools.count():
        hash = hashlib.md5((id + str(i)).encode('utf-8')).hexdigest()
        if hash.startswith('00000'):
            pos = int(hash[5], 16)
            if pos < 8 and code[pos] == '_':
                code = code[:pos] + hash[6] + code[pos+1:]
                used += 1
                if used == 8:
                    return code

# samples

assert solve2('abc') == '05ace8e3'

# problem

s2 = solve2(getinput())
print(s2)
assert s2 == '424a0197'
