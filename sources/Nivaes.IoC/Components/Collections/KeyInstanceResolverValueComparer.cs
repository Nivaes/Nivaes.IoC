namespace Nivaes.IoC
{
    using System.Collections;

    internal class KeyInstanceResolverValueComparer : IComparer
    {
        public int Compare(object? x, object? y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            return ((KeyInstanceResolverValue)x).Key.CompareTo(((KeyInstanceResolverValue)y).Key);
        }
    }
}
