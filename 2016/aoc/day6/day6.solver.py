#!/usr/bin/env python

def getinput(): return open('day6.input.txt').read().splitlines()

def getsample(): return [
    'eedadn', 'drvtee', 'eandsr', 'raavrd',
    'atevrs', 'tsrnev', 'sdttsa', 'rasrtv',
    'nssdts', 'ntnada', 'svetve', 'tesnvt',
    'vntsnd', 'vrdear', 'dvrsen', 'enarar']

def solve(messages, sort):
    ds = [{} for _ in range(len(messages[0]))]
    for message in messages:
        for i in range(len(message)):
            c, d = message[i], ds[i]
            d[c] = d.get(c, 0) + 1
    return ''.join([sorted(d.items(), key=lambda kv: sort*kv[1])[0][0] for d in ds])


### PART 1

def solve1(messages): return solve(messages, -1)

# samples

assert solve1(getsample()) == 'easter'

# problem

s1 = solve1(getinput())
print(s1)
assert s1 == 'afwlyyyq'


### PART 2

def solve2(messages): return solve(messages, 1)

# samples

assert solve2(getsample()) == 'advent'

# problem

s2 = solve2(getinput())
print(s2)
assert s2 == 'bhkzekao'
