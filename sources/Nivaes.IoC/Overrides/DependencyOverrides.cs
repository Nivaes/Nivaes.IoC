namespace Nivaes.IoC.Core.Overrides
{
    public class DependencyOverrides
    {
        public Dictionary<Type, Func<object>> Overrides { get; } = new Dictionary<Type, Func<object>>();
    }
}
