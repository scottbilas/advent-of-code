import re
import matplotlib.pyplot as plt
import matplotlib as mpl

from os.path import exists
dark = not exists('lightmode')

def check(result, expected):
    assert result == expected, f"result: '{result}'', expected: '{expected}'"

def check1(result1):
    global _results
    check(str(result1), _results[0])
    print(f"Part 1 Result: {result1}")

def check2(result2):
    global _results
    check(str(result2), _results[1])
    print(f"Part 2 Result: {result2}")

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

def initDay(day):
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

    global _results
    _results = re.findall('Your puzzle answer was (\S+)\.', open(f"{day}.results.txt").read())
    return open(f"{day}.input.txt").read()
