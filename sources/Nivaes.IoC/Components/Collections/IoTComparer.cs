namespace Nivaes.IoC
{
    using System.Collections;

    internal class IoTComparer : IComparer<int>, IComparer
    {
        public int Compare(int x, int y)
        {
            return x.CompareTo(y);
        }

        public int Compare(object? x, object? y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            return ((int)x).CompareTo((int)y);
        }
    }
}
