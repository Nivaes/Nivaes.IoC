namespace Nivaes.IoC;

public interface IIoCResolver : IDisposable
{
    IIoCResolver CreateScope();

    object? Resolve(Type serviceType);

    bool TryResolve(Type serviceType, out object? result);

    object? Resolve(Type type, IOverrides overrides);
    
    bool TryResolve(Type serviceType, IOverrides overrides, out object? result);
}
