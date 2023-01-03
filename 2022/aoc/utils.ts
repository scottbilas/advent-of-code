import { readFileSync } from 'fs'

// test-related

function prettyMs(ms: number) {
    // TODO: if ms > 1000, show seconds
    return `${ms}ms`
}

export function check(name, solver, expected) {
    const start = new Date().getTime()
    const result = solver()
    const end = new Date().getTime()

    if (result === expected) {
        console.log(`√ ${name} (${prettyMs(end - start)})`)
    }
    else {
        console.log(`× ${name} (${prettyMs(end - start)})`)
        console.log(`    Result: ${result}`)
        console.log(`  Expected: ${expected}`)
    }
}

// parsing

export function getProblemInput(day: number) {
    return readFileSync(`aoc/day${day}.input.txt`, 'utf-8')
}

export function getProblemSample(day: number) {
    return readFileSync(`aoc/day${day}.sample.txt`, 'utf-8')
}

export function parseNumbers(text: string, positiveOnly: boolean = false): number[] {
    return text.match(positiveOnly ? /\d+/g : /-?\d+/g).map(Number)
}

export function parseWords(text: string): string[] {
    return text.match(/\S+/g)
}

export function parseBlocks(text: string): string[] {
    return parseLines(text).join('\n').split(/\n\n/)
}

export function parseLines(text: string): string[] {
    // fix crlf and get lines
    let lines = text.replace(/\r/g, '').split(/\n/)

    // remove leading and trailing blank lines
    let start = 0, end = lines.length
    while (start < end && lines[start].trim() == '') ++start
    while (end >= start && lines[end-1].trim() == '') --end
    lines = lines.slice(start, end)

    // calculate overall indent
    let indent = lines.filter(l => l.length).map(l => l.match(/^ */)[0].length).min()

    // remove indent
    return lines.map(l => l.slice(indent))
}

export class boolean2 {
    constructor(public x: boolean, public y: boolean) {}

    get all() { return this.x && this.y }
    get any() { return this.x || this.y }
}

export class boolean3 {
    constructor(public x: boolean, public y: boolean, public z: boolean) {}

    get all() { return this.x && this.y && this.z }
    get any() { return this.x || this.y || this.z }
}

export class number2 {
    constructor(public x: number, public y: number) {}

    static fromScalar = (scalar: number): number2 => new number2(scalar, scalar)
    static fromTuple = (tuple: [number, number]): number2 => new number2(tuple[0], tuple[1])
    static fromArray = (array: number[]): number2 => new number2(array[0], array[1])
    static fromOffset = (off: number, cx: number): number2 => new number2(off % cx, Math.floor(off / cx))

    equals = (other: number2): boolean => this.x == other.x && this.y == other.y
    lt = (other: number2): boolean2 => new boolean2(this.x < other.x, this.y < other.y)
    le = (other: number2): boolean2 => new boolean2(this.x <= other.x, this.y <= other.y)
    gt = (other: number2): boolean2 => new boolean2(this.x > other.x, this.y > other.y)
    ge = (other: number2): boolean2 => new boolean2(this.x >= other.x, this.y >= other.y)

    setXY = (x: number, y: number) => [this.x, this.y] = [x, y]
    setS = (other: number) => [this.x, this.y] = [other, other]
    set2 = (other: [number, number]) => [this.x, this.y] = [other[0], other[1]]
    set2a = (other: number[]) => [this.x, this.y] = [other[0], other[1]]

    add = (other: number2): number2 => new number2(this.x + other.x, this.y + other.y)
    addS = (other: number): number2 => new number2(this.x + other, this.y + other)
    add2 = (other: [number, number]): number2 => new number2(this.x + other[0], this.y + other[1])
    add2a = (other: number[]): number2 => new number2(this.x + other[0], this.y + other[1])
    addX = (x: number): number2 => new number2(this.x + x, this.y)
    addY = (y: number): number2 => new number2(this.x, this.y + y)
    sub = (other: number2): number2 => new number2(this.x - other.x, this.y - other.y)
    subS = (other: number): number2 => new number2(this.x - other, this.y - other)
    mul = (other: number2): number2 => new number2(this.x * other.x, this.y * other.y)
    mulS = (other: number): number2 => new number2(this.x * other, this.y * other)
    div = (other: number2): number2 => new number2(this.x / other.x, this.y / other.y)
    divS = (other: number): number2 => new number2(this.x / other, this.y / other)
    mod = (other: number2): number2 => new number2(this.x % other.x, this.y % other.y)
    modS = (other: number): number2 => new number2(this.x % other, this.y % other)

    neg = (): number2 => new number2(-this.x, -this.y)
    max = (other: number2): number2 => new number2(Math.max(this.x, other.x), Math.max(this.y, other.y))
    min = (other: number2): number2 => new number2(Math.min(this.x, other.x), Math.min(this.y, other.y))

    toOffset = (cx: number): number => this.y*cx + this.x

    toString = (): string => number2.toString(this.x, this.y)
    static toString = (x: number, y: number): string => x+','+y

    static one = number2.fromScalar(1)
    static zero = number2.fromScalar(0)

    static maxNumber = number2.fromScalar(Number.MAX_SAFE_INTEGER)
    static minNumber = number2.fromScalar(Number.MIN_SAFE_INTEGER)
}

export function parseNumber2s(text: string): number2[] {
    return parseNumbers(text).chunk(2).map(([x, y]) => new number2(x, y))
}

export class number3 {
    constructor(public x: number, public y: number, public z: number) {}

    static fromScalar = (scalar: number): number3 => new number3(scalar, scalar, scalar)
    static fromTuple = (tuple: [number, number, number]): number3 => new number3(tuple[0], tuple[1], tuple[2])
    static fromArray = (array: number[]): number3 => new number3(array[0], array[1], array[2])

    equals = (other: number3): boolean => this.x == other.x && this.y == other.y && this.z == other.z
    lt = (other: number3): boolean3 => new boolean3(this.x < other.x, this.y < other.y, this.z < other.z)
    le = (other: number3): boolean3 => new boolean3(this.x <= other.x, this.y <= other.y, this.z <= other.z)
    gt = (other: number3): boolean3 => new boolean3(this.x > other.x, this.y > other.y, this.z > other.z)
    ge = (other: number3): boolean3 => new boolean3(this.x >= other.x, this.y >= other.y, this.z >= other.z)

    setXY = (x: number, y: number, z: number) => [this.x, this.y, this.z] = [x, y, z]
    setS = (other: number) => [this.x, this.y, this.z] = [other, other, other]
    set3 = (other: [number, number, number]) => [this.x, this.y, this.z] = [other[0], other[1], other[2]]
    set3a = (other: number[]) => [this.x, this.y, this.z] = [other[0], other[1], other[2]]

    add = (other: number3): number3 => new number3(this.x + other.x, this.y + other.y, this.z + other.z)
    addS = (other: number): number3 => new number3(this.x + other, this.y + other, this.z + other)
    add3 = (other: [number, number, number]): number3 => new number3(this.x + other[0], this.y + other[1], this.z + other[2])
    add3a = (other: number[]): number3 => new number3(this.x + other[0], this.y + other[1], this.z + other[2])
    addX = (x: number): number3 => new number3(this.x + x, this.y, this.z)
    addY = (y: number): number3 => new number3(this.x, this.y + y, this.z)
    addZ = (z: number): number3 => new number3(this.x, this.y, this.z + z)
    sub = (other: number3): number3 => new number3(this.x - other.x, this.y - other.y, this.z - other.z)
    subS = (other: number): number3 => new number3(this.x - other, this.y - other, this.z - other)
    mul = (other: number3): number3 => new number3(this.x * other.x, this.y * other.y, this.z * other.z)
    mulS = (other: number): number3 => new number3(this.x * other, this.y * other, this.z * other)
    div = (other: number3): number3 => new number3(this.x / other.x, this.y / other.y, this.z / other.z)
    divS = (other: number): number3 => new number3(this.x / other, this.y / other, this.z / other)
    mod = (other: number3): number3 => new number3(this.x % other.x, this.y % other.y, this.z % other.z)
    modS = (other: number): number3 => new number3(this.x % other, this.y % other, this.z % other)

    neg = (): number3 => new number3(-this.x, -this.y, -this.z)
    max = (other: number3): number3 => new number3(Math.max(this.x, other.x), Math.max(this.y, other.y), Math.max(this.z, other.z))
    min = (other: number3): number3 => new number3(Math.min(this.x, other.x), Math.min(this.y, other.y), Math.min(this.z, other.z))

    toString = (): string => number3.toString(this.x, this.y, this.z)
    static toString = (x: number, y: number, z: number): string => x+','+y+','+z

    static one = number3.fromScalar(1)
    static zero = number3.fromScalar(0)

    static maxNumber = number3.fromScalar(Number.MAX_SAFE_INTEGER)
    static minNumber = number3.fromScalar(Number.MIN_SAFE_INTEGER)
}

export function parseNumber3s(text: string): number3[] {
    return parseNumbers(text).chunk(3).map(([x, y, z]) => new number3(x, y, z))
}

export class Grid<T> {
    constructor(public cells: T[], public cx: number, public cy: number) {}

    get size(): number2 {
        return new number2(this.cx, this.cy)
    }

    get(pos: number2): T {
        return this.cells[pos.toOffset(this.cx)]
    }

    get2(x: number, y: number): T {
        return this.cells[y*this.cx + x]
    }

    posAt(offset: number): number2 {
        return number2.fromOffset(offset, this.cx)
    }

    try(pos: number2): T | undefined {
        if (pos.x < 0 || pos.x >= this.cx || pos.y < 0 || pos.y >= this.cy)
            return undefined
        return this.get(pos)
    }

    inBounds(pos: number2): boolean {
        return pos.x >= 0 && pos.x < this.cx && pos.y >= 0 && pos.y < this.cy
    }

    set(pos: number2, value: T) {
        this.cells[pos.toOffset(this.cx)] = value
    }

    forEach(callback: (cell: T, pos: number2) => void) {
        this.cells.forEach((v, i) => callback(v, number2.fromOffset(i, this.cx)))
    }

    toString(stringize: (cell: T) => string = (v) => v.toString()): string {
        let out = ''
        for (let y = 0; y < this.cy; y++) { // TODO: this can be done way better with map and modulo and stuff
            for (let x = 0; x < this.cx; x++) {
                out += stringize(this.get(new number2(x, y)))
            }
            out += '\n'
        }
        return out
    }
}

export function parseGrid<T>(text: string, parse: (string) => T): Grid<T> {
    let lines = parseLines(text)
    let cx = lines.map(l => l.length).max()
    return new Grid<T>(lines
        .map(l => l.padEnd(cx, ' '))
        .join('')
        .split('')
        .map(parse), cx, lines.length)
}

export function parseStringGrid(text: string, parse: (string) => string = (s) => s): Grid<string> {
    return parseGrid<string>(text, parse)
}

export function parseNumGrid(text: string): Grid<number> {
    return parseGrid(text, s => +s)
}

// utils

export function minmax(value: number, min: number, max: number) {
    return Math.max(min, Math.min(max, value))
}

export function arrayOfArrays(count: number, predicate = () => []) {
    return Array(count).fill(0).map(predicate)
}

// extensions

declare global {
    interface Array<T> {
        sum(): number
        product(): number
        min(): number
        max(): number
        take(count: number): T[]
        first(): T
        last(): T

        chunk(count: number): T[][]
        findOrPush(predicate: (item: T) => boolean, factory: () => T): T
    }

    interface String {
        charOffset(baseChar: string): number
    }
}

Array.prototype.sum = function(): number { return this.reduce((a, b) => a + b, 0) }
Array.prototype.product = function(): number { return this.reduce((a, b) => a * b, 1) }
Array.prototype.min = function(): number { return Math.min(...this) }
Array.prototype.max = function(): number { return Math.max(...this) }
Array.prototype.take = function(count: number): any[] { return this.slice(0, count) }
Array.prototype.first = function(): any { return this.at(0) }
Array.prototype.last = function(): any { return this.at(-1) }

Array.prototype.chunk = function(count: number): any[] {
    let result = []
    for (let i = 0; i < this.length; i += count)
        result.push(this.slice(i, i + count))
    return result
}

Array.prototype.findOrPush = function(predicate, factory): any {
    let item = this.find(predicate)
    if (!item) {
        item = factory()
        this.push(item)
    }
    return item
}

String.prototype.charOffset = function(baseChar: string): number {
    return this.charCodeAt(0) - baseChar.charCodeAt(0)
}
