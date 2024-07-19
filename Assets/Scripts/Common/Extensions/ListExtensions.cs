using System;
using System.Collections.Generic;
using System.Linq;

public static class ListExtensions
{
    public static bool IsNullOrEmpty<T>(this IList<T> list)
    {
        return list == null || !list.Any();
    }

    public static List<T> Clone<T>(this IList<T> list)
    {
        List<T> newList = new List<T>();
        foreach (T item in list)
        {
            newList.Add(item);
        }

        return newList;
    }

    public static void Swap<T>(this IList<T> list, int indexA, int indexB)
    {
        (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
    }

    static Random rng;
    public static IList<T> Shuffle<T>(this IList<T> list)
    {
        if (rng == null) rng = new Random();
        int count = list.Count;
        while (count > 1)
        {
            --count;
            int index = rng.Next(count + 1);
            (list[index], list[count]) = (list[count], list[index]);
        }
        return list;
    }
}