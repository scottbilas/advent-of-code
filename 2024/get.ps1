[CmdletBinding()]
param (
    [int]$Day
)

# to update session: copy 'session' contents from edge://settings/cookies/detail?site=adventofcode.com into ~/.adventofcode.session

$aoc = Join-Path $PSScriptRoot ../Tools/aoc-cli/aoc.exe
& $aoc download -d $Day -y 2024 -i $PSScriptRoot/aoc/day$($Day).input.txt -p $PSScriptRoot/aoc/day$($Day).results.md -m -o
