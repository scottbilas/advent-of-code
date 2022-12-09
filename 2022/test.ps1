1..25 | %{ "aoc/day$_.solver.ts" } | ?{ test-path $_ } | %{
    echo "Testing $_"
    node --nolazy -r ts-node/register $_
}
