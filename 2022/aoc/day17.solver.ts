import _ = require('lodash')
import { number2, Grid, parseBlocks, parseStringGrid, check, getProblemInput } from './utils'

const day = 17

class Shape {
    offsets: number2[]
    cx: number
    cy: number

    constructor(grid: Grid<string>) {
        this.offsets = grid.cells
            .map((c, i) => [c, number2.fromOffset(i, grid.cx)])
            .filter(v => v[0] == '#')
            .map(v => <number2>v[1])
            .map(p => new number2(p.x, grid.cy - p.y - 1))
        this.cx = grid.cx
        this.cy = grid.cy
    }
}

const shapes = parseBlocks(`
    ####

    .#.
    ###
    .#.

    ..#
    ..#
    ###

    #
    #
    #
    #

    ##
    ##
    `, true).map(b => new Shape(parseStringGrid(b)))

const sample = '>>><<><>><<<>><>>><<<>>><<<><<<>><>><<>>'
const input = getProblemInput(day).trim()

type Cell = '.' | '-' | '#' | '@'

function sim(instructions: string, iters: number): number[] {

    const width = 7
    const startX = 2
    const startRelY = 4

    let board: Cell[] = Array<Cell>(width).fill('-')

    const draw = (shape, pos, set: Cell) =>
        shape.offsets.forEach(off =>
            board[pos.add(off).toOffset(width)] = set)

    function collides(shape, pos): boolean {
        for (let off of shape.offsets) {
            let cell = board[pos.add(off).toOffset(width)]
            if (cell != '.')
                return true
        }
        return false
    }

    function print(shape, pos) {
        draw(shape, pos, '@')
        let out = ''
        for (let i = board.length - width; i >= 0; i -= width)
            out += (i == 0 ? '+' : '|') + board.slice(i, i + width).join('') + (i == 0 ? '+' : '|') + '\n'
        console.log(out)
        draw(shape, pos, '.')
    }

    let ishape = 0
    function nextShape(): Shape {
        let shape = shapes[ishape]
        if (++ishape > shapes.length - 1)
            ishape = 0
        return shape
    }

    let iinstr = 0
    function nextInstruction(): number {
        let instr = instructions[iinstr]
        if (++iinstr > instructions.length - 1)
            iinstr = 0
        return instr == '<' ? -1 : 1
    }

    let height = 1
    let top = 0
    let tops = [top]

    for (let i = 0; i < iters; ++i) {
        let shape = nextShape()
        let pos = new number2(startX, top + startRelY)

        for (; height < pos.y + shape.cy; ++height)
            board.push(...Array(width).fill('.'))

        //print(shape, pos)

        for (;;) {
            let test = pos.addX(nextInstruction())
            if (test.x >= 0 && test.x + shape.cx <= width && !collides(shape, test)) {
                pos = test
                //print(shape, pos)
            }

            test = pos.addY(-1)
            if (collides(shape, test)) {
                draw(shape, pos, '#')
                top = Math.max(top, pos.y + shape.cy - 1)
                tops.push(top)
                break
            }

            pos = test
            //print(shape, pos)
        }
    }

    return tops
}

function solve1(instructions: string) {
    return sim(instructions, 2022).last()
}

import { writeFileSync } from 'fs'

let which = 1
function solve2(instructions) {
    let tops = sim(instructions, 100000)
    tops[-1] = 0

    //writeFileSync(`tops-${which++}.txt`, tops.map((v, i) => v - tops[i-1]).join(''))
}

/*

int[] parse(string s) => s.Select(c => int.Parse(new string(c, 1))).ToArray();

int part = 2;

var prologue = part == 1
    ? parse("01321213220132021334")
    : parse("121201130012302133021322012322112100213200300121300300013322132101220213300022401230013020123220004213242133401031213040121221304013300133221230112130133001330013322133021322213300123201321113300132001222213242132111320113022130021332213240133201332013340003110321213040132101334002000123011334013300133201303200130133221322213340103201330013210033000330213022121201334013300121101330012130123001334013200132201230213340003200232211212130301330000322132200230013300122001130012122003100303001032130301234012120122400230213202022100301201300133001122013220103221302013300121301221003211121320330013300121221332013340003");
var segment = part == 1
    ? parse("01230113220023401212012120132001334")
    : parse("20133221334200042121301232213002133221322001300133201302012320123011330212300132121324012300022001303013202132001324000340133021332212300130221303013222133401303213042133001321013240133221304000312013200034013320123201213212302130201320213200133221210013200122001303212340132201222012120133001121201222103201232013300132401303002020023021300013240132000324200300022001222013342133201332213300121201322013322133021332203040123201330013320130301322012220130201320013320133221212013220123401221201220123421322002320133221324002002023221302012300132401212003200121011332013302132001224013300132201320013340020001330013322112111332012302123201330013300132001121113240122201301212200133000220200342130300220012301013001321002120132200200013222123000321213322133001334013211132000232012120132421321013300133021230113040133201332012222030020122012320133201322202020033001330200220122221324013342130221230002220123201220213220103201032013020133200334012300112201030213012133021213013320123001212003202121300120013022122200230002202133201212002200033021322012122133401210213220123000324202320133200230013300023021322012122132201324002240132110322013200032401230003320122220330012130132401322211240133021324013340133001324213200123001212002202023220030013320123011324210002023221330210320133201032212320130001220202220123001330013000122021124013300122021230013320132201302013030032221213002220123011334013300133201303200130133221322213340103201330013210033000330213022121201334013300121101330012130123001334013200132201230213340003200232211212130301330000322132200230013300122001130012122003100303001032130301234012120122400230213202022100301201300133001122013220103221302013300121301221003211121320330013300121221332013340003");
var target = 1000000000000L;

var prologueSum = prologue.Sum();
var segmentSum = segment.Sum();

var need = (target - prologue.Length) / segment.Length;
var extra = (target - prologue.Length) % segment.Length;

var result = prologueSum + segmentSum*need + segment[..(int)extra].Sum();

1514285714288.Dump();
result.Dump();

// it's not 1500874635585

*/

check(`Day ${day}.1 Sample`,  () => solve1(sample),           3068)
check(`Day ${day}.1 Problem`, () => solve1(input),            3059)
check(`Day ${day}.2 Sample`,  () => solve2(sample),  1514285714288)
check(`Day ${day}.2 Problem`, () => solve2(input),  15008746355870)
