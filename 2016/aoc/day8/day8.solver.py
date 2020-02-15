#!/usr/bin/env python

def getinput(): return open('day8.input.txt').read().splitlines()

import re, pytesseract
from PIL import Image, ImageOps

def solve(cx, cy, instrs):
    grid = [['.'] * cx for _ in range(cy)]
    for instr in instrs:
        nums = [int(v) for v in re.findall('\d+', instr)]
        if 'rect' in instr:
            w, h = nums
            for y in range(h):
                for x in range(w):
                    grid[y][x] = '#'
        elif 'column' in instr:
            x, step = nums
            copy = [grid[y][x] for y in range(cy)]
            for y in range(cy):
                grid[y][x] = copy[(y - step) % cy]
        elif 'row' in instr:
            y, step = nums
            copy = [grid[y][x] for x in range(cx)]
            for x in range(cx):
                grid[y][x] = copy[(x - step) % cx]
    return grid


### PART 1

def solve1(cx, cy, instrs):
    return sum(sum(1 for x in y if x == '#') for y in solve(cx, cy, instrs))

# samples

assert solve1(7, 3, [
    'rect 3x2',
    'rotate column x=1 by 1',
    'rotate row y=0 by 4',
    'rotate column x=1 by 1',
    ]) == 6

# problem

s1 = solve1(50, 6, getinput())
print(s1)
assert s1 == 119


### PART 2

def solve2(cx, cy, instrs):
    grid = solve(cx, cy, instrs)

    # render black on white, which tesseract likes better
    img = Image.frombytes('L', (cx, cy), bytes(
        (255 if grid[y][x] == '.' else 0) for y in range(cy) for x in range(cx)))

    # tesseract also needs bigger text and a border
    img = ImageOps.expand(img.resize((cx * 4, cy * 4)), 20, 255)

    return pytesseract.image_to_string(img)

s2 = solve2(50, 6, getinput())
print(s2)
assert s2 == 'ZFHFSFOGPO'
