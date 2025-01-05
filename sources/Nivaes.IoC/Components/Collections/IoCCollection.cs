namespace Nivaes.IoC
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class IoCCollection<TValue>
        where TValue : IInstanceResolver
    {
        private KeyInstanceResolverValue<TValue>[] mValues;

        internal IoCCollection(int length = 0)
        {
            mValues = new KeyInstanceResolverValue<TValue>[length];
        }

        public IoCCollection(KeyInstanceResolverValue<TValue>[] source)
        {
            mValues = source;
            var keyInstanceResolverValues = new Span<KeyInstanceResolverValue<TValue>>(mValues);
            keyInstanceResolverValues.Sort(new KeyInstanceResolverValueComparer<TValue>());
        }

        public void Add(Type key, TValue value)
        {
            int keyHash = key.GetHashCode();
            Add(keyHash, value);
        }

        private void Add(int keyHash, TValue value)
        {
            var spanValues = new Span<KeyInstanceResolverValue<TValue>>(mValues);

            int index = spanValues.BinarySearch(new KeyInstanceResolverValue<TValue>(key: keyHash), new KeyInstanceResolverValueComparer<TValue>());
            if (index < 0)
            {
                index = ~index;
                var newValues = new KeyInstanceResolverValue<TValue>[mValues.Length + 1];
                var newSpanValues = new Span<KeyInstanceResolverValue<TValue>>(newValues);
                var first = mValues.AsSpan(0, index);
                var second = mValues.AsSpan(index);

                first.CopyTo(newSpanValues.Slice(0, first.Length));
                second.CopyTo(newSpanValues.Slice(index + 1, second.Length));
                newSpanValues[index] = new KeyInstanceResolverValue<TValue>(key: keyHash, value: value);
                mValues = newValues;
            }
            else
            {
                spanValues[index] = new KeyInstanceResolverValue<TValue>(key: keyHash, value: value);
            }
        }

        internal void Replace(Type type, TValue value)
        {
            int key = type.GetHashCode();
            var result = TryGetPosition(key, out int position);
            if (result)
            {
                mValues[position] = new KeyInstanceResolverValue<TValue>(key: key, value: value);
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
                value = mValues[position].Value;
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

        internal void Merge(IoCCollection<TValue> newValues)
        {
            var oldValues = mValues;
            var allValues = new KeyInstanceResolverValue<TValue>[oldValues.Length + newValues.mValues.Length];
            int i = 0, j = 0, m = 0;

            while (i < oldValues.Length && j < newValues.mValues.Length)
            {
                if (oldValues[i].Key < newValues.mValues[j].Key)
                {
                    allValues[m++] = oldValues[i++];
                }
                else
                {
                    allValues[m++] = newValues.mValues[j++];
                }
            }
            while (i < oldValues.Length)
            {
                allValues[m++] = oldValues[i++];
            }
            while (j < newValues.mValues.Length)
            {
                allValues[m++] = newValues.mValues[j++];
            }

            mValues = allValues;
        }

        internal IEnumerable<TValue> Values => mValues.Select((o) => (TValue)o.Value);

        public IoCCollection<TValue> Clone()
        {
            IoCCollection<TValue> clone = new IoCCollection<TValue>(mValues.Length);

            for (int i = 0; i < mValues.Length; i++)
            {
                clone.mValues[i] = new KeyInstanceResolverValue<TValue>(key: mValues[i].Key, value: (TValue)mValues[i].Value.Duplicate());
            }

            return clone;
        }
    }
}
