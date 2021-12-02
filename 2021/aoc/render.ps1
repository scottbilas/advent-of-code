#Requires -Version 7
#Requires -Modules Native
Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

Get-ChildItem day* | Where-Object { $_ -match 'day(\d+)' } | ForEach-Object {
    $day = $matches[1]
    Push-Location
    Set-Location $_
    if (Get-Content "day$day.input.txt") {
        New-Item lightmode >$null
        $tempnb = "Day $day.ipynb"
        iee jupyter nbconvert --to notebook --execute --output $tempnb "day$day.solver.ipynb"
        iee jupyter nbconvert --to pdf $tempnb
        Remove-Item $tempnb
        $pdf = "day$day.solver.pdf"
        Remove-Item -ea:silent $pdf
        Move-Item "Day $day.pdf" "day$day.solver.pdf"
        Remove-Item lightmode
        Pop-Location
    }
}
