from libaoc import *

from os.path import exists
dark = not exists('lightmode')

def plotStyle(extra=None):
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
