namespace Nivaes.IoC
{
    public struct KeyInstanceResolverValue
    {
        public int Key { get; }
        public IInstanceResolver Value { get; }

        public KeyInstanceResolverValue(int key)
        {
            Key = key;
        }

        public KeyInstanceResolverValue(Type type, IInstanceResolver value)
        {
            Key = type.GetHashCode();
            Value = value;
        }

        public KeyInstanceResolverValue(int key, IInstanceResolver value)
        {
            Key = key;
            Value = value;
        }
    }
}
