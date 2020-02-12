#!/usr/bin/env python

inputData = open('day1.input.txt').read().replace(',','').split()


### PART 1

def solve1(moves, visitor=None):
    offsets = [(0, -1), (1, 0), (0, 1), (-1, 0)]
    x, y, dir = 0, 0, 0
    for move in moves:
        dir += 1 if move[0] == 'R' else -1
        offset = offsets[dir % 4]
        for i in range(int(move[1:])):
            x, y = x + offset[0], y + offset[1]
            if visitor and visitor(x, y):
                return abs(x) + abs(y)
    return abs(x) + abs(y)

# samples

assert solve1(['R2', 'L3']) == 5
assert solve1(['R2', 'R2', 'R2']) == 2
assert solve1(['R5', 'L5', 'R5', 'R3']) == 12

# problem

s1 = solve1(inputData)
print(s1)
assert s1 == 146


### PART 2

def solve2(moves):
    visited = set()
    def visitor(x, y):
        if (x, y) in visited: return True
        visited.add((x, y))
        return False
    return solve1(moves, visitor)

# samples

assert solve2(['R8', 'R4', 'R4', 'R8']) == 4

# problem

s2 = solve2(inputData)
print(s2)
assert s2 == 131
