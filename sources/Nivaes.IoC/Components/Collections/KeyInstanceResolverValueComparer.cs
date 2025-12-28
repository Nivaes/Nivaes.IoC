namespace Nivaes.IoC;

using System.Collections;

internal class KeyInstanceResolverValueComparer<TValue>
    : IComparer<KeyInstanceResolverValue<TValue>>, IComparer
    where TValue : IInstanceResolver
{
    public int Compare(object? x, object? y)
    {
        if (x == null) return -1;
        if (y == null) return 1;

        return ((KeyInstanceResolverValue<TValue>)x).Key.CompareTo(((KeyInstanceResolverValue<TValue>)y).Key);
    }

    public int Compare(KeyInstanceResolverValue<TValue> x, KeyInstanceResolverValue<TValue> y)
    {
        return x.Key.CompareTo(y.Key);
    }
}
