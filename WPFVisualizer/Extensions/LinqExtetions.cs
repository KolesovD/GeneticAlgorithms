using System.Collections.Generic;
using System.Linq;
using System;

public static class LinqExtetions
{
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        var enumerator = source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            action?.Invoke(enumerator.Current);
        }
    }

    public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
    {
        var enumerator = source.Select((data, i) => (data, i)).GetEnumerator();
        while (enumerator.MoveNext())
        {
            action?.Invoke(enumerator.Current.data, enumerator.Current.i);
        }
    }
}
