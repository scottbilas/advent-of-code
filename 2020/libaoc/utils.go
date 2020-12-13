package libaoc

import (
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

func ParseLines(text string) []string {
	return strings.Split(strings.TrimSpace(text), "\n")
}

func ToInt(b bool) int {
	if b {
		return 1
	} else {
		return 0
	}
}

func AbsInt(i int) int {
	if i < 0 {
		return -i
	}
	return i
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
