namespace Nivaes.IoC
{
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    public class IoTCollection<TValue> //: ISearcher<TValue>
    {
        private struct PairValues
        {
            public int Key;
            public TValue Value;
        }

        private PairValues[] mValues = new PairValues[0];

        //internal IoTCollection()
        //{
        //    mValues = new PairValues[0];
        //    //mValues = source.OrderBy((o) => o.Key, new IoTComparer()).ToArray();
        //}

        internal void Add(Type key, TValue value)
        {
            int keyHash = key.GetHashCode();

            int index = Array.BinarySearch(mValues, new PairValues { Key = keyHash }, new IoTComparer());
            if (index < 0)
            {
                index = ~index;
                Array.Resize(ref mValues, mValues.Length + 1);
                Array.Copy(mValues, index, mValues, index + 1, mValues.Length - index - 1);
                mValues[index] = new PairValues { Key = keyHash, Value = value };
            }
            else
            {
                mValues[index] = new PairValues { Key = keyHash, Value = value };
            }
        }

        internal void Replace(Type key, TValue value)
        {
            throw new NotImplementedException();
        }

        internal bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value)
        {
            ReadOnlySpan<PairValues> spanValue = new ReadOnlySpan<PairValues>(mValues);

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

        internal void Merge(IoTCollection<TValue> collection)
        {
            throw new NotImplementedException();
            //foreach (var value in collection.mValues)
            //{
            //    Add(value.Key, value.Value);
            //}
        }

        internal IEnumerable<TValue> Values => mValues.Select((o) => o.Value);

        public IoTCollection<TValue> Clone()
        {
            IoTCollection<TValue> clone = new IoTCollection<TValue>
            {
                mValues = new PairValues[mValues.Length]
            };

            Array.Copy(mValues, clone.mValues, mValues.Length);
            return clone;
        }
    }
}
