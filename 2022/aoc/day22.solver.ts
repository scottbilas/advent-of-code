import { getProblemInput, check, parseBlocks, parseStringGrid, number2, number3, Grid } from './utils'
import * as math from 'mathjs'

const day = 22
let draw = false

const sample = `
            ...#
            .#..
            #...
            ....
    ...#.......#
    ........#...
    ..#....#....
    ..........#.
            ...#....
            .....#..
            .#......
            ......#.

    10R5L5R10L4R5L5`

const input = getProblemInput(day)

enum Dir { E, S, W, N }

const DirData = [
    { R: Dir.S, L: Dir.N, F: Dir.W, Off: new number2( 1,  0), Mark: '>' },
    { R: Dir.W, L: Dir.E, F: Dir.N, Off: new number2( 0,  1), Mark: 'v' },
    { R: Dir.N, L: Dir.S, F: Dir.E, Off: new number2(-1,  0), Mark: '<' },
    { R: Dir.E, L: Dir.W, F: Dir.S, Off: new number2( 0, -1), Mark: '^' }]

class Board {
    grid: Grid<string>
    instrs: string[]
    start: number2

    constructor(text: string) {
        let [map, instrs] = parseBlocks(text)
        this.grid = parseStringGrid(map)
        this.instrs = instrs.match(/[LR]|\d+/g)
        this.start = this.grid.posAt(this.grid.cells.findIndex(c => c != ' '))
    }

    move(pos: number2, dir: Dir) {
        if (!draw)
            return

        this.grid.set(pos, DirData[dir].Mark)
        console.log(this.grid.toString())
    }
}

const score = (pos: number2, dir: Dir) => 1000*(pos.y+1) + 4*(pos.x+1) + dir

function solve1(text) {
    let board = new Board(text)
    let [pos, dir] = [board.start, Dir.E]

    for (let instr of board.instrs) {
        if (instr == 'R' || instr == 'L') {
            dir = DirData[dir][instr]
            continue
        }

        for (let steps = +instr; steps > 0; --steps) {
            board.move(pos, dir)

            // try to move there
            let testPos = pos.add(DirData[dir].Off)

            // need to wrap?
            if (!board.grid.inBounds(testPos) || board.grid.get(testPos) == ' ') {

                // rewind to find wrap point
                testPos = pos
                for (;;) {
                    let last = testPos.sub(DirData[dir].Off)
                    if (!board.grid.inBounds(last) || board.grid.get(last) == ' ')
                        break
                    testPos = last
                }
            }

            // stop at wall
            if (board.grid.get(testPos) == '#')
                break

            pos = testPos
        }
    }

    return score(pos, dir)
}

class Axes {
    constructor(
        public O: number3,    // out
        public R: number3,    // right
        public D: number3) {} // down
}

class Face {
    origin: number2 // origin (top-left) in map space
    axes:   Axes
    verts:  [number3, number3, number3, number3] // coords in folded unit space (0=tl, 1=tr, 2=br, 3=bl)

    constructor(origin: number2, origin3: number3, axes: Axes) {
        this.origin = origin
        this.axes = axes

        this.verts = [
            origin3,
            origin3.add(axes.R),
            origin3.add(axes.R).add(axes.D),
            origin3.add(axes.D)]
    }

    findVert = (vert: number3) => this.verts.findIndex(v => v.equals(vert))
}

function solve2(text) {
    let board = new Board(text)
    let tilesz = math.gcd(board.grid.cx, board.grid.cy)
    let faces = new Array<Face>()

    function walk(origin: number2, origin3: number3, axes: Axes) {
        if (!board.grid.inBounds(origin))
            return
        if (board.grid.get(origin) == ' ')
            return
        if (faces.some(f => f.origin.equals(origin)))
            return

        let from = new Face(origin, origin3, axes)
        faces.push(from)

        let [v, a] = [from.verts, from.axes]
        for (let item of [
            [[ 1,  0], v[1],          [a.R,       a.O.neg(), a.D      ]],
            [[ 0,  1], v[3],          [a.D,       a.R,       a.O.neg()]],
            [[-1,  0], v[0].sub(a.O), [a.R.neg(), a.O,       a.D      ]],
            [[ 0, -1], v[0].sub(a.O), [a.D.neg(), a.R,       a.O      ]]]) {
            let [off, origin3, iaxes] = item
            let origin = new number2(off[0], off[1]).mulS(tilesz)
            walk(from.origin.add(origin), <number3>origin3, new Axes(iaxes[0], iaxes[1], iaxes[2]))
        }
    }

    // map tiles onto cube faces
    walk(board.start, number3.zero,
        new Axes(new number3(0, 0, -1), new number3(1, 0, 0), new number3(0, 1, 0)))

    // run sim
    let [face, pos, dir] = [faces[0], number2.zero, Dir.E]
    for (let instr of board.instrs) {
        if (instr == 'R' || instr == 'L') {
            dir = DirData[dir][instr]
            continue
        }

        for (let steps = +instr; steps > 0; --steps) {
            board.move(pos.add(face.origin), dir)

            // attempt move
            let [oface, opos, odir] = [face, pos, dir]
            pos = pos.add(DirData[dir].Off)

            // out of tile bounds?
            if (!pos.ge(number2.zero).all || !pos.lt(number2.fromScalar(tilesz)).all) {

                // find shared edge
                let from = (dir+1)%4
                let to
                for (face of faces.filter(f => f != oface)) {
                    to = face.findVert(oface.verts[from])
                    let other = face.findVert(oface.verts[(from+1)%4])
                    if (to >= 0 && other >= 0)
                        break
                }

                // 4 rotations^2 ways that tiles can connect (perf: lol)
                let adjust = [
                    [[0,    opos.x], [-1-opos.x, 0], [-1, -1-opos.x],                ],
                    [              , [-1-opos.y, 0], [-1, -1-opos.y], [   opos.y, -1]],
                    [[0, -1-opos.x],               , [-1,    opos.x], [-1-opos.x, -1]],
                    [[0, -1-opos.y], [   opos.y, 0],                , [-1-opos.y, -1]]
                    ][from][to]
                if (adjust)
                    pos.setXY(adjust[0], adjust[1])

                // wrap
                pos = pos.addS(tilesz).modS(tilesz)

                // new dir depends on rotation "distance" from current face
                let dist = to-from
                if (dist < 3)
                    dir = (dir+dist+1)%4
            }

            // stop at wall
            if (board.grid.get(pos.add(face.origin)) == '#') {
                [face, pos, dir] = [oface, opos, odir]
                break
            }
        }
    }

    return score(pos.add(face.origin), dir)
}

check(`Day ${day}.1 Sample`,  () => solve1(sample),   6032)
check(`Day ${day}.1 Problem`, () => solve1(input),   77318)
check(`Day ${day}.2 Sample`,  () => solve2(sample),   5031)
check(`Day ${day}.2 Problem`, () => solve2(input),  126017)
