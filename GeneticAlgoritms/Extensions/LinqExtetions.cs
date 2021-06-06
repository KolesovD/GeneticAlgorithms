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

    public static IEnumerable<T> FromParams<T>(params T[] _data) 
    {
        return _data.AsEnumerable();
    }

    public static SortedSet<T> ToSortedSet<T>(this IEnumerable<T> source) where T : IComparable<T> 
    {
        SortedSet<T> result = new SortedSet<T>();

        foreach (var item in source)
        {
            result.Add(item);
        }

        return result;
    }

    public static IEnumerable<LinkedListNode<T>> GetAllNodes<T>(this LinkedList<T> data) 
    {
        if (data.Count == 0) yield break;

        LinkedListNode<T> _temp = data.First;

        do
        {
            yield return _temp;
            _temp = _temp.Next;
        }
        while (_temp.Next != default);
    }
}
