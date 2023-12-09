day = 7
include("utils.jl")
using Chain, DataStructures, Match, StatsBase

input = getProblemInput()

sample = getSampleLines("""
    32T3K 765
    T55J5 684
    KK677 28
    KTJJT 220
    QQQJA 483
""")

@enum Strength HighCard OnePair TwoPair ThreeOfAKind FullHouse FourOfAKind FiveOfAKind

function compare(hand1, hand2, strength, value)
    rcomp = Int(strength(hand1)) - Int(strength(hand2))
    if rcomp != 0 return rcomp end

    for i in 1:5
        ccomp = value(hand1[i]) - value(hand2[i])
        if ccomp != 0 return ccomp end
    end
end

strength1 = cards -> @match (
    @chain cards countmap values collect sort(_, rev=true)) begin
        [5,    _...] => FiveOfAKind
        [4, 1, _...] => FourOfAKind
        [3, 2, _...] => FullHouse
        [3,    _...] => ThreeOfAKind
        [2, 2, _...] => TwoPair
        [2,    _...] => OnePair
        _ => HighCard
    end

value1 = card -> @match card begin
    'A' => 14; 'K' => 13; 'Q' => 12; 'J' => 11; 'T' => 10
    _ => parse(Int, card)
end

solve = (lines, strength, value) -> @chain lines begin
    split.(" ")
    sort(_, lt = (x, y) -> compare(x[1], y[1], strength, value) < 0)
    enumerate
    map(((rank, card),) -> rank * parse(Int, card[2]), _)
    sum
end

solve1 = lines -> solve(lines, strength1, value1)

check("Day $day.1 Sample",  () -> solve1(sample),     6440)
check("Day $day.1 Problem", () -> solve1(input), 250347426)

strength2 = cards -> @match (
    strength1(cards), count(x -> x == 'J', cards)) begin
        ($FourOfAKind,  1 || 4) => FiveOfAKind
        ($FullHouse,    2 || 3) => FiveOfAKind
        ($ThreeOfAKind, 1 || 3) => FourOfAKind
        ($ThreeOfAKind,      2) => FiveOfAKind
        ($TwoPair,           1) => FullHouse
        ($TwoPair,           2) => FourOfAKind
        ($OnePair,      1 || 2) => ThreeOfAKind
        ($HighCard,          1) => OnePair
        (strength,           _) => strength
    end

solve2 = lines -> solve(lines, strength2, c -> c == 'J' ? 0 : value1(c))

check("Day $day.2 Sample",  () -> solve2(sample),     5905)
check("Day $day.2 Problem", () -> solve2(input), 251224870)
