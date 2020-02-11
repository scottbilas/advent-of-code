#!/usr/bin/env python

input = open('day1.input.txt').read().replace(',','').split()

### PART 1

def solve1(moves):
    offsets = [(0, -1), (1, 0), (0, 1), (-1, 0)]
    x, y, dir = 0, 0, 0
    for move in moves:
        dir += 1 if move[0] == 'R' else -1
        offset = offsets[dir % 4]
        dist = int(move[1:])
        x += offset[0] * dist
        y += offset[1] * dist
    return abs(x) + abs(y)

# samples

assert solve1(['R2', 'L3']) == 5
assert solve1(['R2', 'R2', 'R2']) == 2
assert solve1(['R5', 'L5', 'R5', 'R3']) == 12

# problem

s1 = solve1(input)
print(s1)
assert s1 == 146


### PART 2


# samples

# problem
