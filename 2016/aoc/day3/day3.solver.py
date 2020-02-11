#!/usr/bin/env python

input = [
    [int(n) for n in l.split()]
    for l in open('day3.input.txt').readlines()]

### PART 1

def solve1(tris):
    valid = 0
    for tri in tris:
        tri.sort()
        valid += 1 if tri[0] + tri[1] > tri[2] else 0
    return valid

# samples

assert solve1([[5, 10, 25]]) == 0
assert solve1([[5, 10, 14]]) == 1

# problem

s1 = solve1(input)
print(s1)
assert s1 == 993


### PART 2


# samples


# problem
