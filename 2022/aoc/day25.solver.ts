import { getProblemInput, check, parseLines } from './utils'

const day = 25

const table1: [number, string][] = [
    [         1,             '1' ],
    [         2,             '2' ],
    [         3,            '1=' ],
    [         4,            '1-' ],
    [         5,            '10' ],
    [         6,            '11' ],
    [         7,            '12' ],
    [         8,            '2=' ],
    [         9,            '2-' ],
    [        10,            '20' ],
    [        15,           '1=0' ],
    [        20,           '1-0' ],
    [      2022,        '1=11-2' ],
    [     12345,       '1-0---0' ],
    [ 314159265, '1121-1110-1=0' ]]
                  1120411044030

const table2: [string, number][] = [
    [ '1=-0-2', 1747],
    [  '12111',  906],
    [   '2=0=',  198],
    [     '21',   11],
    [   '2=01',  201],
    [    '111',   31],
    [  '20012', 1257],
    [    '112',   32],
    [  '1=-1=',  353],
    [   '1-12',  107],
    [     '12',    7],
    [     '1=',    3],
    [    '122',   37]]

const sample = table2.map(([s, _]) => s).join('\n')

const input = getProblemInput(day)

function toSnafu(num: number): string {
    let snafu = ''
    while (num > 0) {
        let digit = num % 5
        num = Math.floor(num / 5)

        if (digit <= 2)
            snafu = digit + snafu
        else {
            ++num
            snafu = '=-'[digit-3] + snafu
        }
    }
    return snafu
}

const fromSnafu = (text: string): number => text.split('')
    .reduce((acc, ch) => acc*5 + ({'-': -1, '=': -2}[ch] ?? Number(ch)), 0)

function testTable(table: [number, string][]): boolean {
    let ok = true
    for (const [num, snafu] of table) {
        ok &&= fromSnafu(snafu) === num
        ok &&= fromSnafu(toSnafu(num)) === num
        ok &&= toSnafu(num) === snafu
        ok &&= toSnafu(fromSnafu(snafu)) === snafu
    }
    return ok
}

const testTable1 = () => testTable(table1)
const testTable2 = () => testTable(table2.map(([s, n]) => [n, s]))

const solve = text => toSnafu(parseLines(text).map(fromSnafu).sum())

check(`Day ${day} Table 1`, () => testTable1(),  true)
check(`Day ${day} Table 2`, () => testTable2(),  true)
check(`Day ${day} Sample`,  () => solve(sample), '2=-1=0')
check(`Day ${day} Problem`, () => solve(input),  '2-21=02=1-121-2-11-0')
