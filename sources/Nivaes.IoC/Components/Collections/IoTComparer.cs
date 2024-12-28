namespace Nivaes.IoC
{
    internal class IoTComparer : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            return x.CompareTo(y);
        }
    }
}
