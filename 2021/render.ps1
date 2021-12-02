#Requires -Version 7
#Requires -Modules Native
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# setup:
#
# pip install jupyter nbconvert
# scoop install latex pandoc poppler

Push-Location
try {
    $pdfs = Get-ChildItem "$PSScriptRoot\aoc" | Where-Object { $_ -match 'day(\d+)' } | ForEach-Object {
        $day = $matches[1]
        Set-Location $_
        if (Get-Content "day$day.input.txt") {

            New-Item -force lightmode >$null
            $tempnb = "Day $day.ipynb"
            iee jupyter nbconvert --to notebook --execute --output $tempnb "day$day.solver.ipynb"
            Remove-Item lightmode

            iee jupyter nbconvert --to pdf $tempnb
            Remove-Item $tempnb

            # yield path
            Resolve-Path "Day $day.pdf"
        }
    }
}
finally {
    Pop-Location
}

iee pdfunite @pdfs "Advent of Code 2021.pdf"
Remove-Item $pdfs
