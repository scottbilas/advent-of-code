#!/usr/bin/env python

input = open('day6.input.txt').read().splitlines()


### PART 1

def solve1(messages):
    ds = [{} for _ in range(len(messages[0]))]
    for message in messages:
        for i in range(len(message)):
            c, d = message[i], ds[i]
            d[c] = d.get(c, 0) + 1
    return ''.join([sorted(d.items(), key=lambda kv: -kv[1])[0][0] for d in ds])

# samples

assert solve1([
    'eedadn',
    'drvtee',
    'eandsr',
    'raavrd',
    'atevrs',
    'tsrnev',
    'sdttsa',
    'rasrtv',
    'nssdts',
    'ntnada',
    'svetve',
    'tesnvt',
    'vntsnd',
    'vrdear',
    'dvrsen',
    'enarar',
    ]) == 'easter'

# problem

s1 = solve1(input)
print(s1)
assert s1 == 'afwlyyyq'


### PART 2


# samples


# problem
