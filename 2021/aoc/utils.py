import itertools
import math
import matplotlib.pyplot as plt
import matplotlib as mpl
import numpy as np
import re
import sys
from dataclasses import dataclass
from os.path import exists
from functools import reduce
dark = not exists('lightmode')

def check(result, expected):
    assert result == expected, f"result: '{result}'', expected: '{expected}'"

def _check(result, index):
    global _results
    print(f"Part {index+1} Result: {result}")
    if _results:
        check(str(result), _results[index])
    else:
        print("NO VALIDATED RESULT YET")

def check1(result1):
    _check(result1, 0)

def check2(result2):
    _check(result2, 1)

def flatten(l):
    if len(l) == 1:
        if type(l[0]) == list:
            result = flatten(l[0])
        else:
            result = l
    elif type(l[0]) == list:
        result = flatten(l[0]) + flatten(l[1:])
    else:
        result = [l[0]] + flatten(l[1:])
    return result

def plotStyle(extra=None):
    if extra:
        for k, v in extra.items():
            mpl.rcParams[k] = v

def plotInvertY(titleY):
    mpl.rcParams['axes.titley'] = titleY
    mpl.rcParams['xtick.top'] = True
    mpl.rcParams['xtick.bottom'] = False
    mpl.rcParams['xtick.labeltop'] = True
    mpl.rcParams['xtick.labelbottom'] = False
    plt.gca().invert_yaxis()

def initDay(day, resultsSpec=None):
    if dark:
        plt.style.use('dark_background')

    mpl.rcParams['axes.labelsize'] = 11
    mpl.rcParams['axes.titlelocation'] = 'left'
    mpl.rcParams['axes.titlesize'] = 13
    mpl.rcParams['axes.xmargin'] = .03
    mpl.rcParams['axes.ymargin'] = .2
    mpl.rcParams['figure.figsize'] = (10, 2)

    if dark:
        plt.style.use('dark_background')
        mpl.rcParams['axes.facecolor'] = '#071318'
        mpl.rcParams['axes.labelcolor'] = '#abacad'
        mpl.rcParams['axes.linewidth'] = 0
        mpl.rcParams['axes.titlecolor'] = '#c7985d'
        mpl.rcParams['figure.facecolor'] = '#131a1f'
        mpl.rcParams['xtick.color'] = '#abacad'
        mpl.rcParams['ytick.color'] = '#abacad'

    inputText = open(f"{day}.input.txt").read().replace('\r\n', '\n')

    resultsText = open(f"{day}.results.txt").read().replace('\r\n', '\n')

    global _results
    _results = re.findall('Your puzzle answer was (\S+)\.', resultsText)

    if not resultsSpec:
        return inputText

    results = [inputText]
    end = 0
    for spec in resultsSpec:
        begin = resultsText.index(spec, end) + len(spec)
        while resultsText[begin].isspace():
            begin += 1
        end = resultsText.index('\n\n', begin)
        results.append(resultsText[begin:end])

    return tuple(results)
