namespace Nivaes.IoC;

public class DependencyOverrides
{
    public Dictionary<Type, Func<object?>> Overrides { get; } = new Dictionary<Type, Func<object?>>();
}
