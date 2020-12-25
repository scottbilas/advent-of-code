package libaoc

import (
	"fmt"
	"io/ioutil"
	"regexp"
	"runtime"
	"strings"
)

// remove any artifacts of copy pasting problem sample from the web page directly into a multiline string in code
func NormalizeSample(text string) string {
	text = strings.ReplaceAll(text, "\r", "")
	text = strings.TrimSpace(text)

	lines := strings.Split(text, "\n")
	for i := 0; i < len(lines); i++ {
		lines[i] = strings.TrimSpace(lines[i])
	}
	text = strings.Join(lines, "\n")

	text += "\n"
	return text
}

func ReadDayFile(name string) string {
	filename := fmt.Sprintf(GetDay() + "." + name + ".txt")
	file, err := ioutil.ReadFile(filename)
	if err != nil {
		panic(err)
	}
	return string(file)
}

func ReadSampleFile() string {
	return ReadDayFile("sample")
}

func ReadInputFile() string {
	return ReadDayFile("input")
}

func GetDay() string {
	stack := make([]uintptr, 10)
	count := runtime.Callers(2, stack)
	rx := regexp.MustCompile(`\bday\d+`)

	for _, pc := range stack[:count] {
		file, _ := runtime.FuncForPC(pc).FileLine(pc)
		match := rx.FindString(file)
		if match != "" {
			return match
		}
	}

	panic("Can't detect day")
}
