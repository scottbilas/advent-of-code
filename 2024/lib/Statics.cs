static class Statics
{
    public static void With<T0>(T0 v0, Action<T0> action) => action(v0);
    public static void With<T0, T1>(T0 v0, T1 v1, Action<T0, T1> action) => action(v0, v1);
    public static void With<T0, T1, T2>(T0 v0, T1 v1, T2 v2, Action<T0, T1, T2> action) => action(v0, v1, v2);
    public static void With<T0, T1, T2, T3>(T0 v0, T1 v1, T2 v2, T3 v3, Action<T0, T1, T2, T3> action) => action(v0, v1, v2, v3);
    public static void With<T0, T1, T2, T3, T4>(T0 v0, T1 v1, T2 v2, T3 v3, T4 v4, Action<T0, T1, T2, T3, T4> action) => action(v0, v1, v2, v3, v4);

    public static TR With<T0, TR>(T0 v0, Func<T0, TR> action) => action(v0);
    public static TR With<T0, T1, TR>(T0 v0, T1 v1, Func<T0, T1, TR> action) => action(v0, v1);
    public static TR With<T0, T1, T2, TR>(T0 v0, T1 v1, T2 v2, Func<T0, T1, T2, TR> action) => action(v0, v1, v2);
    public static TR With<T0, T1, T2, T3, TR>(T0 v0, T1 v1, T2 v2, T3 v3, Func<T0, T1, T2, T3, TR> action) => action(v0, v1, v2, v3);
    public static TR With<T0, T1, T2, T3, T4, TR>(T0 v0, T1 v1, T2 v2, T3 v3, T4 v4, Func<T0, T1, T2, T3, T4, TR> action) => action(v0, v1, v2, v3, v4);

    public static IEnumerable<T> Generate<T>(T initialState, Func<T, bool> condition, Func<T, T> iterate) =>
        EnumerableEx.Generate(initialState, condition, iterate, i => i);
}
