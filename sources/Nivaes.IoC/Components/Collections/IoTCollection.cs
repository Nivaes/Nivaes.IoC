namespace Nivaes.IoC
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class IoTCollection<TValue>
        where TValue : IInstanceResolver
    {
        private KeyInstanceResolverValue[] mValues;

        internal IoTCollection(int length)
        {
            mValues = new KeyInstanceResolverValue[length];
        }

        public IoTCollection(KeyInstanceResolverValue[] source)
        {
            mValues = source;
            var keyInstanceResolverValues = new Span<KeyInstanceResolverValue>(mValues);
            keyInstanceResolverValues.Sort(new KeyInstanceResolverValueComparer());
        }

        public void Add(Type key, TValue value)
        {
            int keyHash = key.GetHashCode();
            Add(keyHash, value);
        }

        private void Add(int keyHash, TValue value)
        {
            var spanValues = new Span<KeyInstanceResolverValue>(mValues);

            int index = spanValues.BinarySearch(new KeyInstanceResolverValue(key: keyHash), new KeyInstanceResolverValueComparer());
            if (index < 0)
            {
                index = ~index;
                var newValues = new KeyInstanceResolverValue[mValues.Length + 1];
                var newSpanValues = new Span<KeyInstanceResolverValue>(newValues);
                var first = mValues.AsSpan(0, index);
                var second = mValues.AsSpan(index);

                first.CopyTo(newSpanValues.Slice(0, first.Length));
                second.CopyTo(newSpanValues.Slice(index + 1, second.Length));
                newSpanValues[index] = new KeyInstanceResolverValue(key: keyHash, value: value);
                mValues = newValues;
            }
            else
            {
                spanValues[index] = new KeyInstanceResolverValue(key: keyHash, value: value);
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
            var high = mValues.Length - 1;
            var low = 0;

            while (low <= high)
            {
                int mid = (high + low) / 2;
                var midKey = mValues[mid].Key;

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

        internal void Merge(IoTCollection<TValue> newValues)
        {
            var oldValues = mValues;
            mValues = new KeyInstanceResolverValue[oldValues.Length + newValues.mValues.Length];
            int i = 0, j = 0, m = 0;

            while (i < oldValues.Length && j < newValues.mValues.Length)
            {
                if (oldValues[i].Key < newValues.mValues[j].Key)
                {
                    mValues[m++] = oldValues[i++];
                }
                else
                {
                    mValues[m++] = newValues.mValues[j++];
                }
            }
            while (i < oldValues.Length)
            {
                mValues[m++] = oldValues[i++];
            }
            while (j < newValues.mValues.Length)
            {
                mValues[m++] = newValues.mValues[j++];
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
