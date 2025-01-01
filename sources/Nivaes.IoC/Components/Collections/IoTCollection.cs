namespace Nivaes.IoC
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;

    public class IoTCollection<TValue> //: ISearcher<TValue>
        where TValue : IInstanceResolver
    {
        #region PairValues
        private struct PairValues
        {
            public int Key;
            public TValue Value;
        }

        internal class PairValuesComparer : IComparer
        {
            public int Compare(object? x, object? y)
            {
                ArgumentNullException.ThrowIfNull(x);
                ArgumentNullException.ThrowIfNull(y);

                return ((PairValues)x).Key.CompareTo(((PairValues)y).Key);
            }
        }
        #endregion

        private PairValues[] mValues = new PairValues[0];

        //internal IoTCollection()
        //{
        //    mValues = new PairValues[0];
        //    //mValues = source.OrderBy((o) => o.Key, new IoTComparer()).ToArray();
        //}

        public void Add(Type key, TValue value)
        {
            int keyHash = key.GetHashCode();
            Add(keyHash, value);
        }

        private void Add(int keyHash, TValue value)
        {
            int index = Array.BinarySearch(mValues, new PairValues { Key = keyHash }, new PairValuesComparer());
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

        internal void Replace(Type type, TValue value)
        {
            int key = type.GetHashCode();
            var result = TryGetPosition(key, out int position);
            if (result)
            {
                mValues[position] = new PairValues { Key = key, Value = value };
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
            ReadOnlySpan<PairValues> spanValue = new ReadOnlySpan<PairValues>(mValues);

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
                Add(value.Key, value.Value);
            }
        }

        internal IEnumerable<TValue> Values => mValues.Select((o) => o.Value);

        public IoTCollection<TValue> Clone()
        {
            IoTCollection<TValue> clone = new IoTCollection<TValue>
            {
                mValues = new PairValues[mValues.Length]
            };

            for (int i = 0; i < mValues.Length; i++)
            {
                clone.mValues[i] = new PairValues { Key = mValues[i].Key, Value = (TValue)mValues[i].Value.Duplicate() };
            }

            return clone;
        }
    }
}
