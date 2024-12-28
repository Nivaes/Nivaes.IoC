namespace Nivaes.IoC
{
    using System.Collections.Frozen;
    
    public static class SearcherExtension
    {
        public static ISearcher<TValue> ToIoTSeeker<TValue>(this IEnumerable<KeyValuePair<int, TValue>> source)
        {
            return new IoTSearcher<TValue>(source);
        }

        public static ISearcher<TValue> ToDictionarySeeker<TValue>(this IDictionary<int, TValue> source)
        {
            return new DictionarySearcher<TValue>(source);
        }

        public static ISearcher<TValue> ToFrozenSeeker<TValue>(this IEnumerable<KeyValuePair<int, TValue>> source)
        {
            return new DictionarySearcher<TValue>(source.ToFrozenDictionary());
        }
    }
}
