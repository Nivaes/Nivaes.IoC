namespace Nivaes.IoC
{
    using System.Collections.Frozen;
    
    public static class SearcherExtension
    {
        public static ISearcher<TValue> ToIoTSearcher<TValue>(this IEnumerable<KeyValuePair<int, TValue>> source)
        {
            return new IoCSearcher<TValue>(source);
        }

        public static ISearcher<TValue> ToDictionarySearcher<TValue>(this IDictionary<int, TValue> source)
        {
            return new DictionarySearcher<TValue>(source);
        }

        public static ISearcher<TValue> ToFrozenSearcher<TValue>(this IEnumerable<KeyValuePair<int, TValue>> source)
        {
            return new DictionarySearcher<TValue>(source.ToFrozenDictionary());
        }
    }
}
