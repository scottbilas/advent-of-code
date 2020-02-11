#!/usr/bin/env python

input = open('day5.input.txt').read().strip()

### PART 1

import hashlib, itertools

def solve1(id):
    code = ''
    for i in itertools.count():
        hash = hashlib.md5((id + str(i)).encode('utf-8')).hexdigest()
        if hash.startswith('00000'):
            code += hash[5]
            if len(code) == 8:
                return code

# samples

assert solve1("abc") == '18f47a30'

# problem

s1 = solve1(input)
print(s1)
assert s1 == '801b56a7'


### PART 2


# samples


# problem
