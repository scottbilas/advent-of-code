#!/usr/bin/env python

def getinput(): return open('day18.input.txt').read().strip()

def solve(row, count):
    last = [1] + [c == '.' for c in row] + [1]
    cur, safe = last.copy(), sum(last)

    for y in range(count-1):
        for x in range(1, len(last)-1):
            cur[x] = last[x-1] == last[x+1]
        safe += sum(cur)
        last, cur = cur, last

    return safe - count*2


### PART 1

# samples

assert solve('..^^.', 3) == 6
assert solve('.^^.^.^^^^', 10) == 38

# problem

s1 = solve(getinput(), 40)
print(s1)
assert s1 == 1982


### PART 2

s2 = solve(getinput(), 400000)
print(s2)
assert s2 == 20005203
