dir -Recurse day*.linq | %{
    write-host -nonew "Testing $($_.Name)..."

    $answers = cat $_.FullName.Replace(".linq", ".txt") |
        ?{ $_ -match "Your puzzle answer was (.*)\.$" } |
        %{ $matches[1] }

    $results = lprun $_

    if ([string]$results -eq [string]$answers) {
        write-host ($results -join ', ')
    }
    else {
        throw "Mismatch! Results: $results; Expected: $answers"
    }
}
