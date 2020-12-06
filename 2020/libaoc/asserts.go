package libaoc

import (
	"log"
	"reflect"
	"testing"
)

func AssertEqual(t *testing.T, got interface{}, expected interface{}) {
	t.Helper()
	if reflect.TypeOf(got) != reflect.TypeOf(expected) {
		log.Panicf("Incompatible types '%v' and '%v'", reflect.TypeOf(got), reflect.TypeOf(expected))
	}
	if got != expected {
		t.Errorf("Expected '%v', got '%v'", expected, got)
	}
}

func AssertEqualSub(testName string, t *testing.T, got interface{}, expected interface{}) {
	t.Run(testName, func(t *testing.T) {
		AssertEqual(t, got, expected)
	})
}

func AssertPanic(t *testing.T, f func()) {
	t.Helper()
	defer func() {
		t.Helper()
		if r := recover(); r == nil {
			t.Errorf("Panic was expected")
		}
	}()
	f()
}
