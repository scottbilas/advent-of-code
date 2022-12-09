import u = require('./utils')

const day = 7

const sample = `
    $ cd /
    $ ls
    dir a
    14848514 b.txt
    8504156 c.dat
    dir d
    $ cd a
    $ ls
    dir e
    29116 f
    2557 g
    62596 h.lst
    $ cd e
    $ ls
    584 i
    $ cd ..
    $ cd ..
    $ cd d
    $ ls
    4060174 j
    8033020 d.log
    5626152 d.ext
    7214296 k`

const input = u.getProblemInput(day)

class Dir {
    name: string
    size: number = 0
    dirs: Dir[] = []

    public constructor(name: string) { this.name = name }
}

function dirSizes(text) {

    let cwd = [new Dir('/')]
    for (let m of text.matchAll(/cd (?<cd>\S+)|(?<size>\d+)/g)) {
        if (m.groups.cd == '/')
            cwd = cwd.slice(-1)
        else if (m.groups.cd == '..')
            cwd.shift()
        else if (m.groups.cd)
            cwd.unshift(cwd[0].dirs.findOrPush(
                d => d.name == m.groups.cd,
                () => new Dir(m.groups.cd)))
        else
            cwd[0].size += +m.groups.size
    }

    let sizes = []
    function calc(dir: Dir): number {
        let total = dir.dirs.map(calc).sum() + dir.size
        sizes.push(total)
        return total
    }

    calc(cwd.last())
    return sizes
}

const solve1 = text => dirSizes(text)
    .filter(x => x <= 100000)
    .sum()

function solve2(text) {
    let sizes = dirSizes(text).sort((a, b) => a - b)
    const needed = 30000000 - (70000000 - sizes.last())
    return sizes.find(x => x >= needed)
}

u.test(`Day ${day}.1 Sample`,  () => solve1(sample),    95437)
u.test(`Day ${day}.1 Problem`, () => solve1(input),   1454188)
u.test(`Day ${day}.2 Sample`,  () => solve2(sample), 24933642)
u.test(`Day ${day}.2 Problem`, () => solve2(input),   4183246)
