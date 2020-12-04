package libaoc

import "testing"

func Test_CleanupSample(t *testing.T) {
	t.Run("WithCrlf_ConvertsToLf", func(t *testing.T) {
		AssertEqual(t, NormalizeSample("abc\r\ndef\n"), "abc\ndef\n")
		AssertEqual(t, NormalizeSample("abc\r\n\r\ndef\r\n"), "abc\n\ndef\n")
	})

	t.Run("WithFileSurroundedBySpace_TrimsAndEnsuresTrailingNewline", func(t *testing.T) {
		AssertEqual(t, NormalizeSample(""), "\n")
		AssertEqual(t, NormalizeSample("  "), "\n")
		AssertEqual(t, NormalizeSample("  \t  "), "\n")
		AssertEqual(t, NormalizeSample("  \n"), "\n")
		AssertEqual(t, NormalizeSample("\n  \n"), "\n")
		AssertEqual(t, NormalizeSample("  \n \t \n  "), "\n")
		AssertEqual(t, NormalizeSample("  abc"), "abc\n")
		AssertEqual(t, NormalizeSample("\n \t abc\t"), "abc\n")
		AssertEqual(t, NormalizeSample("abc  "), "abc\n")
		AssertEqual(t, NormalizeSample("\nabc  "), "abc\n")
		AssertEqual(t, NormalizeSample("  abc \t "), "abc\n")
		AssertEqual(t, NormalizeSample("  \n  abc  "), "abc\n")
		AssertEqual(t, NormalizeSample("  abc\n   "), "abc\n")
		AssertEqual(t, NormalizeSample("  abc\n \t  \n   "), "abc\n")
		AssertEqual(t, NormalizeSample("\t  \nabc\ndef \n  \t "), "abc\ndef\n")
	})

	t.Run("WithLinesSurroundedBySpace_TrimsLines", func(t *testing.T) {
		AssertEqual(t, NormalizeSample("abc\n   def\n"), "abc\ndef\n")
		AssertEqual(t, NormalizeSample("abc\n  \t \n   def\n"), "abc\n\ndef\n")
		AssertEqual(t, NormalizeSample("abc\n  de\tf\n"), "abc\nde\tf\n")
	})
}

func Test_Atoi(t *testing.T) {
	t.Run("WithInvalidData_Panics", func(t *testing.T) {
		AssertPanic(t, func() { ParseInt("abc") })
	})
}
