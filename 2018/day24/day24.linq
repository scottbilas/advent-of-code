<Query Kind="Program">
  <Reference Relative="..\..\libaoc\bin\Debug\netstandard2.0\libaoc.dll">C:\proj\advent-of-code\libaoc\bin\Debug\netstandard2.0\libaoc.dll</Reference>
  <NuGetReference>morelinq</NuGetReference>
  <NuGetReference>Shouldly</NuGetReference>
  <NuGetReference>YC.QuickGraph</NuGetReference>
  <Namespace>AoC</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>MoreLinq.Extensions</Namespace>
  <Namespace>QuickGraph</Namespace>
  <Namespace>QuickGraph.Algorithms</Namespace>
  <Namespace>Shouldly</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.Collections</Namespace>
  <Namespace>System.Collections.Generic</Namespace>
  <Namespace>System.Drawing</Namespace>
  <Namespace>System.Linq</Namespace>
  <Namespace>System.Text</Namespace>
</Query>

string scriptDir = Path.GetDirectoryName(Util.CurrentQueryPath);

void Main()
{
    // sample part 1
    Sim($"{scriptDir}/sample.txt").ShouldBe((immune: 0, infection: 5216));

    // sample part 2
    var sample = FindMinimalBoost($"{scriptDir}/sample.txt");
    sample.ShouldBe(51);
    Sim($"{scriptDir}/sample.txt", 1570).ShouldBe((immune: sample, infection: 0));

    // problem part 1
    var part1 = Sim($"{scriptDir}/input.txt");
    part1.infection.Dump();
    part1.ShouldBe((immune: 0, infection: 16678));

    // problem part 2
    FindMinimalBoost($"{scriptDir}/input.txt").Dump().ShouldBe(3758);
}

enum Side { ImmuneSystem, Infection };

class AttackGroup
{
    public int Id;
    public Side Side;
    public int Units;
    public int HP;
    public string[] Immunities, Weaknesses;
    public int AttackDamage;
    public string AttackType;
    public int Initiative;
    
    public int Power => Units * AttackDamage;
    
    public int PowerAgainst(AttackGroup defender)
    {
        if (defender.Immunities?.Contains(AttackType) == true)
            return 0;
        if (defender.Weaknesses?.Contains(AttackType) == true)
            return Power * 2;
        return Power;
    }

    public override string ToString() => $"#{Id} {Side} p={Power} u={Units}";
}

IEnumerable<AttackGroup> Parse(Side side, string text, int boost = 0)
    => Regex
        .Matches(text,
            @"(?<units>\d+) units each with (?<hp>\d+) hit points (?:\((?<traits>[^)]+)\) )?with an "+
            @"attack that does (?<attack>\d+) (?<type>\w+) damage at initiative (?<initiative>\d+)")
        .Cast<Match>()
        .Select((match, i) =>
        {
            var attackGroup = new AttackGroup
            {
                Id = i + 1,
                Side = side,
                Units = int.Parse(match.Groups["units"].Value),
                HP = int.Parse(match.Groups["hp"].Value),
                AttackDamage = int.Parse(match.Groups["attack"].Value) + boost,
                AttackType = match.Groups["type"].Value,
                Initiative = int.Parse(match.Groups["initiative"].Value)
            };
            
            foreach (var tmatch in Regex
                .Matches(match.Groups["traits"].Value, @"(weak|immune) to (?:(?:, )?(\w+))*")
                .Cast<Match>())
            {
                var traits = tmatch.Groups[2].Captures.Cast<Capture>().Select(c => c.Value).ToArray();
                if (tmatch.Groups[1].Value == "immune")
                    attackGroup.Immunities = traits;
                else
                    attackGroup.Weaknesses = traits;
            }
            
            return attackGroup;
        });

(int immune, int infection) Sim(string path, int boost = 0)
{
    var specs = Regex.Split(File.ReadAllText(path), "Infection:");
    
    var immuneSystem = Parse(Side.ImmuneSystem, specs[0], boost).ToList();
    var infection = Parse(Side.Infection, specs[1]).ToList();
    var bothArmies = immuneSystem.Concat(infection);

    var attackers = new Dictionary<AttackGroup, AttackGroup>();
    var defenders = new HashSet<AttackGroup>();

    while (immuneSystem.Any() && infection.Any())
    {
        attackers.Clear();
        defenders.Clear();
        
        var selectOrder = bothArmies
            .OrderByDescending(g => g.Power)
            .ThenByDescending(g => g.Initiative);
        foreach (var attacker in selectOrder)
        {
            var chosen = bothArmies
                .Where(group => !defenders.Contains(group) && group.Side != attacker.Side)
                .Select(group => (defender: group, power: attacker.PowerAgainst(group)))
                .Where(v => v.power > 0)
                .MaxBy(v => v.power)
                .Select(v => v.defender)
                .OrderByDescending(d => d.Power)
                .ThenByDescending(d => d.Initiative)
                .FirstOrDefault();

            if (chosen != null)
            {
                attackers.Add(attacker, chosen);
                defenders.Add(chosen);
            }
        }
        
        if (attackers.Count == 0)
            break;

        var attackOrder = bothArmies
            .OrderByDescending(g => g.Initiative)
            .ToList();
        foreach (var attacker in attackOrder.Where(a => a.Units > 0))
        {
            if (attackers.TryGetValue(attacker, out var defender))
            {
                var poweragainst = attacker.PowerAgainst(defender);
                var damaged = poweragainst / defender.HP;
                
                defender.Units -= Math.Min(attacker.PowerAgainst(defender) / defender.HP, defender.Units);
                if (defender.Units == 0)
                {
                    if (defender.Side == Side.ImmuneSystem)
                        immuneSystem.Remove(defender);
                    else
                        infection.Remove(defender);
                }
            }
        }
    }
    
    return (
        immune: immuneSystem.Sum(g => g.Units),
        infection: infection.Sum(g => g.Units));
}

// wrong: 1569, 1570
int FindMinimalBoost(string path)
{
    var boost = 0;
    
    for (;;)
    {
        var result = Sim(path, boost);
        if (result.infection > 0)
            boost += 100;
        else
            break;
    }
    
    var lastUnits = 0;
    for (;;)
    {
        var result = Sim(path, boost);
        if (result.infection > 0)
            return lastUnits;
            
        lastUnits = result.immune;
        --boost;
    }
}
