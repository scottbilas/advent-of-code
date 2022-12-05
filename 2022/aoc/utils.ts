import { readFileSync } from 'fs'

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

export function arrayOfArrays(count: number) {
    return Array(count).fill(0).map(_ => [])
}
