namespace Nivaes.IoC
{
    using System.Diagnostics.CodeAnalysis;

    public class DictionarySearcher<TValue> : ISearcher<TValue>
    {
        private readonly IDictionary<int, TValue> mSource;

        internal DictionarySearcher(IDictionary<int, TValue> source)
        {
            mSource = source;
        }

        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value)
        {
            return mSource.TryGetValue(key, out value);
        }
    }
}
