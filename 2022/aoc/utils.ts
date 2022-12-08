import { readFileSync } from 'fs'

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

export function arrayOfArrays(count: number) {
    return Array(count).fill(0).map(_ => [])
}
