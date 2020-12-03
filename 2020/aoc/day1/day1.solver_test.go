package main

import (
	"bufio"
	"os"
	"strconv"
	"testing"
)

func ReadInts(path string) []int {
	file, _ := os.Open(path)
	scanner := bufio.NewScanner(file)
	scanner.Split(bufio.ScanWords)

	var result []int
	for scanner.Scan() {
		x, _ := strconv.Atoi(scanner.Text())
		result = append(result, x)
	}
	return result
}

var sample = []int{
	1721,
	979,
	366,
	299,
	675,
	1456}

var input = ReadInts("day1.input.txt")

// PART 1

func solve1(input []int) int {
	for i1 := 0; ; i1++ {
		for i0 := i1 + 1; i0 < len(input); i0++ {
			if input[i1]+input[i0] == 2020 {
				return input[i1] * input[i0]
			}
		}
	}
}

// samples

func TestSample1(t *testing.T) {
	got, expected := solve1(sample), 514579
	if got != expected {
		t.Error("Expected", expected, "got", got)
	}
}

// problem

func TestSolve1(t *testing.T) {
	got, expected := solve1(input), 1003971
	if got != expected {
		t.Error("Expected", expected, "got", got)
	}
}

// PART 2

func solve2(input []int) int {

	// easy optimization opportunities:
	//   * remove i0 and replace with reverse lookup map
	//   * sort input so we can stop i1 when i2+i1 >= 2020

	for i2 := 0; ; i2++ {
		for i1 := i2 + 1; i1 < len(input); i1++ {
			for i0 := i1 + 1; i0 < len(input); i0++ {
				if input[i2]+input[i1]+input[i0] == 2020 {
					return input[i2] * input[i1] * input[i0]
				}
			}
		}
	}
}

// samples

func TestSample2(t *testing.T) {
	got, expected := solve2(sample), 241861950
	if got != expected {
		t.Error("Expected", expected, "got", got)
	}
}

// problem

func TestSolve2(t *testing.T) {
	got, expected := solve2(input), 84035952
	if got != expected {
		t.Error("Expected", expected, "got", got)
	}
}
