#Requires -Version 7
#Requires -Modules Native
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# setup:
#
# scoop install python latex pandoc
#
# irm https://install.python-poetry.org | python -
# ^^ don't use iwr, you get bytes instead of a string back from .Content (see https://stackoverflow.com/a/51334763/14582)
# add '~\AppData\Roaming\Python\Scripts' to path
#
# cd advent-of-code/2021
# poetry install

try {
    . .venv\Scripts\activate.ps1
    Push-Location $PSScriptRoot\aoc

    $tempnbs = Get-ChildItem *.ipynb | Where-Object { $_ -match 'day(\d+)' } | ForEach-Object {
        if (Get-Content "day$($matches[1]).results.txt") { # skip any incomplete/in-progress solutions
            $tempnb = $_.fullname.replace('.solver.ipynb', '.temp.ipynb')

            New-Item -force lightmode >$null
            iee jupyter nbconvert --to notebook --execute --output $tempnb $_
            Remove-Item -ea:silent lightmode

            Resolve-Path $tempnb
        }
    }

    Set-Location ..
    $merged = 'Advent of Code 2021.ipynb'
    nbmerge @tempnbs > $merged
    Remove-Item $tempnbs

    iee jupyter nbconvert --to pdf $merged
    Remove-Item $merged
}
finally {
    Pop-Location
    deactivate
}
