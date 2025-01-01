namespace Nivaes.IoC
{
    using System.Collections;

    internal class KeyInstanceResolverValueComparer : IComparer<KeyInstanceResolverValue>, IComparer
    {
        public int Compare(object? x, object? y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            return ((KeyInstanceResolverValue)x).Key.CompareTo(((KeyInstanceResolverValue)y).Key);
        }

        public int Compare(KeyInstanceResolverValue x, KeyInstanceResolverValue y)
        {
            return x.Key.CompareTo(y.Key);
        }
    }
}
