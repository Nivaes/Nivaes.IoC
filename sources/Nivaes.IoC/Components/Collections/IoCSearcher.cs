namespace Nivaes.IoC
{
    using System.Diagnostics.CodeAnalysis;

    public class IoCSearcher<TValue> : ISearcher<TValue>
    {
        private readonly KeyValuePair<int, TValue>[] mValues;

        internal IoCSearcher(IEnumerable<KeyValuePair<int, TValue>> source)
        {
            mValues = source.OrderBy((o) => o.Key, new IoCComparer()).ToArray();
        }

        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value)
        {
            ReadOnlySpan<KeyValuePair<int, TValue>> spanValue = new(mValues);

            var high = spanValue.Length - 1;
            var low = 0;

            while (low <= high)
            {
                int mid = (high + low) / 2;
                var midKey = spanValue[mid].Key;

                if (midKey == key)
                {
                    value = spanValue[mid].Value;
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
