static class Statics
{
    public static IEnumerable<T> Generate<T>(T initialState, Func<T, bool> condition, Func<T, T> iterate) =>
        EnumerableEx.Generate(initialState, condition, iterate, i => i);
}
