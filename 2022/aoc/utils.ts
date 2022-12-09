import { readFileSync } from 'fs'

// test-related

function prettyMs(ms: number) {
    // TODO: if ms > 1000, show seconds
    return `${ms}ms`
}

export function test(name, solver, expected) {
    const start = new Date().getTime();
    const result = solver()
    const end = new Date().getTime();

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

export function parseNums(text: string): number[] {
    return text.match(/\d+/g).map(Number)
}

export function parseWords(text: string): string[] {
    return text.match(/\S+/g)
}

export function parseBlocks(text: string): string[] {
    return text.replace(/\r/g, '').split(/\n\n/)
}

export function parseGrid(text: string): [string, number, number] {
    let lines = text.trim().split('\n').map(l => l.trim())
    return [lines.join(''), lines[0].length, lines.length]
}

export function parseNumGrid(text: string): [number[], number, number] {
    let lines = text.trim().split('\n').map(l => l.trim())
    return [lines.join('').split('').map(Number), lines[0].length, lines.length]
}

// utils

export function minmax(value: number, min: number, max: number) {
    return Math.max(min, Math.min(max, value))
}

export function arrayOfArrays(count: number) {
    return Array(count).fill(0).map(_ => [])
}

// extensions

declare global {
    interface Array<T> {
        sum(): number
        product(): number
        take(count: number): T[]
        first(): T
        last(): T
        any(): boolean

        chunk(count: number): T[][]
        findOrPush(predicate: (item: T) => boolean, factory: () => T): T
    }
}

Array.prototype.sum = function(): number { return this.reduce((a, b) => a + b, 0) }
Array.prototype.product = function(): number { return this.reduce((a, b) => a * b, 1) }
Array.prototype.take = function(count: number): [] { return this.slice(0, count) }
Array.prototype.first = function(): any { return this.at(0) }
Array.prototype.last = function(): any { return this.at(-1) }
Array.prototype.any = function(): boolean { return this.sum() > 0 }

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
