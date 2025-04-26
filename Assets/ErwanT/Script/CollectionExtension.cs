using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CollectionExtension
{
    public static T GetRandomElement<T>(this ICollection<T> collection)
    {
        if (collection.Count == 0)
            return default(T);
        int index = Random.Range(0, collection.Count);
        T elem = collection.ElementAt(index);
        return elem;
    }

    public static void Shuffle<T>(this List<T> list)
    {
        var count = list.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = Random.Range(i, count);
            var tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }
}
