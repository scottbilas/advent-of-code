[CmdletBinding()]
param (
    [int]$Day = [datetime]::now.day,
    [int]$Year = [datetime]::now.year
)

# aoc requires 'session' contents from edge://settings/cookies/detail?site=adventofcode.com in ~/.adventofcode.session

$aoc = "$PSScriptRoot/../Tools/aoc-cli/aoc.exe"
$path = "$PSScriptRoot/aoc"
$base = "$path/day$($Day)"

& $aoc download -d $Day -y $Year -i "$base.input.txt" -p "$base.results.md" -m -o

if (!(Test-Path "$base.solver.cs")) {
    Copy-Item "$path/../solver.template" "$base.solver.cs"
}
