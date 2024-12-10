[CmdletBinding()]
param (
    [int]$Day = [datetime]::now.day,
    [int]$Year = [datetime]::now.year
)

# aoc requires 'session' contents from edge://settings/cookies/detail?site=adventofcode.com in ~/.adventofcode.session

$aoc = "$PSScriptRoot/../Tools/aoc-cli/aoc.exe"
$path = "$PSScriptRoot/aoc"
$base = "$path/day$($Day)"

function download($d) {
    & $aoc download -d $d -y $Year -i "$base.input.txt" -p "$base.results.md" -m -o
}

if ($Day -eq 0) {
    foreach ($d in 1..25) {
        download $d
    }
}
else {
    download $Day
}

if (!(Test-Path "$base.solver.cs")) {
    $code = Get-Content "$path/../solver.template"
    $code = $code -replace '{{Day}}', $Day
    $code | Set-Content "$base.solver.cs"
    "Wrote $base.solver.cs"
}
