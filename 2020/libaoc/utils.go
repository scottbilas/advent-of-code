package libaoc

import (
	"fmt"
	"regexp"
	"strconv"
	"strings"
)

func parseInt(text string) (int, error) {
	return strconv.Atoi(strings.TrimSpace(text))
}

func ParseInt(text string) int {
	i, err := parseInt(text)
	if err != nil {
		panic(err)
	}
	return i
}

func ParseIntSafe(text string, defValue int) int {
	i, err := parseInt(text)
	if err != nil {
		i = defValue
	}
	return i
}

var parseIntsRx = regexp.MustCompile(`[-+]?\d+`)

func ParseInts(text string) []int {
	matches := parseIntsRx.FindAllString(text, -1)
	result := make([]int, len(matches))

	for i := 0; i < len(matches); i++ {
		result[i] = ParseInt(matches[i])
	}

	return result
}

func ParseBlocks(text string) []string {
	return strings.Split(strings.TrimSpace(text), "\n\n")
}

func ParseBlocks2(text string) (a, b string) {
	blocks := strings.Split(strings.TrimSpace(text), "\n\n")
	if len(blocks) != 2 {
		panic(fmt.Sprintf("Expected 2 blocks, got %d", len(blocks)))
	}
	return blocks[0], blocks[1]
}

func ParseLines(text string) []string {
	return strings.Split(strings.TrimSpace(text), "\n")
}

func SplitTrim(text string, sep string) []string {
	parts := strings.Split(text, sep)
	for i := 0; i < len(parts); i++ {
		parts[i] = strings.TrimSpace(parts[i])
	}
	return parts
}

func SplitTrim2(text string, sep string) (string, string) {
	parts := strings.Split(text, sep)
	if len(parts) != 2 {
		panic(fmt.Sprintf("Expected 2 splits, got %d", len(parts)))
	}
	return strings.TrimSpace(parts[0]), strings.TrimSpace(parts[1])
}

func ToInt(value bool) int {
	if value {
		return 1
	} else {
		return 0
	}
}

func AbsInt(value int) int {
	if value < 0 {
		return -value
	}
	return value
}

func MinInt(a, b int) int {
	if b < a {
		return b
	}
	return a
}

func MinimizeInt(dst *int, other int) {
	if other < *dst {
		*dst = other
	}
}

func MaxInt(a, b int) int {
	if a > b {
		return a
	}
	return b
}

func MaximizeInt(dst *int, other int) {
	if other > *dst {
		*dst = other
	}
}

func UpdateBoundsInt(min *int, value int, max *int) {
	MinimizeInt(min, value)
	MaximizeInt(max, value)
}

func Manhattan(x0, y0, x1, y1 int) int {
	return AbsInt(x0-x1) + AbsInt(y0-y1)
}

func ManhattanZero(x, y int) int {
	return AbsInt(x) + AbsInt(y)
}

func DivMod(num, denom int) (quo, rem int) {
	return num / denom, num % denom
}

func DivCeil(num, denom int) int {
	quo, rem := DivMod(num, denom)
	return quo + rem
}
