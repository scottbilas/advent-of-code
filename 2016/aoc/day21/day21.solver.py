#!/usr/bin/env python

def getinput(): return open('day21.input.txt').read()

import re

def solve(start, opstext, reverse=False):
    if reverse:
        opstext = '\n'.join(opstext.split('\n')[::-1])

    ops = re.findall(r'\w+', opstext)
    passwd, plen, tok = list(start), len(start), 0

    def rotbased(idx):
        return 1 + idx + (1 if idx >= 4 else 0)

    rrotlut = {}
    for i in range(plen):
        rrotlut[(rotbased(i) + i) % plen] = i

    while tok < len(ops):
        if ops[tok] == 'swap':
            x, y = ops[tok+2], ops[tok+5]
            if x.isdigit():
                x, y = int(x), int(y)
            else:
                x, y = passwd.index(x), passwd.index(y)
            passwd[x], passwd[y] = passwd[y], passwd[x]
            tok += 6

        elif ops[tok] == 'rotate':
            if ops[tok+1] == 'based':
                x = passwd.index(ops[tok+6])
                if reverse: x = rrotlut[x]
                rot = rotbased(x)
                tok += 7
            else:
                rot = (1 if ops[tok+1] == 'right' else -1) * int(ops[tok+2])
                tok += 4

            if reverse: rot *= -1
            passwd = [passwd[(p-rot) % plen] for p in range(plen)]

        elif ops[tok] == 'reverse':
            sub = slice(int(ops[tok+2]), int(ops[tok+4])+1)
            passwd[sub] = passwd[sub][::-1]
            tok += 5

        else: # move
            x, y = int(ops[tok+2]), int(ops[tok+5])
            if reverse: x, y = y, x
            c = passwd[x]
            del passwd[x]
            passwd.insert(y, c)
            tok += 6

    return ''.join(passwd)


### PART 1

# sample

def sample():
    passwds = [
        'ebcda', 'edcba', 'abcde', 'bcdea', 'bdeac', 'abdec', 'ecabd', 'decab']
    opstexts = '''
        swap position 4 with position 0
        swap letter d with letter b
        reverse positions 0 through 4
        rotate left 1 step
        move position 1 to position 4
        move position 3 to position 0
        rotate based on position of letter b
        rotate based on position of letter d
        '''.strip().split('\n')

    passwd = 'abcde'
    for expected, opstext in zip(passwds, opstexts):
        passwd = solve(passwd, opstext)
        assert passwd == expected

# problem

s1 = solve('abcdefgh', getinput())
print(s1)
assert s1 == 'dbfgaehc'


### PART 2

s2 = solve('fbgdceah', getinput(), True)
print(s2)
assert s2 == 'aghfcdeb'
