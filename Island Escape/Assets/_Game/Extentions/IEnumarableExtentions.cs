using System;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtensions
{
    /// <summary>
    /// Returns random element from collection
    /// </summary>
    public static T GetRandom<T>(this IEnumerable<T> source, Random random = null)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        random ??= new Random();

        var list = source as IList<T> ?? source.ToList();

        if (list.Count == 0)
            throw new InvalidOperationException("Коллекция пуста.");

        int index = random.Next(0, list.Count);
        return list[index];
    }

    /// <summary>
    /// Returns random element from collection and remove it from list
    /// </summary>
    public static T PopRandom<T>(this IList<T> source)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        if (source.Count == 0)
            throw new InvalidOperationException("Коллекция пуста.");

        int index = UnityEngine.Random.Range(0, source.Count);
        T item = source[index];
        source.RemoveAt(index);
        return item;
    }
}