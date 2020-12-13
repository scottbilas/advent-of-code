package libaoc

import (
	"github.com/go-test/deep"
	"log"
	"reflect"
	"testing"
)

func AssertEqual(t *testing.T, got interface{}, expected interface{}) {
	t.Helper()
	if reflect.TypeOf(got) != reflect.TypeOf(expected) {
		log.Panicf("Incompatible types '%v' and '%v'", reflect.TypeOf(got), reflect.TypeOf(expected))
	}
	if diff := deep.Equal(got, expected); diff != nil {
		t.Error(diff)
	}
}

func AssertEqualSub(testName string, t *testing.T, got interface{}, expected interface{}) {
	t.Helper()
	t.Run(testName, func(t *testing.T) {
		t.Helper()
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
