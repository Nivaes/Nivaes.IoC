namespace Nivaes.IoC;

public struct KeyInstanceResolverValue<TValue>
    : IKeyInstanceResolverValue
     where TValue : IInstanceResolver
{
    public int Key { get; }
    public TValue? Value { get; }

    object? IKeyInstanceResolverValue.Value => Value;

    public KeyInstanceResolverValue(int key)
    {
        Key = key;
    }

    public KeyInstanceResolverValue(Type type, TValue? value)
    {
        Key = type.GetHashCode();
        Value = value;
    }

    public KeyInstanceResolverValue(int key, TValue? value)
    {
        Key = key;
        Value = value;
    }
}
