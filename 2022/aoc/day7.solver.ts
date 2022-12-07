import _ = require('lodash')
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
    for (let m of text.matchAll(/cd (?<cd>\S+)|dir (?<dir>\S+)|(?<size>\d+)/g)) {
        if (m.groups.cd == '/') {
            cwd = cwd.slice(-1)
        } else if (m.groups.cd == '..') {
            cwd.shift()
        } else if (m.groups.cd) {
            cwd.unshift(cwd[0].dirs.find(d => d.name == m.groups.cd))
        } else if (m.groups.dir) {
            cwd[0].dirs.push(new Dir(m.groups.dir))
        } else {
            cwd[0].size += +m.groups.size
        }
    }

    let sizes = []
    function calc(dir: Dir): number {
        let total = _.sum(dir.dirs.map(calc)) + dir.size
        sizes.push(total)
        return total
    }

    calc(cwd.at(-1))
    return sizes
}

const solve1 = text => _
    .sum(dirSizes(text)
    .filter(x => x <= 100000))

function solve2(text) {
    let sizes = dirSizes(text).sort((a, b) => a - b)
    const needed = 30000000 - (70000000 - sizes.at(-1))
    return sizes.find(x => x >= needed)
}

test(`Day ${day}.1 Sample`, () => { expect(solve1(sample)).toBe(95437); });
test(`Day ${day}.1 Problem`, () => { expect(solve1(input)).toBe(1454188); });
test(`Day ${day}.2 Sample`, () => { expect(solve2(sample)).toBe(24933642); });
test(`Day ${day}.2 Problem`, () => { expect(solve2(input)).toBe(4183246); });
