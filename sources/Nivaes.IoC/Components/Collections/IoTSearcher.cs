namespace Nivaes.IoC
{
    using System.Diagnostics.CodeAnalysis;

    public class IoTSearcher<TValue> : ISearcher<TValue>
    {
        private readonly KeyValuePair<int, TValue>[] mArray;

        internal IoTSearcher(IEnumerable<KeyValuePair<int, TValue>> source)
        {
            mArray = source.OrderBy((o) => o.Key, new IoTComparer()).ToArray();
        }

        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value)
        {
            var high = mArray.Length - 1;
            var low = 0;

            while (low <= high)
            {
                int mid = (high + low) / 2;
                var midKey = mArray[mid].Key;

                if (midKey == key)
                {
                    value = mArray[mid].Value;
                    return true;
                }
                else
                {
                    if (key < midKey)
                        high = mid - 1;
                    else
                        low = mid + 1;
                }
            }
            value = default;
            return false;
        }
    }
}
