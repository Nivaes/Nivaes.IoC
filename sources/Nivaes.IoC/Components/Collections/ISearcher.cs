namespace Nivaes.IoC
{
    using System.Diagnostics.CodeAnalysis;

    public interface ISearcher<TValue>
    {
        public bool TryGetValue(int key, [MaybeNullWhen(false)] out TValue value);
    }
}
