namespace Nivaes.IoC
{
    public struct KeyInstanceResolverValue<TValue>
        : IKeyInstanceResolverValue
         where TValue : IInstanceResolver
    {
        public int Key { get; }
        public TValue Value { get; }

        object IKeyInstanceResolverValue.Value => Value;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public KeyInstanceResolverValue(int key)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        {
            Key = key;
        }

        public KeyInstanceResolverValue(Type type, TValue value)
        {
            Key = type.GetHashCode();
            Value = value;
        }

        public KeyInstanceResolverValue(int key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }
}
