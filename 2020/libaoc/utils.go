package libaoc

import (
	"strconv"
	"strings"
)

func ParseInt(text string) int {
	i, err := strconv.Atoi(text)
	if err != nil {
		panic(err)
	}
	return i
}

func ParseIntSafe(text string, defValue int) int {
	i, err := strconv.Atoi(text)
	if err != nil {
		i = defValue
	}
	return i
}

func ParseInts(text string) []int {
	matches := strings.Fields(text)
	result := make([]int, len(matches))

	for i := 0; i < len(matches); i++ {
		result[i] = ParseInt(matches[i])
	}

	return result
}

func ParseBlocks(text string) []string {
	blocks := strings.Split(text, "\n\n")
	for i := 0; i < len(blocks); i++ {
		blocks[i] = strings.TrimSpace(blocks[i])
	}
	return blocks
}

func ToInt(b bool) int {
	if b {
		return 1
	} else {
		return 0
	}
}

func DivMod(num, denom int) (quo, rem int) {
	return num / denom, num % denom
}

func DivCeil(num, denom int) int {
	quo, rem := DivMod(num, denom)
	return quo + rem
}
