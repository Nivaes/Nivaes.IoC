namespace Nivaes.IoC
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class IoTCollection<TValue>
        where TValue : IInstanceResolver
    {
        #region PairValues
      
        #endregion

        private KeyInstanceResolverValue[] mValues;

        internal IoTCollection(int length)
        {
            mValues = new KeyInstanceResolverValue[length];
        }

        public IoTCollection(IEnumerable<KeyInstanceResolverValue> source)
        {
            mValues = source.ToArray();
            var keyInstanceResolverValues = new Span<KeyInstanceResolverValue>(mValues);
            keyInstanceResolverValues.Sort(new KeyInstanceResolverValueComparer());

            //mValues = source.OrderBy((o) => o.Key, new IoTComparer()).ToArray();
        }

        public void Add(Type key, TValue value)
        {
            int keyHash = key.GetHashCode();
            Add(keyHash, value);
        }

        private void Add(int keyHash, TValue value)
        {
            int index = Array.BinarySearch(mValues, new KeyInstanceResolverValue(key: keyHash), new KeyInstanceResolverValueComparer());
            if (index < 0)
            {
                index = ~index;
                Array.Resize(ref mValues, mValues.Length + 1);
                Array.Copy(mValues, index, mValues, index + 1, mValues.Length - index - 1);
                mValues[index] = new KeyInstanceResolverValue (key: keyHash, value: value);
            }
            else
            {
                mValues[index] = new KeyInstanceResolverValue (key: keyHash, value: value);
            }
        }

        internal void Replace(Type type, TValue value)
        {
            int key = type.GetHashCode();
            var result = TryGetPosition(key, out int position);
            if (result)
            {
                mValues[position] = new KeyInstanceResolverValue(key: key, value: value);
            }
            else
            {
                Add(key, value);
            }
        }

        internal bool TryGetValue(Type type, [MaybeNullWhen(false)] out TValue value)
        {
            int key = type.GetHashCode();

            var result = TryGetPosition(key, out int position);
            if (result)
            {
                value = (TValue)mValues[position].Value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }

        internal bool TryGetPosition(int key, [MaybeNullWhen(false)] out int position)
        {
            ReadOnlySpan<KeyInstanceResolverValue> spanValue = new ReadOnlySpan<KeyInstanceResolverValue>(mValues);

            var high = spanValue.Length - 1;
            var low = 0;

            while (low <= high)
            {
                int mid = (high + low) / 2;
                var midKey = spanValue[mid].Key;

                if (midKey == key)
                {
                    position = mid;
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
            position = -1;
            return false;
        }

        internal void Merge(IoTCollection<TValue> collection)
        {
            foreach (var value in collection.mValues)
            {
                Add(value.Key, (TValue)value.Value);
            }
        }

        internal IEnumerable<TValue> Values => mValues.Select((o) => (TValue)o.Value);

        public IoTCollection<TValue> Clone()
        {
            IoTCollection<TValue> clone = new IoTCollection<TValue>(mValues.Length);

            for (int i = 0; i < mValues.Length; i++)
            {
                clone.mValues[i] = new KeyInstanceResolverValue(key: mValues[i].Key, value: (TValue)mValues[i].Value.Duplicate());
            }

            return clone;
        }
    }
}
